namespace XMessenger.Identity.Services
{
    public interface IRoleService
    {
        Task<Result<List<RoleViewModel>>> GetAllRolesAsync();
        Task<Result<RoleViewModel>> GetRoleByIdAsync(int id);
        Task<Result<RoleViewModel>> CreateRoleAsync(RoleDto role);
        Task<Result<RoleViewModel>> UpdateRoleAsync(RoleDto role);
        Task<Result<bool>> UpdateClaimsRoleAsync(RoleClaimsDto roleClaims);
        Task<Result<bool>> DeleteRoleAsync(int id);
    }
}