namespace XMessenger.Database.Models
{
    public class Metro : DatabaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public int SettlementId { get; set; }
        public Settlement Settlement { get; set; }
        public List<MetroLine> Lines { get; set; }
    }
}