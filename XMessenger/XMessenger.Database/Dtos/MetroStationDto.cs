namespace XMessenger.Database.Dtos
{
    public class MetroStationDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsTransfer { get; set; }
        public int MetroLineId { get; set; }
    }
}