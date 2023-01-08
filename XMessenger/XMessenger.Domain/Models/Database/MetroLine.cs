namespace XMessenger.Domain.Models.Database
{
    public class MetroLine : BaseModel<int>
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int MetroId { get; set; }
        public Metro Metro { get; set; }
        public List<MetroStation> Stations { get; set; }
    }
}
