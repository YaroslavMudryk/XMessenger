namespace XMessenger.Database.Models
{
    public class Settlement : DatabaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public SettlementType Type { get; set; }
        public List<string> OldNames { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public List<University> Universities { get; set; } = new List<University>();
        public List<Metro> Metro { get; set; } = new List<Metro>();

    }
    public enum SettlementType
    {
        City = 1,
        UrbanVillage,
        Village,
    }
}
