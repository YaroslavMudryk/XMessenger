using XMessenger.Domain.Models.Identity;

namespace XMessenger.Identity.Services
{
    public interface ILocationService
    {
        Task<LocationInfo> GetLocationAsync(string ip);
    }
}