using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Database.Context;
using XMessenger.Database.Services;

namespace XMessenger.Database.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(configuration["DbConnectionStrings:DatabaseDb:SqlServer"]);
                //options.UseNpgsql(configuration["DbConnectionStrings:DatabaseDb:PostgreSQL"]);
                //options.UseSqlite(configuration["DbConnectionStrings:DatabaseDb:Sqlite"]);
            });


            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<ISettlementService, SettlementService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IMetroService, MetroService>();
        }
    }
}
