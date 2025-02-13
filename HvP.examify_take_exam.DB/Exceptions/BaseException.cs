﻿using System.Net;
using HvP.examify_take_exam.DB.Constants.Errors;

namespace HvP.examify_take_exam.DB.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;
        public ErrorMsgModel ErrorMsg { get; }
        public object? Details { get; set; }

        // default constructor
        public BaseException(string? message) : base(message) { }

        // constructor with errorMsg
        public BaseException(ErrorMsgModel errorMsg, object? details = null)
        {
            this.ErrorMsg = errorMsg;
            this.Details = details;
        }

        public virtual string GetLogStr()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var exceptionType = this.GetType().Name;
            var message = this.ErrorMsg?.Message ?? this.Message ?? "No message provided";
            var stackTrace = this.StackTrace ?? "No stack trace available";
            var errorCode = this.ErrorMsg?.ErrorCode ?? "N/A";
            var details = this.Details ?? "No details available";

            return $@"
                [EXCEPTION !!!]
                [Timestamp: {timestamp}] [LogLevel: ERROR] 
                [ExceptionType: {exceptionType}] - [Thread: {threadId}]
                [ErrorCode: {errorCode}] - [Message: {message}]
                [Details: {details}]
                [StackTrace: {stackTrace}]";
        }
    }
}
