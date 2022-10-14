namespace XMessenger.Domain.Models.Identity;

public class Confirm : BaseModel<Guid>
{
    public string Code { get; set; }
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}