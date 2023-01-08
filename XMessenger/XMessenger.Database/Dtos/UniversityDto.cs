namespace XMessenger.Database.Dtos
{
    public class UniversityDto
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Emblem { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string CodeEDBO { get; set; }
        public int SettlementId { get; set; }
    }
}