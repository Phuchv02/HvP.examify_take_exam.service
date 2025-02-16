namespace HvP.examify_take_exam.DB.Constants.Errors
{
    // # BAD REQUEST ERRORS
    public static partial class ErrorMsg
    {
        public static ErrorMsgModel ErrBadRequest = new()
        {
            ErrorCode = "BAD_REQUEST",
            Message = "Bad Request"
        };
    }
}
