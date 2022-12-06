using Microsoft.AspNetCore.Mvc;
using XMessenger.Application.Dtos;
using XMessenger.Application.Dtos.Identity;
using XMessenger.Web.Responses;

namespace XMessenger.Web.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiBaseController : Controller
    {
        [NonAction]
        protected IActionResult JsonResult<T>(Result<T> result)
        {
            if (result.IsCreated)
                return CreatedResult(result.Data);
            if (result.IsSuccess)
                return Ok(APIResponse.OkResponse(result.Data, result.Meta));
            if (result.IsNotFound)
                return NotFound(APIResponse.NotFoundResponse(result.ErrorMessage));
            if (result.IsError)
            {
                if (result.ErrorMessage == "Need MFA" && result.Data is JwtTokenDto)
                    return BadRequest(APIResponse.BadRequestResponse("Need MFA", (result.Data as JwtTokenDto).SessionId));
                return BadRequest(APIResponse.BadRequestResponse(result.ErrorMessage));
            }
            if (result.IsForbid)
                return JsonForbiddenResult();
            return JsonInternalServerErrorResult();
        }

        [NonAction]
        public IActionResult CreatedResult(object data, Meta meta = null)
        {
            HttpContext.Response.StatusCode = 201;
            return Json(APIResponse.OkResponse(data, meta));
        }

        [NonAction]
        public IActionResult JsonForbiddenResult()
        {
            HttpContext.Response.StatusCode = 403;
            return Json(APIResponse.ForbiddenResposne());
        }

        [NonAction]
        public IActionResult JsonInternalServerErrorResult()
        {
            HttpContext.Response.StatusCode = 500;
            return Json(APIResponse.InternalServerError(HttpContext.TraceIdentifier));
        }
    }
}