namespace HvP.examify_take_exam.Common.Constants.Errors
{
    // # ERROR MSG MODEL
    public class ErrorMsgModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string? LogMessage { get; set; }

        public ErrorMsgModel() { }
    }

    // # INFOF MSG MODEL
    public class InforMsgModel
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public InforMsgModel() { }

        public string GetLogStr() => $"[{Code}] - {Message}";
    }
}
