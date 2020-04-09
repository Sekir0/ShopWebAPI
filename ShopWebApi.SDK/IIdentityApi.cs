using System.Threading.Tasks;
using Refit;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;

namespace ShopWebApi.SDK
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<AuthSuccessResponse>> RegisterAsynk([Body] UserRegistrationRequest registrationRequest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsynk([Body] UserLoginRequest loginRequest);

        [Post("/api/v1/identity/refresh")]
        Task<ApiResponse<AuthSuccessResponse>> TokenRefreshAsynk([Body] RefreshTokenRequest refreshRequest);
    }
}
