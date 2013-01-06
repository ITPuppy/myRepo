using log4net;
using System;
using System.IO;

namespace Chatter.Log
{
    public class Logger
    {

        private static ILog log;
        static  private ILog GetLogger()
        {
            FileInfo fi = new FileInfo("../../../App.config");
            log4net.Config.XmlConfigurator.Configure(fi);
            log = log4net.LogManager.GetLogger("");
            return log;
        }

        public static void Fatal(string mesg)
        {
            if (log == null)
            {
                log = GetLogger();
            }
            log.Fatal(mesg);
        }

        public static void Debug(string mesg)
        {
            if (log == null)
            {
                log = GetLogger();
            }
            log.Debug(mesg);
        }

        public static void Info(string mesg)
        {
            if (log == null)
            {
                log = GetLogger();
            }
            log.Info(mesg);
        }
        public static void Error(string mesg)
        {
            if (log == null)
            {
                log = GetLogger();
            }
            log.Error(mesg);
        }
        public static void Warn(string mesg)
        {
            if (log == null)
            {
                log = GetLogger();
            }
            log.Warn(mesg);
        }
    }
}
