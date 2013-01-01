using Chatter.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Fatal("Fatal");
            Logger.Error("Error");
            Logger.Warn("Warn");
            Logger.Info("Info");
            Logger.Debug("Debug");

            
        }
    }
}
