namespace XMessenger.Database.ViewModels
{
    public class SettlementSearchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SettlementType Type { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
    }
}