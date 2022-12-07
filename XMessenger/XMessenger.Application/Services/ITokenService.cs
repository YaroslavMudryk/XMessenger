using XMessenger.Application.Dtos.Identity;
using XMessenger.Domain.Models.Identity;

namespace XMessenger.Application.Services
{
    public interface ITokenService
    {
        Task<Token> GetUserTokenAsync(UserTokenDto userToken);
    }

    public class TokenService : ITokenService
    {
        public async Task<Token> GetUserTokenAsync(UserTokenDto userToken)
        {
            return await Task.FromResult(new Token
            {
                ExpiredAt = DateTime.Now.AddDays(1),
                SessionId = userToken.SessionId,
                JwtToken = Guid.NewGuid().ToString()
            });
        }
    }
}
