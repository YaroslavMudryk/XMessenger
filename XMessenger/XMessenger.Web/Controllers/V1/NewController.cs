namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class NewController : ApiBaseController
    {
        private readonly ISeederService _seederService;
        public NewController(ISeederService seederService)
        {
            _seederService = seederService;
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return JsonResult(await _seederService.SeedSystemAsync());
        }
    }
}
