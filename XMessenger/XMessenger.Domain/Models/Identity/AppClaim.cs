using System.Security.Claims;

namespace XMessenger.Domain.Models.Identity;

public class AppClaim : BaseModel<int>
{
    public int ClaimId { get; set; }
    public Claim Claim { get; set; }

    public int AppId { get; set; }
    public App App { get; set; }
}