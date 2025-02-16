using HvP.examify_take_exam.DB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HvP.examify_take_exam.DB.Extentions
{
    public static class ControllerBaseExtention
    {
        // response success
        public static IActionResult ResponseSuccess(this ControllerBase controllerBase, object data, string message = "")
        {
            var jsonRs = new SuccessResponseModel<object>(message, data);
            string jsonContent = JsonSerializer.Serialize(jsonRs);

            //TODO: write log

            return new ContentResult
            {
                Content = jsonContent,
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        // response error
        public static IActionResult ResponseError(
            this ControllerBase controllerBase,
            string message = "",
            string errCode = "",
            object? details = null)
        {
            var jsonRs = new ErrorResponseModel<object>(errCode, message, details);
            string jsonContent = JsonSerializer.Serialize(jsonRs);

            return new ContentResult
            {
                Content = jsonContent,
                ContentType = "application/json",
                StatusCode = 400
            };
        }

        // response validation error
        public static IActionResult ResponseValidateError(
            this ControllerBase controllerBase,
            List<ValidationErrorModel> errors,
            string message = "validate fiels error")
        {
            var jsonRs = new ValidationErrorResponseModel(message, errors);
            string jsonContent = JsonSerializer.Serialize(jsonRs);

            return new ContentResult
            {
                Content = jsonContent,
                ContentType = "application/json",
                StatusCode = 400
            };
        }
    }
}
