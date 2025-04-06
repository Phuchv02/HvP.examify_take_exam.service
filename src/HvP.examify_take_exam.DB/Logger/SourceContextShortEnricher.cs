using Serilog.Core;
using Serilog.Events;

namespace HvP.examify_take_exam.DB.Logger
{
    public class SourceContextShortEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var sc))
            {
                var fullName = sc.ToString().Trim('"');
                var shortName = fullName[(fullName.LastIndexOf('.') + 1)..];
                var shortNameColor = $"\x1b[35m{shortName}\u001b[0m";
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SourceContextShort", shortNameColor));
            }
        }
    }
}
