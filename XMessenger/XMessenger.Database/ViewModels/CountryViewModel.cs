namespace XMessenger.Database.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Populations { get; set; }
        public List<Region> Regions { get; set; }
    }
}