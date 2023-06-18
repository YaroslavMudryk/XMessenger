namespace XMessenger.Database.ViewModels
{
    public class SettlementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public SettlementType Type { get; set; }
        public AreaViewModel Area { get; set; }
    }
}