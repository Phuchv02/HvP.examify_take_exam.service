using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace HvP.examify_take_exam.Common.Logger
{
    public class MemoryUsageEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // Get current memory usage (unit MB)
            var memoryUsed = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;

            // Add property
            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("MemoryUsedMB", memoryUsed));
        }
    }
}
