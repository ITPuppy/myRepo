using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Chatter.Service;
namespace Chatter.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host=new ServiceHost(typeof(RegisterService));
            host.Opened += delegate
            {
                Console.WriteLine("Service Start");
            };
            host.Closed += delegate
            {
                Console.WriteLine("Service Stopped");
            };
            host.Open();
            Console.ReadLine();
            host.Close();
        }
    }
}
