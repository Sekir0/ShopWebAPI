﻿using ShopWebAPI.DAL.Domain;
using System.Threading.Tasks;

namespace ShopWebAPI.BL.Services.Interfaices
{
    public interface IIdentityService
    {
        Task<AuthenticatioResult> RegisterAsynk(string email, string password);

        Task<AuthenticatioResult> LoginAsynk(string email, string password);

        Task<AuthenticatioResult> RefreshTokenAsynk(string token, string refreshtoken);
    }
}
