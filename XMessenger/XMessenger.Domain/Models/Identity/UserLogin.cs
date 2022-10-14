namespace XMessenger.Domain.Models.Identity;

public class UserLogin : BaseModel<int>
{
    public string Key { get; set; }
    public string Provider { get; set; }
    public bool IsActive { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}