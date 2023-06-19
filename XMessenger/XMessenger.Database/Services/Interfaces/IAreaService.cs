namespace XMessenger.Database.Services.Interfaces
{
    public interface IAreaService
    {
        Task<Result<List<AreaViewModel>>> GetAllAreasAsync(int regionId);
        Task<Result<AreaViewModel>> CreateAsync(AreaDto region);
        Task<Result<AreaViewModel>> EditAsync(AreaDto region);
        Task<Result<AreaViewModel>> RemoveAsync(int id);
    }
}