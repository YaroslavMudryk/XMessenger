using Extensions.DeviceDetector.Models;
using System.ComponentModel.DataAnnotations;

namespace XMessenger.Domain.Models.Identity
{
    public class Session : BaseModel<Guid>
    {
        [Required]
        public App App { get; set; }
        [Required]
        public Location Location { get; set; }
        [Required]
        public ClientInfo Client { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public Guid? DeactivatedBySessionId { get; set; }
        [Required, StringLength(50, MinimumLength = 40)]
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Token> Tokens { get; set; }
    }
}