namespace XMessenger.Database.Services.Interfaces
{
    public interface IRegionService
    {
        Task<Result<List<RegionViewModel>>> GetAllRegionsAsync(int countryId);
        Task<Result<RegionViewModel>> CreateAsync(RegionDto country);
        Task<Result<RegionViewModel>> EditAsync(RegionDto country);
        Task<Result<RegionViewModel>> RemoveAsync(int id);
    }
}