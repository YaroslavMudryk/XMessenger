
using Extensions.DeviceDetector.Models;

namespace XMessenger.Domain.Models.Identity;

public class LoginAttempt: BaseModel<int>
{
    public string Password { get; set; }
    public ClientInfo Device { get; set; }
    public Location Location { get; set; }
    public bool IsSuccess { get; set; }
    public int? UserId { get; set; }
}