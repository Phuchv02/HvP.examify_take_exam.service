using HvP.examify_take_exam.Common.Constants.Errors;

namespace HvP.examify_take_exam.Common.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string? message) : base(message) { }

        public NotFoundException(ErrorMsgModel errorMsg, object? details = null) : base(errorMsg, details) { }

    }
}
