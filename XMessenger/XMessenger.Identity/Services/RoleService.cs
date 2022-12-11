using Mapster;
using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers;
using XMessenger.Helpers.Services;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Dtos;
using XMessenger.Identity.ViewModels;

namespace XMessenger.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly IIdentityService _identityService;
        private readonly IdentityContext _db;
        public RoleService(IdentityContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Result<RoleViewModel>> CreateRoleAsync(RoleDto role)
        {
            var userId = _identityService.GetUserId();

            if (await _db.Roles.AsNoTracking().AnyAsync(s => s.Name == role.Name && s.NameNormalized == role.Name.ToUpper()))
                return Result<RoleViewModel>.Error("The same role already exist");

            if (!await IsAllClaimsExistAsync(role.ClaimIds))
                return Result<RoleViewModel>.Error("Not all claims exist");

            var newRole = new Role(role.Name);

            await _db.Roles.AddAsync(newRole);
            await _db.SaveChangesAsync();

            var roleClaims = role.ClaimIds.Select(s => new RoleClaim { ClaimId = s, RoleId = newRole.Id });

            await _db.RoleClaims.AddRangeAsync(roleClaims);
            await _db.SaveChangesAsync();

            return Result<RoleViewModel>.SuccessWithData(newRole.Adapt<RoleViewModel>());
        }

        public async Task<Result<bool>> DeleteRoleAsync(int id)
        {
            var roleForDelete = await _db.Roles.FindAsync(id);

            if (roleForDelete == null)
                return Result<bool>.NotFound("Role not found");

            var roleClaimsFroDelete = await _db.RoleClaims.AsNoTracking().Where(s => s.RoleId == id).ToListAsync();

            _db.RoleClaims.RemoveRange(roleClaimsFroDelete);
            _db.Roles.Remove(roleForDelete);
            await _db.SaveChangesAsync();

            return Result<bool>.SuccessWithData(true);
        }

        public async Task<Result<List<RoleViewModel>>> GetAllRolesAsync()
        {
            var roles = await _db.Roles.AsNoTracking().OrderByDescending(s => s.CreatedAt).ToListAsync();
            return Result<List<RoleViewModel>>.SuccessWithData(roles.Adapt<List<RoleViewModel>>());
        }

        public async Task<Result<RoleViewModel>> GetRoleByIdAsync(int id)
        {
            var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (role == null)
                return Result<RoleViewModel>.NotFound("Role not found");

            var claimsForRole = await _db.RoleClaims.Include(s => s.Claim).Where(s => s.RoleId == id).Select(s => s.Claim).ToListAsync();

            var roleToView = role.Adapt<RoleViewModel>();
            roleToView.Claims = claimsForRole.Adapt<List<ClaimViewModel>>();

            return Result<RoleViewModel>.SuccessWithData(roleToView);
        }

        public async Task<Result<bool>> UpdateClaimsRoleAsync(RoleClaimsDto roleClaims)
        {
            var roleId = roleClaims.Id;
            var roleClaimIds = roleClaims.ClaimIds;

            var roleToUpdate = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == roleId);

            if (roleToUpdate == null)
                return Result<bool>.NotFound("Role not found");

            if (!await IsAllClaimsExistAsync(roleClaimIds))
                return Result<bool>.Error("Not all claims exist");

            var roleClaimsFromDb = await _db.RoleClaims.AsNoTracking().Where(s => s.RoleId == roleId).ToListAsync();

            var claimIdsFromDb = roleClaimsFromDb.Select(s => s.ClaimId);

            var roleClaimsToDelete = roleClaimsFromDb.Where(s => !roleClaimIds.Contains(s.ClaimId));

            var roleClaimIdsToCreate = roleClaimIds.Where(s => !claimIdsFromDb.Contains(s));

            var roleClaimsToCreate = roleClaimIdsToCreate.Select(s => new RoleClaim { RoleId = roleId, ClaimId = s });

            _db.RoleClaims.RemoveRange(roleClaimsToDelete);
            await _db.RoleClaims.AddRangeAsync(roleClaimsToCreate);
            await _db.SaveChangesAsync();

            return Result<bool>.SuccessWithData(true);
        }

        public async Task<Result<RoleViewModel>> UpdateRoleAsync(RoleDto role)
        {
            var roleToUpdate = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(s => s.Id == role.Id);

            if (roleToUpdate == null)
                return Result<RoleViewModel>.NotFound("App not found");

            roleToUpdate.Name = role.Name;
            roleToUpdate.NameNormalized = role.Name.ToUpper();

            _db.Roles.Update(roleToUpdate);
            await _db.SaveChangesAsync();

            return Result<RoleViewModel>.SuccessWithData(roleToUpdate.Adapt<RoleViewModel>());
        }

        private async Task<bool> IsAllClaimsExistAsync(int[] claimsIds)
        {
            var claims = await _db.Claims.AsNoTracking().Where(s => claimsIds.Contains(s.Id)).ToListAsync();
            return claims.Count == claimsIds.Count();
        }
    }
}