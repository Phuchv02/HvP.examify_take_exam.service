using HvP.examify_take_exam.Common.Logger;
using Microsoft.AspNetCore.Http;

namespace HvP.examify_take_exam.Common.Middlewares
{
    public class TraceRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Init TraceId
            var traceId = context.Request.Headers["X-Trace-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();

            // Create scope using TraceId
            using (TraceContext.BeginTraceScope(traceId))
            {
                context.Response.Headers["X-Trace-Id"] = traceId;
                await _next(context);
            }
        }
    }
}
