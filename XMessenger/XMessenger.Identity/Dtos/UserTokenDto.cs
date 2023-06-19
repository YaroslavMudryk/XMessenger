namespace XMessenger.Identity.Dtos
{
    public class UserTokenDto
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
        public string AuthType { get; set; }
        public string Lang { get; set; }
    }
}