using Microsoft.AspNetCore.Mvc;
using XMessenger.Application.Dtos.Identity;
using XMessenger.Application.Services;

namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class IdentityController : ApiBaseController
    {
        private readonly IAuthService _authService;
        public IdentityController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Identity

        [HttpPost("signup")]
        public async Task<IActionResult> Registration([FromBody] RegisterDto registerDto)
        {
            return JsonResult(await _authService.RegisterAsync(registerDto));
        }

        [HttpPost("confirm")]
        public IActionResult Confirm()
        {
            return Ok();
        }

        [HttpPost("send-confirm")]
        public IActionResult SendConfirm()
        {
            return Ok();
        }

        [HttpPost("signin")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("signin-2mfa")]
        public IActionResult Login2MFA()
        {
            return Ok();
        }

        [HttpDelete("signout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpPost("restore-password")]
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