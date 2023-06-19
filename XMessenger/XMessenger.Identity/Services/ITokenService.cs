namespace XMessenger.Identity.Services
{
    public interface ITokenService
    {
        Task<Token> GetUserTokenAsync(UserTokenDto userToken);
    }
}
