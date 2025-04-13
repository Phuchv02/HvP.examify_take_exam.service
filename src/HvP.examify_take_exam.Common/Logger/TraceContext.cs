using HvP.examify_take_exam.Common.Helpers;

namespace HvP.examify_take_exam.Common.Logger
{
    public class TraceContext
    {
        private static readonly AsyncLocal<string> _traceId = new();

        public static string CurrentTraceId
        {
            get => _traceId.Value;
            set => _traceId.Value = value;
        }

        public static IDisposable BeginTraceScope(string traceId)
        {
            var oldTraceId = CurrentTraceId;
            CurrentTraceId = traceId;

            return new DisposeAction(() =>
            {
                CurrentTraceId = oldTraceId;
            });
        }
    }
}
