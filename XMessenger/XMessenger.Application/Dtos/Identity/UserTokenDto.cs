using XMessenger.Domain.Models.Identity;

namespace XMessenger.Application.Dtos.Identity
{
    public class UserTokenDto
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid SessionId { get; set; }
        public string AuthType { get; set; }
        public string Lang { get; set; }
    }
}
