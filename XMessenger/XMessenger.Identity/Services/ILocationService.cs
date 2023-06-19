namespace XMessenger.Identity.Services
{
    public interface ILocationService
    {
        Task<LocationInfo> GetIpInfoAsync(string ip);
    }
}