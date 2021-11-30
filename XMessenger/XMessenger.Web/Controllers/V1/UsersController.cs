using Microsoft.AspNetCore.Mvc;
namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class UsersController : ApiBaseController
    {
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            return Ok();
        }
    }
}