using Extensions.DeviceDetector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Application.Seeder;
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
                var conn = configuration["DbConnectionStrings:IdentityDb:SqlServer"];
                options.UseSqlServer(conn);
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
            services.AddScoped<ISeederService, BaseSeederService>();

            #endregion


            services.AddHttpContextAccessor();

            services.AddDeviceDetector();

            return services;
        }
    }
}
