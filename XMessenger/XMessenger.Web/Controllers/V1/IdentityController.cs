using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XMessenger.Identity.Dtos;
using XMessenger.Identity.Services;

namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class IdentityController : ApiBaseController
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;
        private readonly IAppService _appService;
        public IdentityController(IAuthService authService, ISessionService sessionService, IAppService appService)
        {
            _authService = authService;
            _sessionService = sessionService;
            _appService = appService;
        }

        #region Identity

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromBody] RegisterDto registerDto)
        {
            return JsonResult(await _authService.RegisterAsync(registerDto));
        }

        [HttpPost("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string code, int userId)
        {
            return JsonResult(await _authService.ConfirmAccountAsync(code, userId));
        }

        [HttpPost("send-confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> SendConfirm(int userId)
        {
            return JsonResult(await _authService.SendConfirmAsync(userId));
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return JsonResult(await _authService.LoginByPasswordAsync(loginDto));
        }

        [HttpPost("signin-2mfa")]
        [AllowAnonymous]
        public async Task<IActionResult> Login2MFA([FromBody] LoginMFADto loginMFADto)
        {
            return JsonResult(await _authService.LoginByMFAAsync(loginMFADto));
        }

        [HttpPost("2mfa")]
        public async Task<IActionResult> EnableMFA(string code = null)
        {
            return JsonResult(await _authService.EnableMFAAsync(code));
        }

        [HttpDelete("2mfa/{code}")]
        public async Task<IActionResult> DisableMFA(string code)
        {
            return JsonResult(await _authService.DisableMFAAsync(code));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string token)
        {
            return JsonResult(await _authService.RefreshTokenAsync(token));
        }

        [HttpDelete("signout")]
        public async Task<IActionResult> Logout()
        {
            return JsonResult(await _authService.LogoutAsync());
        }

        [HttpPost("restore-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RestorePassword(RestorePasswordDto restorePasswordDto)
        {
            return JsonResult(await _authService.RestorePasswordAsync(restorePasswordDto));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] NewPasswordDto passwordDto)
        {
            return JsonResult(await _authService.ChangePasswordAsync(passwordDto));
        }

        [HttpPost("change-login")]
        public async Task<IActionResult> ChangeLogin([FromBody] NewLoginDto loginDto)
        {
            return JsonResult(await _authService.ChangeLoginAsync(loginDto));
        }

        #endregion

        #region Sessions

        [HttpGet("sessions")]
        public async Task<IActionResult> GetUserSessions(int q = 0, int page = 1)
        {
            return JsonResult(await _sessionService.GetUserSessionsAsync(q, page));
        }

        [HttpDelete("sessions")]
        public async Task<IActionResult> CloseSessionById(Guid[] sessionIds)
        {
            return JsonResult(await _sessionService.CloseSessionsByIdsAsync(sessionIds));
        }

        #endregion

        #region Apps

        [HttpPost("apps")]
        public async Task<IActionResult> CreateApp([FromBody] AppDto appDto)
        {
            return JsonResult(await _appService.CreateAppAsync(appDto));
        }

        [HttpPut("apps/{id}")]
        public async Task<IActionResult> UpdateApp(int id, [FromBody] AppDto appDto)
        {
            appDto.Id = id;
            return JsonResult(await _appService.UpdateAppAsync(appDto));
        }

        [HttpPut("apps/{id}/secret")]
        public async Task<IActionResult> UpdateSecretApp(int id)
        {
            return JsonResult(await _appService.ChangeAppSecretAsync(id));
        }

        [HttpPut("apps/{id}/claims")]
        public async Task<IActionResult> UpdateClaimsApp(int id, [FromBody] AppClaimsDto appClaims)
        {
            appClaims.Id = id;
            return JsonResult(await _appService.UpdateAppClaimsAsync(appClaims));
        }

        [HttpDelete("apps/{id}")]
        public async Task<IActionResult> DeleteApp(int id)
        {
            return JsonResult(await _appService.DeleteAppAsync(id));
        }

        [HttpGet("apps")]
        public async Task<IActionResult> GetAllApps(int page)
        {
            return JsonResult(await _appService.GetAllAppsAsync(page));
        }

        [HttpGet("apps/{id}")]
        public async Task<IActionResult> GetApp(int id)
        {
            return JsonResult(await _appService.GetAppByIdAsync(id));
        }

        [HttpGet("apps/{id}/secret")]
        public async Task<IActionResult> GetAppSecret(int id)
        {
            return JsonResult(await _appService.GetAppSecretAsync(id));
        }

        [HttpGet("apps/my")]
        public async Task<IActionResult> GetAllApps()
        {
            return JsonResult(await _appService.GetMyAppsAsync());
        }

        #endregion
    }
}