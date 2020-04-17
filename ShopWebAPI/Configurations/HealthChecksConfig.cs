using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopWebAPI.Configurations.Interfaices;
using ShopWebAPI.Data;

namespace ShopWebAPI.Configurations
{
    public class HealthChecksConfig : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
                //.AddDbContextCheck<DataContext>();
        }
    }
}
