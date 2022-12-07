using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XMessenger.Application.Dtos.Identity;
using XMessenger.Application.Services;

namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class IdentityController : ApiBaseController
    {
        private readonly IAuthService _authService;
        public IdentityController(IAuthService authService)
        {
            _authService = authService;
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
            return JsonResult(await _authService.ConfirmAsync(code, userId));
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

        [HttpDelete("signout")]
        public async Task<IActionResult> Logout()
        {
            return JsonResult(await _authService.LogoutAsync());
        }

        [HttpPost("restore-password")]
        [AllowAnonymous]
        public IActionResult RestorePassword()
        {
            return Ok();
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword()
        {
            return Ok();
        }

        #endregion

        #region Sessions

        [HttpGet("sessions")]
        public async Task<IActionResult> GetUserSessions(int q = 0, int page = 1)
        {
            return JsonResult(await _authService.GetUserSessionsAsync(q, page));
        }

        [HttpDelete("sessions")]
        public async Task<IActionResult> CloseSessionById(Guid[] sessionIds)
        {
            return JsonResult(await _authService.LogoutBySessionIdsAsync(sessionIds));
        }

        #endregion
    }
}