namespace XMessenger.Database.ViewModels
{
    public class RegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public CountryViewModel Country { get; set; }
        public List<SettlementViewModel> Settlements { get; set; }
    }
}