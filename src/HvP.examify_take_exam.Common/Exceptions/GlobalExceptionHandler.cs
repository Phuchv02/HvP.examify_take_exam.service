using HvP.examify_take_exam.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HvP.examify_take_exam.Common.Exceptions
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly int StatusCode = 500;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            ErrorResponseModel<object> jsonRs = new();
            string logStr = "";

            if (exception is BaseException baseException)
            {
                logStr = baseException.GetLogStr();
                jsonRs = baseException.GetResponseError();
            }
            else
            {
                // default log str
                string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                int threadId = Thread.CurrentThread.ManagedThreadId;
                string exceptionType = "GlobalException";
                string message = exception.Message ?? "No message provided";
                string stackTrace = exception.StackTrace ?? "No stack trace available";
                string errorCode = "INTERNAL_SERVER_ERROR";
                logStr = $@"
                [EXCEPTION !!!]
                [Timestamp: {timestamp}] [LogLevel: ERROR] 
                [ExceptionType: {exceptionType}] [Message: {message}]
                [ErrorCode: {errorCode}] [Thread: {threadId}]
                [StackTrace: {stackTrace}]";

                // default err rs
                jsonRs.ErrorCode = "INTERNAL_SERVER_ERROR";
                jsonRs.Message = "Internal Server Error";
                jsonRs.Details = exception.StackTrace;
            }

            // write log
            _logger.LogError(logStr);

            httpContext.Response.StatusCode = StatusCode;

            // response
            await httpContext.Response.WriteAsJsonAsync(jsonRs, cancellationToken);

            return true;
        }
    }
}
