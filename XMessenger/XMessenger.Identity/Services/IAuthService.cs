using XMessenger.Helpers;
using XMessenger.Identity.Dtos;

namespace XMessenger.Identity.Services
{
    public interface IAuthService
    {
        Task<Result<bool>> ChangeLoginAsync(NewLoginDto loginDto);
        Task<Result<bool>> ChangePasswordAsync(NewPasswordDto passwordDto);
        Task<Result<bool>> ConfirmAccountAsync(string code, int userId);
        Task<Result<bool>> DisableMFAAsync(string code);
        Task<Result<MFADto>> EnableMFAAsync(string code = null);
        Task<Result<JwtTokenDto>> LoginByMFAAsync(LoginMFADto mfaDto);
        Task<Result<JwtTokenDto>> LoginByPasswordAsync(LoginDto loginDto);
        Task<Result<bool>> LogoutAsync();
        Task<Result<JwtTokenDto>> RefreshTokenAsync(string refreshToken);
        Task<Result<bool>> RegisterAsync(RegisterDto registerDto);
        Task<Result<bool>> RestorePasswordAsync(RestorePasswordDto restorePasswordDto);
        Task<Result<bool>> SendConfirmAsync(int userId);
    }
}
