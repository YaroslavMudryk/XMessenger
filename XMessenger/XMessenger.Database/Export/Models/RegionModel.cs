namespace XMessenger.Database.Export.Models
{
    public class RegionModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public List<AreaModel> Areas { get; set; }
    }
}
