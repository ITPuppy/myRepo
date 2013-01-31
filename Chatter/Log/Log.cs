
using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace Chatter.Log
{
    public class MyLogger
    {

        private static ILog log;

        public static ILog Logger
        {
            get
            {
                if (log == null)
                {
                    
                    log = GetLogger();

                }
                return log;
            }

        }
        static private ILog GetLogger()
        {
            FileInfo fi = new FileInfo("../../../App.config");
            log4net.Config.XmlConfigurator.Configure(fi);
            log = log4net.LogManager.GetLogger("");

            return log;
        }


    }
    
}
