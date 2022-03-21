using Extensions.DeviceDetector.Models;
namespace XMessenger.Domain.Models.Identity
{
    public class Session : BaseModel<Guid>
    {
        public App App { get; set; }
        public Location Location { get; set; }
        public ClientInfo Client { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public Guid? DeactivatedBySessionId { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}