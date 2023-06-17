namespace XMessenger.Database.Models
{
    public class University : BaseModel<int>
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Emblem { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string CodeEDBO { get; set; }
        public int SettlementId { get; set; }
        public Settlement Settlement { get; set; }

        public List<Faculty> Faculties { get; set; }
    }
}
