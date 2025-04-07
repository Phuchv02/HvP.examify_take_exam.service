using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Text;

namespace HvP.examify_take_exam.DB.Logger
{
    public sealed class LoggerService<T> : ILoggerService<T>, IDisposable
    {
        private readonly ILogger _logger;

        public LoggerService()
        {
            _logger = Log.ForContext<T>();
        }

        public void LogInformation(string message, params object[] args)
        {
            this._logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            this._logger.Warning(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            this._logger.Error(message, args);
        }

        public void LogFatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public void LogVerbose(string message, params object[] args)
        {
            _logger.Verbose(message, args);
        }

        public static readonly string _cachedOutputTemplate = GetOutPutTemplate();
        public static readonly ConsoleTheme _cachedTheme = GetColorTheme();

        public static string GetOutPutTemplate()
        {
            return new StringBuilder()
            .Append($"[{{Timestamp:HH:mm:ss}}][{{Level:u5}}]")
            .Append($"-[PID:{{ProcessId}}|TID:{{ThreadId}}|MEM:{{MemoryUsedMB}}MB]")
            .Append($"-[TraceId:{{CustomTraceId}}]")
            .Append($"-[{{SourceContextShort}}]: {{Message:lj}}{{NewLine}}{{Exception:j}}")
            .ToString();
        }

        public static AnsiConsoleTheme GetColorTheme()
        {
            // # ANSI Color Codes
            return new AnsiConsoleTheme(
                new Dictionary<ConsoleThemeStyle, string>
                {
                    [ConsoleThemeStyle.Text] = "\x1b[0;33;49m",
                    [ConsoleThemeStyle.SecondaryText] = "\x1b[1;34;49m",
                    [ConsoleThemeStyle.TertiaryText] = "\x1b[38;5;0242m",
                    [ConsoleThemeStyle.Invalid] = "\x1b[33;1m",
                    [ConsoleThemeStyle.Null] = "\x1b[38;5;0038m",
                    [ConsoleThemeStyle.Name] = "\x1b[38;5;0081m",
                    [ConsoleThemeStyle.String] = "\x1b[38;5;0216m",
                    [ConsoleThemeStyle.Number] = "\x1b[38;5;151m",
                    [ConsoleThemeStyle.Boolean] = "\x1b[38;5;0038m",
                    [ConsoleThemeStyle.Scalar] = "\x1b[38;5;0079m",
                    [ConsoleThemeStyle.LevelVerbose] = "\x1b[37m",
                    [ConsoleThemeStyle.LevelDebug] = "\x1b[37;1m",
                    [ConsoleThemeStyle.LevelInformation] = "\x1b[32m",
                    [ConsoleThemeStyle.LevelWarning] = "\x1b[33;1m",
                    [ConsoleThemeStyle.LevelError] = "\x1b[31;1m",
                    [ConsoleThemeStyle.LevelFatal] = "\x1b[37;1m\x1b[48;5;0196m",
                }
            );
        }

        public void Dispose() { }
    }
}
