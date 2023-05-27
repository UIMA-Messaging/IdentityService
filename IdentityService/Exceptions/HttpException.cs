namespace IdentityService.Exceptions
{
    public abstract class HttpException : Exception
    {
        public readonly string? Message;
        public readonly int StatusCode;

        protected HttpException(int statusCode, string message = "") 
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
