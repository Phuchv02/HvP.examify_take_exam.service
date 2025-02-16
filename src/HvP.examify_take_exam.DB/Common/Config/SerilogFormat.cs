using Serilog.Events;
using Serilog.Formatting;

namespace HvP.DB.Common.Config
{
    public class SerilogFormat : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            string value = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string value2 = logEvent.Level.ToString();
            string propertyValue = GetPropertyValue(logEvent, "SourceContext");
            string propertyValue2 = GetPropertyValue(logEvent, "Exception");
            propertyValue2 = ((propertyValue2 == null) ? "" : ("\r\n" + propertyValue2));
            string value3 = logEvent.RenderMessage();
            string value4 = ((propertyValue == null) ? "" : (" [" + propertyValue.Substring(propertyValue.LastIndexOf('.') + 1) + "]"));
            SetColor(logEvent);
            output.WriteLine($"{value} [{value2}]{value4} - {value3}{propertyValue2}");
        }

        private string GetPropertyValue(LogEvent logEvent, string propertyName)
        {
            if (!logEvent.Properties.ContainsKey(propertyName))
            {
                return null;
            }

            return ((ScalarValue)logEvent.Properties[propertyName]).Value?.ToString();
        }

        private void SetColor(LogEvent logEvent)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogEventLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogEventLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogEventLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                default:
                    Console.ForegroundColor = foregroundColor;
                    break;
            }
        }
    }
}