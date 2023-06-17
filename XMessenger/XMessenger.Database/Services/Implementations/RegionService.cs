using Microsoft.EntityFrameworkCore;
using XMessenger.Database.Context;
using XMessenger.Database.Dtos;
using XMessenger.Database.Services.Interfaces;
using XMessenger.Database.ViewModels;

namespace XMessenger.Database.Services.Implementations
{
    public class RegionService : IRegionService
    {
        private readonly DatabaseContext _db;
        public RegionService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<RegionViewModel>> CreateAsync(RegionDto region)
        {
            if (!await _db.Countries.AnyAsync(s => s.Id == region.CountryId))
                return Result<RegionViewModel>.NotFound();

            if (await _db.Regions.AnyAsync(s => s.Name.ToLower() == region.Name.ToLower() && s.CountryId == region.CountryId))
                return Result<RegionViewModel>.Error("Region already exist");

            var newRegion = new Region
            {
                Name = region.Name,
                Flag = region.Flag,
                CountryId = region.CountryId,
            };

            await _db.Regions.AddAsync(newRegion);
            await _db.SaveChangesAsync();

            return Result<RegionViewModel>.SuccessWithData(new RegionViewModel
            {
                Id = newRegion.Id,
                Name = region.Name,
                Flag = region.Flag
            });
        }

        public async Task<Result<RegionViewModel>> EditAsync(RegionDto region)
        {
            var regionForEdit = await _db.Regions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == region.Id);

            if (regionForEdit == null)
                return Result<RegionViewModel>.NotFound();

            regionForEdit.Name = region.Name;
            regionForEdit.Flag = region.Flag;
            regionForEdit.CountryId = region.CountryId;

            _db.Regions.Update(regionForEdit);
            await _db.SaveChangesAsync();
            return Result<RegionViewModel>.SuccessWithData(new RegionViewModel
            {
                Id = regionForEdit.Id,
                Name = regionForEdit.Name,
                Flag = regionForEdit.Flag
            });
        }

        public async Task<Result<List<RegionViewModel>>> GetAllRegionsAsync(int countryId)
        {
            var regions = await _db.Regions.AsNoTracking().Where(s => s.CountryId == countryId).OrderBy(s => s.Name).ToListAsync();
            return Result<List<RegionViewModel>>.SuccessList(regions.Select(s => new RegionViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Flag = s.Flag
            }).ToList(), Meta.FromMeta(regions.Count, 1));
        }

        public async Task<Result<RegionViewModel>> RemoveAsync(int id)
        {
            var regionForRemove = await _db.Regions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (regionForRemove == null)
                return Result<RegionViewModel>.NotFound();

            _db.Regions.Remove(regionForRemove);
            await _db.SaveChangesAsync();
            return Result<RegionViewModel>.Success();
        }
    }
}
