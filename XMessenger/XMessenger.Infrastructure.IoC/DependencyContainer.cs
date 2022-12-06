using Extensions.DeviceDetector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Application.Services;
using XMessenger.Application.Sessions;
using XMessenger.Helpers.Services;
using XMessenger.Infrastructure.Data.EntityFramework.Context;

namespace XMessenger.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddXMessengerServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Db

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(configuration["DbConnectionStrings:IdentityDb:SqlServer"]);
                //options.UseNpgsql(configuration["DbConnectionStrings:IdentityDb:PostgreSQL"]);
                //options.UseSqlite(configuration["DbConnectionStrings:IdentityDb:Sqlite"]);
            });

            #endregion

            #region Services

            services.AddSingleton<ISessionManager, SessionManager>();

            services.AddScoped<IIdentityService, HttpIdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITokenService, TokenService>();

            #endregion


            services.AddHttpContextAccessor();

            services.AddDeviceDetector();

            return services;
        }
    }
}
