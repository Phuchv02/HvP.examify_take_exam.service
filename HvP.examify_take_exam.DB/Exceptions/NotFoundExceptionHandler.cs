using HvP.examify_take_exam.DB.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HvP.examify_take_exam.DB.Exceptions
{
    public sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly int StatusCode = 404;
        private readonly ILogger<NotFoundExceptionHandler> _logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFoundException)
            {
                return false;
            }

            // write log
            _logger.LogError(notFoundException.GetLogStr());

            httpContext.Response.StatusCode = StatusCode;

            var jsonRs = new ErrorResponseModel<object>()
            {
                ErrorCode = notFoundException.ErrorMsg.ErrorCode,
                Message = notFoundException.ErrorMsg.Message,
                Details = notFoundException.ErrorMsg.Details
            };

            // response
            await httpContext.Response.WriteAsJsonAsync(jsonRs, cancellationToken);

            return true;
        }
    }
}
