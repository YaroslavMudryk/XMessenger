using XMessenger.Database.Export;

namespace XMessenger.Database.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(configuration["DbConnectionStrings:DatabaseDb:SqlServer"]);
                //options.UseNpgsql(configuration["DbConnectionStrings:DatabaseDb:PostgreSQL"]);
                //options.UseSqlite(configuration["DbConnectionStrings:DatabaseDb:Sqlite"]);
            });


            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<ISettlementService, SettlementService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IMetroService, MetroService>();

            services.AddScoped<ISeederService, DatabaseSeederService>();
            services.AddScoped<IExportDatabaseService, FileExportDatabaseService>();
        }
    }
}