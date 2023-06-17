namespace XMessenger.Database.ViewModels
{
    public class AreaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public RegionViewModel Region { get; set; }
        public List<Settlement> Settlements { get; set; }
    }
}