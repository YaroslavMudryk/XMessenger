namespace XMessenger.Database.Export.Models
{
    public class AreaModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public List<SettlementModel> Settlements { get; set; }
    }
}
