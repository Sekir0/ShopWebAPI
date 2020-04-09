

namespace ShopWebAPI.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Products
        {
            public const string GetAll  = Base + "/products";
            public const string Create  = Base + "/products";
            public const string GetById = Base + "/products/{productId}";
            public const string Update  = Base + "/products/{productId}";
            public const string Delete  = Base + "/products/{productId}";
        }
        public static class Identity
        {
            public const string Login        = Base + "/identity/login";
            public const string Register     = Base + "/identity/register";
            public const string RefreshToken = Base + "/identity/refresh";
        }
        public static class Categorys
        {
            public const string GetAll    = Base + "/categorys";
            public const string Create    = Base + "/categorys";
            public const string GetByName = Base + "/categorys/{categoryName}";
            public const string Delete    = Base + "/categorys/{categorysId}";
        }
    }
   
    
}
