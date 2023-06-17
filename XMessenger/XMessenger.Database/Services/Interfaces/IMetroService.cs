using XMessenger.Database.Dtos;
using XMessenger.Database.ViewModels;
namespace XMessenger.Database.Services.Interfaces
{
    public interface IMetroService
    {
        Task<Result<MetroViewModel>> GetMetroAsync(int metroId);
        Task<Result<List<MetroViewModel>>> GetAllMetroAsync(int settlementId);
        Task<Result<List<MetroLineViewModel>>> GetAllMetroLinesAsync(int metroId);
        Task<Result<List<MetroStationViewModel>>> GetAllMetroStationsAsync(int metroLineId);

        Task<Result<MetroViewModel>> CreateMetroAsync(MetroDto metro);
        Task<Result<MetroViewModel>> EditMetroAsync(MetroDto metro);
        Task<Result<MetroViewModel>> RemoveMetroAsync(int id);

        Task<Result<MetroLineViewModel>> CreateMetroLineAsync(MetroLineDto metro);
        Task<Result<MetroLineViewModel>> EditMetroLineAsync(MetroLineDto metro);
        Task<Result<MetroLineViewModel>> RemoveMetroLineAsync(int id);

        Task<Result<MetroStationViewModel>> CreateMetroStationAsync(MetroStationDto metro);
        Task<Result<MetroStationViewModel>> EditMetroStationAsync(MetroStationDto metro);
        Task<Result<MetroStationViewModel>> RemoveMetroStationAsync(int id);
    }
}