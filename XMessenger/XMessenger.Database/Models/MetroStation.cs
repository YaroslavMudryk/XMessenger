namespace XMessenger.Database.Models
{
    public class MetroStation : BaseModel<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsTransfer { get; set; }
        public int MetroLineId { get; set; }
        public MetroLine MetroLine { get; set; }
    }
}
