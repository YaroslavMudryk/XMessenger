namespace XMessenger.Domain.Models.Database
{
    public class Region : BaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<Settlement> Settlements { get; set; }
    }
}
