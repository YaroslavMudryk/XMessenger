using Extensions.DeviceDetector.Models;
using System.ComponentModel.DataAnnotations;

namespace XMessenger.Domain.Models.Identity
{
    public class Session : BaseSoftDeletableModel<Guid>
    {
        [Required]
        public AppInfo App { get; set; }
        [Required]
        public LocationInfo Location { get; set; }
        [Required]
        public ClientInfo Client { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string Type { get; set; }
        public bool ViaMFA { get; set; }
        public SessionStatus Status { get; set; }
        public string Language { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public Guid? DeactivatedBySessionId { get; set; }
        [Required, StringLength(50, MinimumLength = 40)]
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Token> Tokens { get; set; }
    }

    public class AppInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Image { get; set; }
    }

    public class LocationInfo
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Provider { get; set; }
        public string IP { get; set; }
    }

    public enum SessionStatus
    {
        New,
        Active,
        Close
    }
}