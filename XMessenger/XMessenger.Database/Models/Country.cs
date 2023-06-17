namespace XMessenger.Database.Models
{
    public class Country : BaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Populations { get; set; }
        public List<Region> Regions { get; set; }
    }
}
