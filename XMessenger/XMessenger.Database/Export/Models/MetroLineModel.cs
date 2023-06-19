namespace XMessenger.Database.Export.Models
{
    public class MetroLineModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<MetroStationModel> Stations { get; set; }
    }
}
