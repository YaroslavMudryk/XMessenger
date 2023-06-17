using XMessenger.Identity.Models;
using XMessenger.Identity.Dtos;

namespace XMessenger.Identity.Services
{
    public interface ITokenService
    {
        Task<Token> GetUserTokenAsync(UserTokenDto userToken);
    }
}
