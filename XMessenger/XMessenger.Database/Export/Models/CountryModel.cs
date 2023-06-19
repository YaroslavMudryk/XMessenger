namespace XMessenger.Database.Export.Models
{
    public class CountryModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Populations { get; set; }
        public List<RegionModel> Regions { get; set; }
    }
}
