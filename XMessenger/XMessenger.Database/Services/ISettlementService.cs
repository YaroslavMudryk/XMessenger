using Microsoft.EntityFrameworkCore;
using XMessenger.Database.Context;
using XMessenger.Database.Dtos;
using XMessenger.Database.ViewModels;
using XMessenger.Domain.Models.Database;
using XMessenger.Helpers;
using XMessenger.Helpers.Services;

namespace XMessenger.Database.Services
{
    public interface ISettlementService
    {
        Task<Result<List<SettlementViewModel>>> GetAllSettlementsAsync(int regionId, int page = 1);
        Task<Result<SettlementViewModel>> CreateSettlementAsync(SettlementDto model);
        Task<Result<SettlementViewModel>> EditSettlementAsync(SettlementDto model);
        Task<Result<SettlementViewModel>> RemoveSettlementAsync(int settlementId);
    }

    public class SettlementService : ISettlementService
    {
        private readonly DatabaseContext _db;
        private readonly IIdentityService _identityService;
        public SettlementService(DatabaseContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Result<SettlementViewModel>> CreateSettlementAsync(SettlementDto model)
        {
            if (!await _db.Regions.AnyAsync(s => s.Id == model.RegionId))
                return Result<SettlementViewModel>.NotFound();

            if (await _db.Settlements.AnyAsync(s => s.Name.ToLower() == model.Name.ToLower() && s.RegionId == model.RegionId && s.Type == model.Type))
                return Result<SettlementViewModel>.Error("Settlement already exist");

            var newSettlement = new Settlement
            {
                Name = model.Name,
                Flag = model.Flag,
                Type = model.Type,
                RegionId = model.RegionId
            };

            await _db.Settlements.AddAsync(newSettlement);
            await _db.SaveChangesAsync();
            return Result<SettlementViewModel>.SuccessWithData(new SettlementViewModel
            {
                Id = newSettlement.Id,
                Name = newSettlement.Name,
                Flag = newSettlement.Flag,
                Type = newSettlement.Type,
            });
        }

        public async Task<Result<SettlementViewModel>> EditSettlementAsync(SettlementDto model)
        {
            var settlementForEdit = await _db.Settlements.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.Id);

            if (settlementForEdit == null)
                return Result<SettlementViewModel>.NotFound();

            settlementForEdit.Name = model.Name;
            settlementForEdit.Flag = model.Flag;
            settlementForEdit.Type = model.Type;
            settlementForEdit.RegionId = model.RegionId;

            _db.Settlements.Update(settlementForEdit);
            await _db.SaveChangesAsync();
            return Result<SettlementViewModel>.SuccessWithData(new SettlementViewModel
            {
                Id = settlementForEdit.Id,
                Name = settlementForEdit.Name,
                Flag = settlementForEdit.Flag,
                Type = settlementForEdit.Type
            });
        }

        public async Task<Result<List<SettlementViewModel>>> GetAllSettlementsAsync(int regionId, int page = 1)
        {
            var settlements = await _db.Settlements.AsNoTracking().Where(s => s.RegionId == regionId).Skip(50 * (page - 1)).Take(50).OrderBy(s => s.Name).ToListAsync();
            var totalCount = await _db.Settlements.CountAsync(s => s.RegionId == regionId);

            return Result<List<SettlementViewModel>>.SuccessList(settlements.Select(s => new SettlementViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Flag = s.Flag,
                Type = s.Type,
            }).ToList(), Meta.FromMeta(totalCount, page));
        }

        public async Task<Result<SettlementViewModel>> RemoveSettlementAsync(int settlementId)
        {
            var settlementForRemove = await _db.Settlements.AsNoTracking().FirstOrDefaultAsync(s => s.Id == settlementId);

            if (settlementForRemove == null)
                return Result<SettlementViewModel>.NotFound();

            _db.Settlements.Remove(settlementForRemove);
            await _db.SaveChangesAsync();

            return Result<SettlementViewModel>.Success();
        }
    }
}
