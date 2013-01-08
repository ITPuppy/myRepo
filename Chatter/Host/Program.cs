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
            List<ServiceHost> hosts = new List<ServiceHost>();
            ServiceHost host1 = new ServiceHost(typeof(RegisterService));
            ServiceHost host2 = new ServiceHost(typeof(ChatterService));
            hosts.Add(host1);
            hosts.Add(host2);
            foreach (ServiceHost host in hosts)
            {
                host.Opened += delegate
                {
                    Console.WriteLine(host.ToString()+"Service Start");
                };
                host.Closed += delegate
                {
                    Console.WriteLine(host.ToString()+"Service Stopped");
                };
                host.Open();
                
            }

            Console.ReadLine();
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }
    }
}
