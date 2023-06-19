namespace XMessenger.Database.Services.Implementations
{
    public class AreaService : IAreaService
    {
        private readonly DatabaseContext _db;
        public AreaService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<AreaViewModel>> CreateAsync(AreaDto area)
        {
            if (!await _db.Regions.AnyAsync(s => s.Id == area.RegionId))
                return Result<AreaViewModel>.NotFound();

            if (await _db.Areas.AnyAsync(s => s.Name.ToLower() == area.Name.ToLower() && s.RegionId == area.RegionId))
                return Result<AreaViewModel>.Error("Area already exist");

            var newArea = new Area
            {
                Name = area.Name,
                Flag = area.Flag,
                RegionId = area.RegionId
            };

            await _db.Areas.AddAsync(newArea);
            await _db.SaveChangesAsync();

            return Result<AreaViewModel>.SuccessWithData(new AreaViewModel
            {
                Id = newArea.Id,
                Name = area.Name,
                Flag = area.Flag
            });
        }

        public async Task<Result<AreaViewModel>> EditAsync(AreaDto area)
        {
            var areaForEdit = await _db.Areas.AsNoTracking().FirstOrDefaultAsync(s => s.Id == area.Id);

            if (areaForEdit == null)
                return Result<AreaViewModel>.NotFound();

            areaForEdit.Name = area.Name;
            areaForEdit.Flag = area.Flag;
            areaForEdit.RegionId = area.RegionId;

            _db.Areas.Update(areaForEdit);
            await _db.SaveChangesAsync();
            return Result<AreaViewModel>.SuccessWithData(new AreaViewModel
            {
                Id = areaForEdit.Id,
                Name = areaForEdit.Name,
                Flag = areaForEdit.Flag
            });
        }

        public async Task<Result<List<AreaViewModel>>> GetAllAreasAsync(int regionId)
        {
            var regions = await _db.Areas.AsNoTracking().Where(s => s.RegionId == regionId).OrderBy(s => s.Name).ToListAsync();
            return Result<List<AreaViewModel>>.SuccessList(regions.Select(s => new AreaViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Flag = s.Flag
            }).ToList(), Meta.FromMeta(regions.Count, 1));
        }

        public async Task<Result<AreaViewModel>> RemoveAsync(int id)
        {
            var areaForRemove = await _db.Areas.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (areaForRemove == null)
                return Result<AreaViewModel>.NotFound();

            _db.Areas.Remove(areaForRemove);
            await _db.SaveChangesAsync();
            return Result<AreaViewModel>.Success();
        }
    }
}