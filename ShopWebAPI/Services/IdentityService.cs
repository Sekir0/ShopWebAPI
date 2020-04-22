using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using ShopWebAPI.DAL.Options;
using ShopWebAPI.Data;
using ShopWebAPI.DAL.Domain;
using ShopWebAPI.Services.Interfaices;

namespace ShopWebAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<IdentityUser> userManage, JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters, DataContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManage;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
            _roleManager = roleManager;
        }

        

        public async Task<AuthenticatioResult> RegisterAsynk(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticatioResult
                {
                    Errors = new[] { "User with this email already exists" }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticatioResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthResult(newUser);
        }

        public async Task<AuthenticatioResult> LoginAsynk(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticatioResult
                {
                    Errors = new[] { "Email dose not exists" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticatioResult
                {
                    Errors = new[] { "Email or Password dose not exists" }
                };
            }

            return await GenerateAuthResult(user);

        }

        private async Task<AuthenticatioResult> GenerateAuthResult(IdentityUser newUser)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                new Claim("id", newUser.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(newUser);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                UserId = newUser.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddHours(2)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthenticatioResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticatioResult> RefreshTokenAsynk(string token, string refreshtoken)
        {
            var validToken = GetPrincipalFromToken(token);

            if(validToken == null)
            {
                return new AuthenticatioResult { Errors = new[] { "Invalid Token" } };
            }

            var expiryDateUnix = long.Parse(validToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);
                

            if (expiryDateTimeUtc > DateTime.Now)
            {
                return new AuthenticatioResult { Errors = new[] { "This token hasn't expired yet" } };
            }

            var jti = validToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshtoken);

            if(storedRefreshToken == null)
            {
                return new AuthenticatioResult { Errors = new[] { "This refresh token doesn't exist" } };
            }

            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticatioResult { Errors = new[] { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticatioResult { Errors = new[] { "This refresh token has been invalidated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticatioResult { Errors = new[] { "This refresh token has been used" } };
            }

            if(storedRefreshToken.JwtId != jti)
            {
                return new AuthenticatioResult { Errors = new[] { "This refresh token doesn't match this JWT" } };
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthResult(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSequrityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSequrityAlgorithm(SecurityToken validedToken)
        {
            return (validedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
