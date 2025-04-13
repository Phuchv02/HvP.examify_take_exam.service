namespace HvP.examify_take_exam.Common.Constants.Errors
{
    // # COMMON INFORS
    public static partial class InforMsg
    {
        public static InforMsgModel InfFuncStart(string funcName) => new()
        {
            Code = "INF_FUNC_START",
            Message = $"Function Start: {funcName}"
        };

    }
}
