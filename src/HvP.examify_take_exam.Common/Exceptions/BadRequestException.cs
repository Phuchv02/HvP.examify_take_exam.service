using HvP.examify_take_exam.Common.Constants.Errors;

namespace HvP.examify_take_exam.Common.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string? message) : base(message) { }

        public BadRequestException(ErrorMsgModel errorMsg, object? details = null) : base(errorMsg, details) { }

    }
}
