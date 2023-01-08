using XMessenger.Database.ViewModels;
using XMessenger.Domain.Models.Database;

namespace XMessenger.Database.Dtos
{
    public class SettlementDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public int RegionId { get; set; }
        public SettlementType Type { get; set; }
    }
}