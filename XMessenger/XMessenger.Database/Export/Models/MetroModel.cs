namespace XMessenger.Database.Export.Models
{
    public class MetroModel : ExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public List<MetroLineModel> Lines { get; set; }
    }
}
