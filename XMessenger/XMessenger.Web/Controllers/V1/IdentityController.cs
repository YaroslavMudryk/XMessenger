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
            var d = this.ModelState;
            return JsonResult(await _authService.LoginByPasswordAsync(loginDto));
        }

        [HttpPost("signin-2mfa")]
        [AllowAnonymous]
        public async Task<IActionResult> Login2MFA([FromBody] LoginMFADto loginMFADto)
        {
            return JsonResult(await _authService.LoginByMFAAsync(loginMFADto));
        }

        [HttpDelete("signout")]
        public IActionResult Logout()
        {
            return Ok();
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
        public IActionResult GetUserSessions(int q, int page = 1)
        {
            return Ok();
        }

        [HttpGet("sessions/{sessionId}")]
        public IActionResult GetSessionById(Guid sessionId)
        {
            return Ok();
        }

        [HttpDelete("sessions/{sessionId}")]
        public IActionResult CloseSessionById(Guid sessionId)
        {
            return Ok();
        }

        #endregion
    }
}