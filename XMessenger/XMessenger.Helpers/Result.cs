namespace XMessenger.Helpers
{
    public class Result<T>
    {
        #region ctors
        public Result(bool success, bool isCreated, bool notFound, bool forbid, string error, Exception exception, T data, Meta meta)
        {
            IsSuccess = success;
            IsCreated = isCreated;
            IsNotFound = notFound;
            IsForbid = forbid;
            IsError = string.IsNullOrEmpty(error) ? false : true;
            ErrorMessage = error;
            ExceptionType = exception;
            Data = data;
            Meta = meta;
        }
        public Result()
        {

        }
        #endregion

        #region Methods

        public static Result<T> Created(T data)
        {
            return new Result<T>(true, true, false, false, null, null, data, null);
        }

        public static Result<T> Created()
        {
            return new Result<T>(true, true, false, false, null, null, default, null);
        }

        public static Result<T> CreatedList(T data, Meta meta)
        {
            return new Result<T>(true, true, false, false, null, null, data, meta);
        }

        public static Result<T> Success()
        {
            return new Result<T>(true, false, false, false, null, null, default, null);
        }

        public static Result<T> SuccessWithData(T data)
        {
            if (data == null)
                return Success();
            return new Result<T>(true, false, false, false, null, null, data, null);
        }

        public static Result<T> SuccessList(T data, Meta meta = null)
        {
            if (data == null)
                return Success();
            return new Result<T>(true, false, false, false, null, null, data, meta);
        }

        public static Result<T> NotFound(string error = "Resource not found")
        {
            return new Result<T>(false, false, true, false, error, null, default, null);
        }

        public static Result<T> Error(string error = "Resource not found")
        {
            return new Result<T>(false, false, false, false, error, null, default, null);
        }

        public static Result<T> Forbiden(string error = "Access denited")
        {
            return new Result<T>(false, false, false, true, error, null, default, null);
        }

        public static Result<T> Exception(Exception exception)
        {
            return new Result<T>(false, false, false, false, null, exception, default, null);
        }

        #endregion

        #region Props

        public bool IsSuccess { get; set; }
        public bool IsCreated { get; set; }
        public bool IsNotFound { get; set; }
        public bool IsError { get; set; }
        public bool IsForbid { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ExceptionType { get; set; }
        public T Data { get; set; }
        public Meta Meta { get; set; }

        #endregion

        public Result<U> MapToNew<U>(U data, Meta meta = null)
        {
            return new Result<U>(IsSuccess, IsCreated, IsNotFound, IsForbid, ErrorMessage, ExceptionType, data, meta);
        }
    }

    public class Meta
    {
        public int TotalPages { get; set; }
        public int Page { get; set; }

        public static Meta FromMeta(int totalCount, int page)
        {
            var totalPages = 1;

            var res = (double)(totalCount / Paginations.PerPage);
            if (res % 10 == 0)
                totalPages = (int)res;
            else
                totalPages = (int)Math.Ceiling(res);

            var meta = new Meta();
            meta.Page = page;
            meta.TotalPages = totalPages;
            return meta;
        }
    }
}
