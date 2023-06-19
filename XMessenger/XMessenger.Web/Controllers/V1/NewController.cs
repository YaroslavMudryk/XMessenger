namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class NewController : ApiBaseController
    {
        private readonly IEnumerable<ISeederService> _seederServices;
        public NewController(IEnumerable<ISeederService> seederServices)
        {
            _seederServices = seederServices;
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            int count = 0;

            foreach (var seederService in _seederServices)
            {
                var res = await seederService.SeedSystemAsync();

                if (res.Data > 0)
                    count++;
            }

            return JsonResult(Result<string>.SuccessWithData($"Initialized: {count} databases"));
        }
    }
}
