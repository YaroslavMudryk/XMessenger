using XMessenger.Database.Models;

namespace XMessenger.Database.Export
{
    public interface IExportDatabaseService
    {
        Task<Result<List<Country>>> ExportAllCountriesWithAllDataAsync();
    }

    public class FileExportDatabaseService : IExportDatabaseService
    {
        private readonly DatabaseContext _db;
        public FileExportDatabaseService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<List<Country>>> ExportAllCountriesWithAllDataAsync()
        {
            var countries = await _db.Countries
                .Include(s => s.Regions)
                .ThenInclude(s => s.Areas)
                .ThenInclude(s => s.Settlements)
                .ToListAsync();

            var allUniversities = await _db.Universities.Include(s => s.Faculties).ToListAsync();
            var allMetro = await _db.Metro.Include(s => s.Lines).ThenInclude(s => s.Stations).ToListAsync();

            var fileName = $"export_countries_{DateTime.Now.ToString("HH:mm_dd.MM.yyyy")}.json";
            
            var json = JsonSerializer.Serialize(countries, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            var sw = new StreamWriter(fileName);

            await sw.WriteAsync(json);
            sw.Close();

            return Result<List<Country>>.SuccessWithData(countries);
        }
    }
}
