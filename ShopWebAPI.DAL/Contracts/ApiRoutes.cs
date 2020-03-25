using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.DAL.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Products
        {
            public const string GetAllAsynk = Base + "/products";
            public const string CreateAsynk = Base + "/products";
            public const string GetAsynk = Base + "/products/{productId}";
            public const string UpdateAsynk = Base + "/products/{productId}";
            public const string DeleteAsynk = Base + "/products/{productId}";
        }
        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
        }
    }
   
    
}
