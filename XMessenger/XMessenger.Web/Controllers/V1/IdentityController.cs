using Microsoft.AspNetCore.Mvc;
namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class IdentityController : ApiBaseController
    {
        #region Identity

        [HttpPost("registration")]
        public IActionResult Registration()
        {
            return Ok();
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

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("login-2mfa")]
        public IActionResult Login2MFA()
        {
            return Ok();
        }

        [HttpDelete("logout")]
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
        public IActionResult GetSessionById(long id)
        {
            return Ok();
        }

        [HttpDelete("sessions/{sessionId}")]
        public IActionResult CloseSessionById(long id)
        {
            return Ok();
        }

        #endregion
    }
}