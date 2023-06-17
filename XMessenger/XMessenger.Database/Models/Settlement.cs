﻿namespace XMessenger.Database.Models
{
    public class Settlement : BaseModel<int>
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public SettlementType Type { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public List<University > Universities { get; set; }
        public List<Metro> Metro { get; set; }

    }
    public enum SettlementType
    {
        City = 1,
        Village,
        UrbanVillage
    }
}