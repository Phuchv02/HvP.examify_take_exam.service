using HvP.examify_take_exam.DB.Constants.Errors;

namespace HvP.examify_take_exam.DB.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string? message) : base(message) { }

        public BadRequestException(ErrorMsgModel errorMsg) : base(errorMsg) { }

        public override string GetLogStr()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var exceptionType = this.GetType().Name;
            var message = this.ErrorMsg?.Message ?? this.Message ?? "No message provided";
            var stackTrace = this.StackTrace ?? "No stack trace available";
            var errorCode = this.ErrorMsg?.ErrorCode ?? "N/A";
            var details = this.ErrorMsg?.Details ?? "No details available";

            return $@"
                [EXCEPTION !!!]
                [Timestamp: {timestamp}] [LogLevel: ERROR] 
                [ExceptionType: {exceptionType}] [Message: {message}]
                [ErrorCode: {errorCode}] [Thread: {threadId}]
                [StackTrace: {stackTrace}]
                [AdditionalData: {details}]";
        }
    }
}
