namespace XMessenger.Database.Models
{
    public class Area : DatabaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<Settlement> Settlements { get; set; }
    }
}
