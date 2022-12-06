namespace XMessenger.Application.Sessions
{
    public class SessionModel
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }

        public List<TokenModel> Tokens { get; set; }
    }
}
