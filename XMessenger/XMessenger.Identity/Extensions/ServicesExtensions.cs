namespace XMessenger.Identity.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(configuration["DbConnectionStrings:IdentityDb:SqlServer"]);
                //options.UseNpgsql(configuration["DbConnectionStrings:IdentityDb:PostgreSQL"]);
                //options.UseSqlite(configuration["DbConnectionStrings:IdentityDb:Sqlite"]);
            });


            services.AddSingleton<ISessionManager, SessionManager>();

            services.AddScoped<IIdentityService, HttpIdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<ISeederService, IdentitySeederService>();

            services.AddHttpContextAccessor();

            services.AddDeviceDetector();
        }
    }
}
