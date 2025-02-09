using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HvP.examify_take_exam.DB.Constants.Errors
{
    public class ErrorMsgModel
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public object? Details { get; set; }

        public ErrorMsgModel() { }
    }

    public static class ErrorMsg
    {
        // BadRequest
        public static ErrorMsgModel ErrBadRequest = new()
        {
            ErrorCode = "BAD_REQUEST Code",
            Message = "Bad Request 123"
        };
    }
}
