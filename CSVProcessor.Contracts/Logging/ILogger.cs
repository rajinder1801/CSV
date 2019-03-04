using System;

namespace CSVProcessor.Contracts.Logging
{
    public interface ILogger
    {
        void LogInfoMessage(string format, string message);
        void LogErrorMessage(string format, string message);
        void LogError(string message, Exception ex);
    }
}
