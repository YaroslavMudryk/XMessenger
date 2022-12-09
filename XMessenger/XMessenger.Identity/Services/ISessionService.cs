using XMessenger.Helpers;
using XMessenger.Identity.ViewModels;

namespace XMessenger.Identity.Services
{
    public interface ISessionService
    {
        Task<Result<List<SessionViewModel>>> GetUserSessionsAsync(int q, int page);
        Task<Result<int>> CloseSessionsByIdsAsync(Guid[] ids);
    }
}
