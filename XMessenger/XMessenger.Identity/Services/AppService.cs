using Mapster;
using Microsoft.EntityFrameworkCore;
using XMessenger.Identity.Models;
using XMessenger.Helpers;
using XMessenger.Helpers.Services;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Dtos;
using XMessenger.Identity.ViewModels;

namespace XMessenger.Identity.Services
{
    public class AppService : IAppService
    {
        private readonly IdentityContext _db;
        private readonly IIdentityService _identityService;
        public AppService(IdentityContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Result<AppSecretViewModel>> ChangeAppSecretAsync(int id)
        {
            var userId = _identityService.GetUserId();

            var appToChangeSecret = await _db.Apps.FindAsync(id);

            if (appToChangeSecret == null)
                return Result<AppSecretViewModel>.NotFound("App not found");

            if (appToChangeSecret.IsDeleted)
                if (!_identityService.IsAdmin())
                    return Result<AppSecretViewModel>.Error("App deleted");

            if (!_identityService.IsAdmin())
                if (appToChangeSecret.CreatedBy != userId)
                    return Result<AppSecretViewModel>.Forbiden();

            appToChangeSecret.ClientSecret = Generator.GetAppSecret();

            _db.Apps.Update(appToChangeSecret);

            await _db.SaveChangesAsync();

            return Result<AppSecretViewModel>.SuccessWithData(new AppSecretViewModel { ClientId = appToChangeSecret.ClientId, ClientSecret = appToChangeSecret.ClientSecret });
        }

        public async Task<Result<AppViewModel>> CreateAppAsync(AppDto appDto)
        {
            var userId = _identityService.GetUserId();
            var isAdmin = _identityService.IsAdmin();

            if (!isAdmin)
                if (await _db.Apps.AsNoTracking().CountAsync(s => s.CreatedBy == userId) >= 10)
                    return Result<AppViewModel>.Error("One user can create only 10 apps");

            if (!await IsAllClaimsExistAsync(appDto.ClaimIds))
                return Result<AppViewModel>.Error("Not all claims exist");

            var newApp = appDto.Adapt<App>();

            newApp.ClientId = Generator.GetAppId();
            newApp.ClientSecret = Generator.GetAppSecret();

            await _db.Apps.AddAsync(newApp);
            await _db.SaveChangesAsync();

            var createdApp = newApp.Adapt<AppViewModel>();

            var claimIds = appDto.ClaimIds.Distinct();

            var claimsForApp = await _db.Claims.AsNoTracking().Where(s => claimIds.Contains(s.Id)).ToListAsync();

            var newAppRoles = claimsForApp.Select(s => new AppClaim { ClaimId = s.Id, AppId = createdApp.Id });

            await _db.AppClaims.AddRangeAsync(newAppRoles);

            await _db.SaveChangesAsync();

            return Result<AppViewModel>.Created(createdApp);
        }

        public async Task<Result<bool>> DeleteAppAsync(int id)
        {
            var userId = _identityService.GetUserId();

            var appToRemove = await _db.Apps.FindAsync(id);

            if (appToRemove == null)
                return Result<bool>.NotFound("App not found");

            if (!_identityService.IsAdmin())
                if (appToRemove.CreatedBy != userId)
                    return Result<bool>.Forbiden();

            _db.Apps.Remove(appToRemove);
            await _db.SaveChangesAsync();

            return Result<bool>.SuccessWithData(true);
        }

        public async Task<Result<List<AppViewModel>>> GetAllAppsAsync(int page)
        {
            var apps = await _db.Apps.AsNoTracking().Skip((page - 1) * Paginations.PerPage).Take(Paginations.PerPage).ToListAsync();

            var appsToView = apps.Adapt<List<AppViewModel>>();

            var totalApps = await _db.Apps.CountAsync();

            return Result<List<AppViewModel>>.SuccessList(appsToView, Meta.FromMeta(totalApps, page));
        }

        public async Task<Result<AppViewModel>> GetAppByIdAsync(int id)
        {
            var userId = _identityService.GetUserId();

            var app = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (app == null)
                return Result<AppViewModel>.NotFound("App not found");

            if (app.IsDeleted)
                if (!_identityService.IsAdmin())
                    return Result<AppViewModel>.Error("App deleted");

            if (!_identityService.IsAdmin())
                if (app.CreatedBy != userId)
                    return Result<AppViewModel>.Forbiden();

            return Result<AppViewModel>.SuccessWithData(app.Adapt<AppViewModel>());
        }

        public async Task<Result<AppSecretViewModel>> GetAppSecretAsync(int id)
        {
            var userId = _identityService.GetUserId();

            var app = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (app == null)
                return Result<AppSecretViewModel>.NotFound("App not found");

            if (app.IsDeleted)
                if (!_identityService.IsAdmin())
                    return Result<AppSecretViewModel>.Error("App deleted");

            if (!_identityService.IsAdmin())
                if (app.CreatedBy != userId)
                    return Result<AppSecretViewModel>.Forbiden();

            return Result<AppSecretViewModel>.SuccessWithData(new AppSecretViewModel { ClientId = app.ClientId, ClientSecret = app.ClientSecret });
        }

        public async Task<Result<List<AppViewModel>>> GetMyAppsAsync()
        {
            var userId = _identityService.GetUserId();
            var isAdmin = _identityService.IsAdmin();

            var query = (IQueryable<App>)_db.Apps;

            if (!isAdmin)
                query = query.Where(s => !s.IsDeleted);

            var apps = await query.OrderByDescending(s => s.CreatedAt).ToListAsync();


            return Result<List<AppViewModel>>.SuccessWithData(apps.Adapt<List<AppViewModel>>());
        }

        public async Task<Result<AppViewModel>> UpdateAppAsync(AppDto appDto)
        {
            var userId = _identityService.GetUserId();

            var appToUpdate = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.Id == appDto.Id);

            if (appToUpdate == null)
                return Result<AppViewModel>.NotFound("App not found");

            if (appToUpdate.IsDeleted)
                if (!_identityService.IsAdmin())
                    return Result<AppViewModel>.Error("App deleted");

            if (!_identityService.IsAdmin())
                if (appToUpdate.CreatedBy != userId)
                    return Result<AppViewModel>.Forbiden();

            appToUpdate.Name = appDto.Name;
            appToUpdate.ShortName = appDto.ShortName;
            appToUpdate.Description = appDto.Description;
            appToUpdate.ActiveFrom = appDto.ActiveFrom;
            appToUpdate.ActiveTo = appDto.ActiveTo;
            appToUpdate.IsActive = appDto.IsActive;

            _db.Apps.Update(appToUpdate);
            await _db.SaveChangesAsync();

            return Result<AppViewModel>.SuccessWithData(appToUpdate.Adapt<AppViewModel>());
        }

