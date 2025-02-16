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
        public string? LogMessage { get; set; }

        public ErrorMsgModel() { }
    }

    public static class ErrorMsg
    {
        // Common Error
        public static ErrorMsgModel ErrQueryDatabase = new()
        {
            ErrorCode = "ERR_QUERY_DB",
            Message = "Query database failure"
        };

        public static ErrorMsgModel ErrGetEnvConfig(string configKey) => new()
        {
            ErrorCode = "ERR_GET_ENV_CONFIG",
            Message = $"Get config from ENV error",
            LogMessage = $"Get config from ENV error, ConfigKey: {configKey}"
        };

        // BadRequest
        public static ErrorMsgModel ErrBadRequest = new()
        {
            ErrorCode = "BAD_REQUEST Code",
            Message = "Bad Request"
        };
    }
}
