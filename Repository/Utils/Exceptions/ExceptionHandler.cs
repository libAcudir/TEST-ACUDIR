

using Repository.Results;

namespace Utils.Exception
{
    public static class ExceptionHandler
    {
        public static ErrorResult CreateErrorResult(System.Exception exception)
        {
            ErrorResult errorResult = new ErrorResult();

            if(exception.InnerException != null)
            {
                errorResult.traceId = exception.InnerException.GetHashCode().ToString();
                errorResult.message = exception.InnerException.Message;

            }else
            {
                errorResult.traceId = exception.GetHashCode().ToString();
                errorResult.message = exception.Message;
            }
           
            return errorResult;
        }
    }
}
