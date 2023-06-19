namespace XMessenger.Database.Import
{
    public class JsonFileCountryDataImport : ICountryData
    {
        private readonly string _countryUAPath;
        private readonly DatabaseContext _db;
        public JsonFileCountryDataImport(DatabaseContext db)
        {
            _db = db;
            _countryUAPath = "country-ua.json";
        }

        public async Task<Country> ExportCountryDataAsync(string name)
        {
            var country = await _db.Countries.Include(s => s.Regions).FirstOrDefaultAsync(s => s.Name == name);

            var json = JsonSerializer.Serialize(country, new JsonSerializerOptions
            {
                MaxDepth = 1
            });

            using (var sw = new StreamWriter($"country({DateTime.Now.ToString("HH:mm dd.MM.yyyy")}).json"))
            {
                await sw.WriteAsync(json);
                sw.Close();
            }

            return country;
        }

        public async Task<Country> ImportCountryDataAsync()
        {
            var json = await File.ReadAllTextAsync(_countryUAPath);

            var country = JsonSerializer.Deserialize<Country>(json);

            SortNames(country);

            if (await _db.Countries.FirstOrDefaultAsync(s => s.Name == country.Name) == null)
            {
                await _db.Countries.AddAsync(country);
                await _db.SaveChangesAsync();
            }

            return country;
        }

        private void SortNames(Country country)
        {
            country.Regions.ForEach(region =>
            {
                region.Areas.ForEach(area =>
                {
                    area.Settlements = area.Settlements.OrderBy(s => s.Name).ToList();
                });
                region.Areas = region.Areas.OrderBy(s => s.Name).ToList();
            });
            country.Regions = country.Regions.OrderBy(s => s.Name).ToList();
        }
    }
}