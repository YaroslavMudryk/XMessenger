using AngleSharp;
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using XMessenger.Database.Db.Context;
using XMessenger.Helpers.Extensions;

namespace XMessenger.Database.Import
{
    public interface ICountryDataImport
    {
        Task<Country> ImportCountryDataAsync();
    }

    public class MapsVlasenkoCountryDataImport : ICountryDataImport
    {
        private readonly string _countryUAPath;
        private readonly DatabaseContext _db;
        private readonly string _url = "https://maps.vlasenko.net/list/ukraine/";
        private readonly IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        private readonly Dictionary<string, SettlementType> _settlementTypes;

        public MapsVlasenkoCountryDataImport(DatabaseContext db)
        {
            _db = db;
            _settlementTypes = new Dictionary<string, SettlementType>
            {
                ["С"] = SettlementType.Village,
                ["С-ЩЕ"] = SettlementType.Village,
                ["СМТ"] = SettlementType.UrbanVillage,
                ["М"] = SettlementType.City
            };
            _countryUAPath = "country-uk.json";
        }
        public async Task<Country> ImportCountryDataAsync()
        {
            var country = new Country();
            country.Name = "Україна";
            country.Populations = "46 млн осіб";
            country.Flag = "https://upload.wikimedia.org/wikipedia/commons/d/d2/Flag_of_Ukraine.png";

            var mainPageDocument = await _context.OpenAsync(_url);

            country.Regions = GetRegions(mainPageDocument);

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = JsonSerializer.Serialize(country, jsonOptions);

            using (var sw = new StreamWriter(_countryUAPath))
            {
                await sw.WriteAsync(json);
                sw.Close();
            }

            if (await _db.Countries.FirstOrDefaultAsync(s => s.Name == country.Name) == null)
            {
                await _db.Countries.AddAsync(country);
                await _db.SaveChangesAsync();
            }

            return country;
        }

        private List<Region> GetRegions(IDocument document)
        {
            var regions = new List<Region>();

            var body = document.QuerySelector("body");

            var regionH2s = body.QuerySelectorAll("h2");

            foreach (var regionH2 in regionH2s)
            {
                var a = regionH2.QuerySelector("a");
                var url = _url + a.GetAttribute("href");
                var region = new Region
                {
                    Name = a.TextContent,
                    Areas = GetAreas(url)
                };
                regions.Add(region);
            }

            return regions.OrderBy(s => s.Name).ToList();
        }

        private List<Area> GetAreas(string url)
        {
            var areaList = new List<Area>();

            var areasPageDocument = _context.OpenAsync(url).GetAwaiter().GetResult();

            var body = areasPageDocument.QuerySelector("body");

            var areasH3s = body.QuerySelectorAll("h3");

            int indexArea = 0;

            foreach (var areaH3 in areasH3s)
            {
                areaList.Add(new Area
                {
                    Name = areaH3.TextContent,
                    Settlements = GetSettlements(areasPageDocument, indexArea++)
                });
            }

            return areaList.OrderBy(s => s.Name).ToList();
        }

        private List<Settlement> GetSettlements(IDocument document, int index)
        {
            var listSettlements = new List<Settlement>();

            var body = document.QuerySelector("body");

            var allSettlements = body.QuerySelectorAll("blockquote");

            var neededSettlementsByRegion = allSettlements[index];

            var allIs = neededSettlementsByRegion.QuerySelectorAll("i");
            var allBs = neededSettlementsByRegion.QuerySelectorAll("b");

            int currentIndex = 0;
            foreach (var i in allIs)
            {
                var smallElement = i.QuerySelector("small");
                var aElement = allBs[currentIndex++].QuerySelector("a");

                if (string.IsNullOrEmpty(aElement.TextContent) || string.IsNullOrEmpty(smallElement.TextContent))
                    continue;

                var newSettlement = new Settlement
                {
                    Name = aElement.TextContent.ToUpperFirstLatter(),
                    Type = _settlementTypes[smallElement.TextContent]
                };
                listSettlements.Add(newSettlement);
            }

            return listSettlements.OrderBy(s => s.Name).ToList();
        }
    }

    public class JsonFileCountryDataImport : ICountryDataImport
    {
        private readonly string _countryUAPath;
        private readonly DatabaseContext _db;
        public JsonFileCountryDataImport(DatabaseContext db)
        {
            _db = db;
            _countryUAPath = "country-ua.json";
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