        public async Task<Result<bool>> UpdateAppClaimsAsync(AppClaimsDto appClaims)
        {
            var userId = _identityService.GetUserId();

            var appId = appClaims.Id;
            var appClaimIds = appClaims.ClaimIds;

            var appToUpdate = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.Id == appId);

            if (appToUpdate == null)
                return Result<bool>.NotFound("App not found");

            if (appToUpdate.IsDeleted)
                if (!_identityService.IsAdmin())
                    return Result<bool>.Error("App deleted");

            if (!_identityService.IsAdmin())
                if (appToUpdate.CreatedBy != userId)
                    return Result<bool>.Forbiden();

            if (!await IsAllClaimsExistAsync(appClaimIds))
                return Result<bool>.Error("Not all claims exist");


            var appClaimsFromApp = await _db.AppClaims.AsNoTracking().Where(s => s.AppId == appId).ToListAsync();

            var claimIdsFromDb = appClaimsFromApp.Select(s => s.ClaimId);

            var appClaimsToDelete = appClaimsFromApp.Where(s => !appClaimIds.Contains(s.ClaimId));

            var claimsToCreate = appClaimIds.Where(s => !claimIdsFromDb.Contains(s));

            var appClaimsToCreate = claimsToCreate.Select(s => new AppClaim { AppId = appId, ClaimId = s });

            _db.AppClaims.RemoveRange(appClaimsToDelete);
            await _db.AppClaims.AddRangeAsync(appClaimsToCreate);
            await _db.SaveChangesAsync();

            return Result<bool>.SuccessWithData(true);
        }

        private async Task<bool> IsAllClaimsExistAsync(int[] claimsIds)
        {
            var claims = await _db.Claims.AsNoTracking().Where(s => claimsIds.Contains(s.Id)).ToListAsync();
            return claims.Count == claimsIds.Count();
        }
    }
}