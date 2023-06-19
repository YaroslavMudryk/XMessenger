namespace XMessenger.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsAuthenticationRequired(this HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
                return false;
            var metadata = endpoint.Metadata;

            var existAuthAttrbt = metadata.Any(s => s is AuthorizeAttribute);
            if (existAuthAttrbt)
            {
                if (metadata.Any(s => s is AllowAnonymousAttribute))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
