using XMessenger.Application.Dtos;

namespace XMessenger.Web.Responses
{
    public class APIResponse
    {
        public APIResponse(bool ok, string message, string description, object data, Meta meta)
        {
            Ok = ok;
            Message = message;
            Description = description;
            Data = data;
            Meta = meta;
        }

        public bool Ok { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public object Data { get; set; }
        public Meta Meta { get; set; }

        public static APIResponse OkResponse(object data = null, Meta meta = null)
        {
            return new APIResponse(true, null, null, data, meta);
        }

        public static APIResponse CreatedResponse(object data = null)
        {
            return new APIResponse(true, null, null, data, null);
        }

        public static APIResponse BadRequestResponse(string error, object data = null)
        {
            return new APIResponse(false, error, null, data, null);
        }

        public static APIResponse UnauthorizedResposne(string error = "Unauthorized")
        {
            return new APIResponse(false, error, "Access denited", null, null);
        }

        public static APIResponse ForbiddenResposne()
        {
            return new APIResponse(false, "Forbidden", "You are missing access rights", null, null);
        }

        public static APIResponse NotFoundResponse(string error = "Resource not found")
        {
            return new APIResponse(false, error, null, null, null);
        }

        public static APIResponse InternalServerError(string requestId)
        {
            return new APIResponse(false, "Internal server error", null, requestId, null);
        }
    }
}