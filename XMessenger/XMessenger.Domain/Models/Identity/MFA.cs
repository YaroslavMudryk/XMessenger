namespace XMessenger.Domain.Models.Identity
{
    public class MFA : BaseModel<int>
    {
        public DateTime? Activated { get; set; }
        public Guid? ActivatedBySessionId { get; set; }
        public bool IsActivated { get; set; }
        public string Secret { get; set; }
        public string EntryCode { get; set; }
        public string QrCodeBase64 { get; set; }
        public DateTime? Diactived { get; set; }
        public Guid? DiactivedBySessionId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
