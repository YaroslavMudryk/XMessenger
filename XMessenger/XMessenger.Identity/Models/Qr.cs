namespace XMessenger.Identity.Models
{
    public class Qr : BaseModel<Guid>
    {
        public string QrCode { get; set; }
        public bool IsTaken { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
