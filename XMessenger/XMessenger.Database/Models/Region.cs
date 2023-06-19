namespace XMessenger.Database.Models
{
    public class Region : DatabaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<Area> Areas { get; set; }
    }
}
