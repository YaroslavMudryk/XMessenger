namespace XMessenger.Database.Services.Interfaces
{
    public interface ISettlementService
    {
        Task<Result<List<SettlementSearchViewModel>>> SearchSettlementsAsync(string q, int page = 1);
        Task<Result<List<SettlementViewModel>>> GetAllSettlementsByAreaIdAsync(int areaId, int page = 1);
        Task<Result<List<SettlementViewModel>>> GetAllSettlementsByRegionIdAsync(int regionId, int page = 1);
        Task<Result<SettlementViewModel>> GetSettlementByIdAsync(int id);
        Task<Result<SettlementViewModel>> CreateSettlementAsync(SettlementDto model);
        Task<Result<SettlementViewModel>> EditSettlementAsync(SettlementDto model);
        Task<Result<SettlementViewModel>> RemoveSettlementAsync(int settlementId);
        Task<Result<object>> FindObjectByItemIdAsync(string itemId);
    }
}