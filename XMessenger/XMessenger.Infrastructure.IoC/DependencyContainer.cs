using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Database.Extensions;
using XMessenger.Identity.Extensions;

namespace XMessenger.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddXMessengerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityServices(configuration);
            services.AddDatabaseServices(configuration);

            return services;
        }
    }
}
