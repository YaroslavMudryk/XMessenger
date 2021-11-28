namespace XMessenger.Web.Responses
{
    public class ApiResponse
    {
        public ApiResponse() : this(true, null, null)
        {

        }
        public ApiResponse(object data) : this(true, null, data)
        {

        }
        public ApiResponse(string error, object data = null) : this(false, error, data)
        {

        }
        public ApiResponse(bool ok, string error, object data)
        {
            Ok = ok;
            Error = error;
            Data = data;
        }

        public bool Ok { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }
    }
}