namespace XMessenger.Application.Dtos.Identity
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string SessionId { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}