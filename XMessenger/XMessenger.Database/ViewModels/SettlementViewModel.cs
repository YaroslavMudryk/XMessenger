using XMessenger.Domain.Models.Database;

namespace XMessenger.Database.ViewModels
{
    public class SettlementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public RegionViewModel Region { get; set; }
        public SettlementType Type { get; set; }
    }
}