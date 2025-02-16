using HvP.examify_take_exam.DB.Constants.Errors;

namespace HvP.examify_take_exam.DB.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string? message) : base(message) { }

        public NotFoundException(ErrorMsgModel errorMsg, object? details = null) : base(errorMsg, details) { }

    }
}
