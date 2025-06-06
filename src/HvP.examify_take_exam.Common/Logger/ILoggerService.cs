﻿using HvP.examify_take_exam.Common.Constants.Errors;

namespace HvP.examify_take_exam.Common.Logger
{
    public interface ILoggerService<T>
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogVerbose(string message, params object[] args);
        void LogFatal(string message, params object[] args);
        //void LogErrMsg(ErrorMsgModel errorMsg, Exception ex);
    }
}
