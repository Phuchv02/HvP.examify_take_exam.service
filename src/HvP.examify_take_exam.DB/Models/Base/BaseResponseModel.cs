using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HvP.examify_take_exam.DB.Models
{
    // success response
    public class SuccessResponseModel<T>
    {
        public string Status { get; set; } = "success";
        public string Message { get; set; }
        public T Data { get; set; }

        public SuccessResponseModel(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }

    // error response
    public class ErrorResponseModel<T>
    {
        public string Status { get; set; } = "error";
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T? Details { get; set; }

        public ErrorResponseModel() { }

        public ErrorResponseModel(string errorCode, string message, T? details)
        {
            Message = message;
            ErrorCode = errorCode;
            Details = details;
        }
    }

    // validate error response
    public class ValidationErrorResponseModel
    {
        public string Status { get; set; } = "fail";
        public string Message { get; set; }
        public List<ValidationErrorModel> Errors { get; set; } = new();

        public ValidationErrorResponseModel(string message, List<ValidationErrorModel> errors)
        {
            Message = message;
            Errors = errors;
        }
    }

    public class ValidationErrorModel
    {
        public string Field { get; set; }
        public string Message { get; set; }

        public ValidationErrorModel(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }

}
