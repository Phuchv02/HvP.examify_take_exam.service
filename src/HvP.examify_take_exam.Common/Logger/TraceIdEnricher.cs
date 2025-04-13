using Serilog.Core;
using Serilog.Events;

namespace HvP.examify_take_exam.Common.Logger
{
    public class TraceIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var traceId = TraceContext.CurrentTraceId;

            if (!string.IsNullOrEmpty(traceId))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CustomTraceId", traceId));
            }
        }
    }
}