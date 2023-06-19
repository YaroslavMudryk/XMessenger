namespace XMessenger.Database.Export.Models
{
    public class SettlementModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public SettlementType Type { get; set; }
        public List<string> OldNames { get; set; }
        public List<UniversityModel> Universities { get; set; } = new List<UniversityModel>();
        public List<MetroModel> Metro { get; set; } = new List<MetroModel>();
    }
}
