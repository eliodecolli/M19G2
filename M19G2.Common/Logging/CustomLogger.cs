using log4net;
using log4net.Config;
using System;

namespace M19G2.Common.Logging
{
    public class CustomLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);

        public static void LogInfo(string message)
        {
            XmlConfigurator.Configure();
            Logger.Info(message);
        }

        public static void LogInfoFormat(string s, object[] args)
        {
            XmlConfigurator.Configure();
            Logger.Info(string.Format(s, args));
        }

        public static void LogError(string message, Exception e = null)
        {
            XmlConfigurator.Configure();
            if (e is null)
            {
                Logger.Error(message);
            }
            else
            {
                Logger.Error(message, e);
            }
        }

        public static void LogErrorFormat(string message, object[] args, Exception e = null)
        {
            XmlConfigurator.Configure();
            if (e is null)
            {
                Logger.Error(string.Format(message, args));
            }
            else
            {
                Logger.Error(string.Format(message, args), e);
            }
        }
    }
}
