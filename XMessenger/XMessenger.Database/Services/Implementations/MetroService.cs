namespace XMessenger.Database.Services.Implementations
{
    public class MetroService : IMetroService
    {
        private readonly DatabaseContext _db;
        public MetroService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<MetroViewModel>> CreateMetroAsync(MetroDto metro)
        {
            if (!await _db.Settlements.AnyAsync(s => s.Id == metro.SettlementId))
                return Result<MetroViewModel>.NotFound();

            if (await _db.Metro.AnyAsync(s => s.Name.ToLower() == metro.Name.ToLower() && s.SettlementId == metro.SettlementId))
                return Result<MetroViewModel>.Error("Metro already exist");

            var newMetro = new Metro
            {
                Description = metro.Description,
                Flag = metro.Flag,
                Name = metro.Name,
                SettlementId = metro.SettlementId,
            };

            await _db.Metro.AddAsync(newMetro);
            await _db.SaveChangesAsync();
            return Result<MetroViewModel>.SuccessWithData(new MetroViewModel
            {
                Id = newMetro.Id,
                Name = metro.Name,
                Flag = metro.Flag,
                Description = metro.Description,
            });
        }

        public async Task<Result<MetroLineViewModel>> CreateMetroLineAsync(MetroLineDto metroLine)
        {
            if (!await _db.Metro.AnyAsync(s => s.Id == metroLine.MetroId))
                return Result<MetroLineViewModel>.NotFound();

            if (await _db.MetroLines.AnyAsync(s => s.Name.ToLower() == metroLine.Name.ToLower() && s.MetroId == metroLine.MetroId))
                return Result<MetroLineViewModel>.Error("Metro line already exist");

            var newMetroLine = new MetroLine
            {
                Name = metroLine.Name,
                Color = metroLine.Color,
                MetroId = metroLine.MetroId,
            };

            await _db.MetroLines.AddAsync(newMetroLine);
            await _db.SaveChangesAsync();
            return Result<MetroLineViewModel>.SuccessWithData(new MetroLineViewModel
            {
                Id = newMetroLine.Id,
                Name = metroLine.Name,
                Color = metroLine.Color
            });
        }

        public async Task<Result<MetroStationViewModel>> CreateMetroStationAsync(MetroStationDto metroStation)
        {
            if (!await _db.MetroLines.AnyAsync(s => s.Id == metroStation.MetroLineId))
                return Result<MetroStationViewModel>.NotFound();

            if (await _db.MetroStations.AnyAsync(s => s.Name.ToLower() == metroStation.Name.ToLower() && s.MetroLineId == metroStation.MetroLineId))
                return Result<MetroStationViewModel>.Error("Metro stations already exist");

            var newMetroStations = new MetroStation
            {
                Name = metroStation.Name,
                Code = metroStation.Code,
                IsTransfer = metroStation.IsTransfer,
            };

            await _db.MetroStations.AddAsync(newMetroStations);
            await _db.SaveChangesAsync();
            return Result<MetroStationViewModel>.SuccessWithData(new MetroStationViewModel
            {
                Id = newMetroStations.Id,
                Name = metroStation.Name,
                Code = metroStation.Code,
                IsTransfer = metroStation.IsTransfer,
            });
        }

        public async Task<Result<MetroViewModel>> EditMetroAsync(MetroDto metro)
        {
            var metroToUpdate = await _db.Metro.AsNoTracking().FirstOrDefaultAsync(s => s.Id == metro.Id);

            if (metroToUpdate == null)
                return Result<MetroViewModel>.NotFound();

            metroToUpdate.Name = metro.Name;
            metroToUpdate.Description = metro.Description;
            metroToUpdate.Flag = metro.Flag;
            metroToUpdate.SettlementId = metro.SettlementId;


            _db.Metro.Update(metroToUpdate);
            await _db.SaveChangesAsync();
            return Result<MetroViewModel>.SuccessWithData(new MetroViewModel
            {
                Id = metroToUpdate.Id,
                Name = metroToUpdate.Name,
                Description = metroToUpdate.Description,
                Flag = metroToUpdate.Flag,
            });
        }

        public async Task<Result<MetroLineViewModel>> EditMetroLineAsync(MetroLineDto metroLine)
        {
            var metroLineToUpdate = await _db.MetroLines.AsNoTracking().FirstOrDefaultAsync(s => s.Id == metroLine.Id);

            if (metroLineToUpdate == null)
                return Result<MetroLineViewModel>.NotFound();

            metroLineToUpdate.Name = metroLine.Name;
            metroLineToUpdate.Color = metroLine.Color;
            metroLineToUpdate.MetroId = metroLine.MetroId;


            _db.MetroLines.Update(metroLineToUpdate);
            await _db.SaveChangesAsync();
            return Result<MetroLineViewModel>.SuccessWithData(new MetroLineViewModel
            {
                Id = metroLineToUpdate.Id,
                Name = metroLineToUpdate.Name,
                Color = metroLineToUpdate.Color
            });
        }

        public async Task<Result<MetroStationViewModel>> EditMetroStationAsync(MetroStationDto metroStation)
        {
            var metroStationToUpdate = await _db.MetroStations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == metroStation.Id);

            if (metroStationToUpdate == null)
                return Result<MetroStationViewModel>.NotFound();

            metroStationToUpdate.Name = metroStation.Name;
            metroStationToUpdate.Code = metroStation.Code;
            metroStationToUpdate.IsTransfer = metroStation.IsTransfer;
            metroStationToUpdate.MetroLineId = metroStation.MetroLineId;


            _db.MetroStations.Update(metroStationToUpdate);
            await _db.SaveChangesAsync();
            return Result<MetroStationViewModel>.SuccessWithData(new MetroStationViewModel
            {
                Id = metroStationToUpdate.Id,
                Name = metroStationToUpdate.Name,
                Code = metroStationToUpdate.Code
            });
        }

        public async Task<Result<List<MetroViewModel>>> GetAllMetroAsync(int settlementId)
        {
            var metro = await _db.Metro.AsNoTracking().Where(s => s.SettlementId == settlementId).OrderBy(s => s.Name).ToListAsync();

            return Result<List<MetroViewModel>>.SuccessList(metro.Select(s => new MetroViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Flag = s.Flag,
            }).ToList(), Meta.FromMeta(metro.Count, 1));
        }

        public async Task<Result<List<MetroLineViewModel>>> GetAllMetroLinesAsync(int metroId)
        {
            var metroLines = await _db.MetroLines.AsNoTracking().Where(s => s.MetroId == metroId).OrderBy(s => s.Name).ToListAsync();

            return Result<List<MetroLineViewModel>>.SuccessList(metroLines.Select(s => new MetroLineViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
            }).ToList(), Meta.FromMeta(metroLines.Count, 1));
        }

        public async Task<Result<List<MetroStationViewModel>>> GetAllMetroStationsAsync(int metroLineId)
        {
            var metroStations = await _db.MetroStations.AsNoTracking().Where(s => s.MetroLineId == metroLineId).OrderBy(s => s.Name).ToListAsync();

            return Result<List<MetroStationViewModel>>.SuccessList(metroStations.Select(s => new MetroStationViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                IsTransfer = s.IsTransfer
            }).OrderBy(s => s.Code).ToList(), Meta.FromMeta(metroStations.Count, 1));
        }

        public async Task<Result<MetroViewModel>> GetMetroAsync(int metroId)
        {
            var metro = await _db.Metro
                .AsNoTracking()
                .Include(s => s.Lines)
                .ThenInclude(s => s.Stations)
                .FirstOrDefaultAsync(s => s.Id == metroId);
            if (metro == null)
                return Result<MetroViewModel>.NotFound();

            return Result<MetroViewModel>.SuccessWithData(new MetroViewModel
            {
                Id = metro.Id,
                Name = metro.Name,
                Description = metro.Description,
                Flag = metro.Flag,
                Lines = metro.Lines.Select(s => new MetroLineViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Color = s.Color,
                    Stations = s.Stations.Select(x => new MetroStationViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        IsTransfer = x.IsTransfer,
                    }).ToList()
                }).ToList()
            });
        }

        public async Task<Result<MetroViewModel>> RemoveMetroAsync(int id)
        {
            var metroToRemove = await _db.Metro.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (metroToRemove == null)
                return Result<MetroViewModel>.NotFound();

            _db.Metro.Remove(metroToRemove);
            await _db.SaveChangesAsync();
            return Result<MetroViewModel>.Success();
        }

        public async Task<Result<MetroLineViewModel>> RemoveMetroLineAsync(int id)
        {
            var metroLineToRemove = await _db.MetroLines.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (metroLineToRemove == null)
                return Result<MetroLineViewModel>.NotFound();

            _db.MetroLines.Remove(metroLineToRemove);
            await _db.SaveChangesAsync();
            return Result<MetroLineViewModel>.Success();
        }

        public async Task<Result<MetroStationViewModel>> RemoveMetroStationAsync(int id)
        {
            var metroStationToRemove = await _db.MetroStations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (metroStationToRemove == null)
                return Result<MetroStationViewModel>.NotFound();

            _db.MetroStations.Remove(metroStationToRemove);
            await _db.SaveChangesAsync();
            return Result<MetroStationViewModel>.Success();
        }
    }
}
