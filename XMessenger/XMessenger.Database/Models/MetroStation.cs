namespace XMessenger.Database.Models
{
    public class MetroStation : DatabaseModel<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsTransfer { get; set; }
        public string TransferToCode { get; set; }
        public int MetroLineId { get; set; }
        public MetroLine MetroLine { get; set; }
    }
}
