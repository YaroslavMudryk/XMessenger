﻿using Microsoft.EntityFrameworkCore;
using XMessenger.Database.Context;
using XMessenger.Database.Dtos;
using XMessenger.Database.Services.Interfaces;
using XMessenger.Database.ViewModels;

namespace XMessenger.Database.Services.Implementations
{
    public class SettlementService : ISettlementService
    {
        private readonly DatabaseContext _db;
        public SettlementService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<SettlementViewModel>> CreateSettlementAsync(SettlementDto model)
        {
            if (!await _db.Areas.AnyAsync(s => s.Id == model.AreaId))
                return Result<SettlementViewModel>.NotFound();

            if (await _db.Settlements.AnyAsync(s => s.Name.ToLower() == model.Name.ToLower() && s.AreaId == model.AreaId && s.Type == model.Type))
                return Result<SettlementViewModel>.Error("Settlement already exist");

            var newSettlement = new Settlement
            {
                Name = model.Name,
                Flag = model.Flag,
                Type = model.Type,
                AreaId = model.AreaId
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
            settlementForEdit.AreaId = model.AreaId;

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

        public async Task<Result<List<SettlementSearchViewModel>>> SearchSettlementsAsync(string q, int page = 1)
        {
            var settlements = await _db.Settlements.AsNoTracking()
                .Where(s => s.Name == q || s.Name.Contains(q))
                .OrderBy(s => s.Name).ThenBy(s => s.Id)
                .Include(s => s.Area).ThenInclude(s => s.Region)
                .Skip(50 * (page - 1)).Take(50).OrderBy(s => s.Name)
                .ToListAsync();
            var totalCount = await _db.Settlements.CountAsync(s => s.Name.Contains(q));

            return Result<List<SettlementSearchViewModel>>.SuccessList(settlements.Select(s => new SettlementSearchViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Type = s.Type,
                Area = s.Area.Name,
                Region = s.Area.Region.Name
            }).OrderBy(s => s.Id).ThenBy(s => s.Name).ToList(), Meta.FromMeta(totalCount, page));
        }

        public async Task<Result<List<SettlementViewModel>>> GetAllSettlementsByAreaIdAsync(int areaId, int page = 1)
        {
            var settlements = await _db.Settlements.AsNoTracking().Where(s => s.AreaId == areaId).Skip(50 * (page - 1)).Take(50).OrderBy(s => s.Name).ToListAsync();
            var totalCount = await _db.Settlements.CountAsync(s => s.AreaId == areaId);

            return Result<List<SettlementViewModel>>.SuccessList(settlements.Select(s => new SettlementViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Flag = s.Flag,
                Type = s.Type,
            }).ToList(), Meta.FromMeta(totalCount, page));
        }

        public async Task<Result<List<SettlementViewModel>>> GetAllSettlementsByRegionIdAsync(int regionId, int page = 1)
        {
            var areaIds = await _db.Areas.AsNoTracking().Where(s => s.RegionId == regionId).Select(s => s.Id).ToListAsync();

            var settlements = await _db.Settlements.AsNoTracking().Where(s => areaIds.Contains(s.AreaId)).Skip(50 * (page - 1)).Take(50).OrderBy(s => s.Name).ToListAsync();
            var totalCount = await _db.Settlements.CountAsync(s => areaIds.Contains(s.AreaId));

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