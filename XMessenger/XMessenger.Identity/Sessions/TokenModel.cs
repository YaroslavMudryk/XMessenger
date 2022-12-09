namespace XMessenger.Identity.Sessions
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
