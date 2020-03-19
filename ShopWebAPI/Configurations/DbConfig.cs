using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopWebAPI.BL.Services;
using ShopWebAPI.BL.Services.Interfaices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.Configurations
{
    public class DbConfig : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProductService, ProductService>();
        }
    }
}
