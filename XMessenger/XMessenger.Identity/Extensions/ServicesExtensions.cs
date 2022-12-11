using Extensions.DeviceDetector;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Helpers.Services;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Seeder;
using XMessenger.Identity.Services;
using XMessenger.Identity.Sessions;

namespace XMessenger.Identity.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                var conn = configuration["DbConnectionStrings:IdentityDb:SqlServer"];
                options.UseSqlServer(conn);
                //options.UseNpgsql(configuration["DbConnectionStrings:IdentityDb:PostgreSQL"]);
                //options.UseSqlite(configuration["DbConnectionStrings:IdentityDb:Sqlite"]);
            });


            services.AddSingleton<ISessionManager, SessionManager>();

            services.AddScoped<IIdentityService, HttpIdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISeederService, BaseSeederService>();
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddHttpContextAccessor();

            services.AddDeviceDetector();
        }
    }
}
