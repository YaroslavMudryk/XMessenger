using XMessenger.Database.ViewModels;

namespace XMessenger.Database.Dtos
{
    public class SettlementDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public int AreaId { get; set; }
        public SettlementType Type { get; set; }
    }
}