using CSVProcessor.Contracts.Logging;
using log4net;
using System;
using System.IO;

namespace CSVProcessor.Logging
{
    public class Logger : ILogger
    {
        private ILog log4NetLogger;
        //private static ILogger myLogger;
        //private static readonly object lockObject = new object();

        public Logger()
        {
            var filename = System.Configuration.ConfigurationManager.AppSettings["LoggerConfig"];
            var logConfig = new FileInfo(filename);
            log4net.Config.XmlConfigurator.Configure(configFile: logConfig);
            log4NetLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        //public static ILogger MyLogger
        //{
        //    get
        //    {
        //        if (myLogger == null)
        //        {
        //            lock (lockObject)
        //            {
        //                if (myLogger == null)
        //                    myLogger = new Logger();
        //            }
        //        }

        //        return myLogger;
        //    }
        //}

        public void LogError(string message, Exception ex)
        {
            log4NetLogger.Error(message, ex);
        }

        public void LogErrorMessage(string format, string message)
        {
            log4NetLogger.Error(String.Format(format, message));
        }

        public void LogInfoMessage(string format, string message)
        {
            log4NetLogger.Info(String.Format(format, message));
        }
    }
}
