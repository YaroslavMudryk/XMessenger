using Extensions.DeviceDetector.Models;

namespace XMessenger.Identity.Models;

public class LoginAttempt : BaseSoftDeletableModel<int>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public ClientInfo Device { get; set; }
    public LocationInfo Location { get; set; }
    public bool IsSuccess { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
}