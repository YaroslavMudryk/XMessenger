namespace XMessenger.Helpers.Services
{
    public interface IIdentityService
    {
        int GetUserId();
        string GetIP();
        bool IsAdmin();
        IEnumerable<string> GetRoles();
    }

    public class HttpIdentityService : IIdentityService
    {
        private readonly HttpContext _httpContext;
        public HttpIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public int GetUserId()
        {
            if (!IsAuth())
                return 0;

            var claim = _httpContext.User.Claims.FirstOrDefault(s => s.Type == ConstantsClaimTypes.UserId);
            return Convert.ToInt32(claim.Value);
        }

        public string GetIP()
        {
            return _httpContext.Connection.RemoteIpAddress.ToString();
        }

        public bool IsAdmin()
        {
            if (!IsAuth())
                return false;
            return GetRoles().Any(s => s.Contains(DefaultsRoles.Administrator));
        }

        public IEnumerable<string> GetRoles()
        {
            if (!IsAuth())
                return Enumerable.Empty<string>();
            var claims = _httpContext.User.Claims.Where(s => s.Type == ConstantsClaimTypes.Role);
            return claims.Select(x => x.Value);
        }

        private bool IsAuth()
        {
            return _httpContext.User.Identity.IsAuthenticated;
        }
    }
}