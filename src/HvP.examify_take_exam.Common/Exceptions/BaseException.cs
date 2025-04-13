using System.Net;
using HvP.examify_take_exam.Common.Constants.Errors;
using HvP.examify_take_exam.Common.Models;

namespace HvP.examify_take_exam.Common.Exceptions
{
    public class BaseException : Exception
    {
        public ErrorMsgModel ErrorMsg { get; }
        public object? Details { get; set; }

        // default constructor
        public BaseException(string? message) : base(message) { }

        // constructor with errorMsg
        public BaseException(ErrorMsgModel errorMsg, object? details = null)
        {
            this.ErrorMsg = errorMsg;
            this.Details = details;
        }

        public virtual ErrorResponseModel<object> GetResponseError()
        {
            return new ErrorResponseModel<object>()
            {
                ErrorCode = this.ErrorMsg.ErrorCode,
                Message = this.ErrorMsg.Message,
                Details = this?.Details?.ToString()
            };
        }

        public virtual string GetLogStr()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var exceptionType = this.GetType().Name;
            var message = this.ErrorMsg?.LogMessage ?? this.ErrorMsg?.Message ?? this.Message ?? "No message provided";
            var stackTrace = this.StackTrace ?? "No stack trace available";
            var errorCode = this.ErrorMsg?.ErrorCode ?? "N/A";
            var details = this.Details ?? "No details available";

            return $@"
                [EXCEPTION !!!]
                [{exceptionType}]
                [ErrorCode: {errorCode}] - [Message: {message}]
                [Details: {details}]
                [StackTrace: {stackTrace}]";
        }
    }
}
