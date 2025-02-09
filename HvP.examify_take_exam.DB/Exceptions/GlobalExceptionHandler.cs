using HvP.examify_take_exam.DB.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HvP.examify_take_exam.DB.Exceptions
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
            // write log
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            int threadId = Thread.CurrentThread.ManagedThreadId;
            string exceptionType = "GlobalException";
            string message = exception.Message ?? "No message provided";
            string stackTrace = exception.StackTrace ?? "No stack trace available";
            string errorCode = "INTERNAL_SERVER_ERROR";

            string logStr = $@"
                [EXCEPTION !!!]
                [Timestamp: {timestamp}] [LogLevel: ERROR] 
                [ExceptionType: {exceptionType}] [Message: {message}]
                [ErrorCode: {errorCode}] [Thread: {threadId}]
                [StackTrace: {stackTrace}]";

            _logger.LogError(logStr);

            httpContext.Response.StatusCode = StatusCode;

            var jsonRs = new ErrorResponseModel<object>()
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = "Internal Server Error",
                Details = exception.StackTrace
            };

            // response
            await httpContext.Response.WriteAsJsonAsync(jsonRs, cancellationToken);

            return true;
        }
    }
}
