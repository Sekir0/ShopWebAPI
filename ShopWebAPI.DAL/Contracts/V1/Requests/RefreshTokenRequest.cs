using System;
using System.Collections.Generic;
using System.Text;

namespace ShopWebAPI.DAL.Contracts.V1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
