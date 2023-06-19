namespace XMessenger.Database.Export.Models
{
    public class MetroStationModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsTransfer { get; set; }
    }
}
