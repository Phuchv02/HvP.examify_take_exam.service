using HvP.examify_take_exam.DB.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HvP.examify_take_exam.DB.Exceptions
{
    public sealed class BadRequestExceptionHandler : IExceptionHandler
    {
        private readonly int StatusCode = 400;
        private readonly ILogger<BadRequestExceptionHandler> _logger;

        public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not BadRequestException badRequestException)
            {
                return false;
            }

            // write log
            _logger.LogError(badRequestException.GetLogStr());

            httpContext.Response.StatusCode = StatusCode;

            var jsonRs = new ErrorResponseModel<object>()
            {
                ErrorCode = badRequestException.ErrorMsg.ErrorCode,
                Message = badRequestException.ErrorMsg.Message,
                Details = badRequestException.ErrorMsg.Details
            };

            // response
            await httpContext.Response.WriteAsJsonAsync(jsonRs, cancellationToken);

            return true;
        }
    }
}
