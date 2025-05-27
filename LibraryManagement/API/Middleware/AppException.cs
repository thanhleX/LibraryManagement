namespace LibraryManagement.API.Middleware
{
    public class AppException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public AppException(ErrorCode errorCode, params object[] args)
            : base(errorCode.FormattedMessage(args))
        {
            ErrorCode = errorCode;
        }
    }
}
