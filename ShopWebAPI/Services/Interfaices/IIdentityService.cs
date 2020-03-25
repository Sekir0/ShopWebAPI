using ShopWebAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.Services.Interfaices
{
    public interface IIdentityService
    {
        Task<AuthenticatioResult> RegisterAsynk(string email, string password);
    }
}
