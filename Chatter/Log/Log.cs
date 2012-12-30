using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Chatter.Log
{
    public class Logger
    {

        private static ILog log;
        public Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger("");
        }

        public static void Fatal(string mesg)
        {
            if (log == null)
            {
                log = log4net.LogManager.GetLogger("");
            }
            log.Fatal(mesg);
        }

        public static void Debug(string mesg)
        {
            if (log == null)
            {
                log = log4net.LogManager.GetLogger("");
            }
            log.Debug(mesg);
        }

        public static void Info(string mesg)
        {
            if (log == null)
            {
                log = log4net.LogManager.GetLogger("");
            }
            log.Info(mesg);
        }
        public static void Error(string mesg)
        {
            if (log == null)
            {
                log = log4net.LogManager.GetLogger("");
            }
            log.Error(mesg);
        }
        public static void Warn(string mesg)
        {
            if (log == null)
            {
                log = log4net.LogManager.GetLogger("");
            }
            log.Warn(mesg);
        }
    }
}
