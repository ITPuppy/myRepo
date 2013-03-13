using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading;
using Chatter.Log;
using Chatter.Service;

namespace Chatter.Host
{
    class Program
    {
       static CustomPeerResolverService cprs;
        static void Main(string[] args)
        {
           
         
         

            List<ServiceHost> hosts = new List<ServiceHost>();
            ServiceHost host1 = new ServiceHost(typeof(RegisterService));

            ServiceHost host2 = new ServiceHost(typeof(ChatterService));
          
           
            cprs = new CustomPeerResolverService();
            cprs.RefreshInterval = TimeSpan.FromSeconds(5);

            cprs.ControlShape = true;
            cprs.Open();
          
            ServiceHost host3 = new ServiceHost(cprs);
            hosts.Add(host1);
            hosts.Add(host2);
            hosts.Add(host3);

            Console.WriteLine("***************输入\"exit\" 关闭程序***************");
            Console.WriteLine();
            foreach (ServiceHost host in hosts)
            {
                host.Opened += delegate
                {
                    ;
                    MyLogger.Logger.Info(host.Description.Name + "已经启动");
                };
                host.Closed += delegate
                {

                    MyLogger.Logger.Info(host.Description.Name + "已经关闭");
                };
                host.Open();

            }
          

            
            string s = String.Empty;
            while (true)
            {

                s = Console.ReadLine();

                if (s.Trim().ToLower() == "exit")
                {
                    break;
                }
                else if (s.Trim().ToLower() == "clear")
                {
                    Console.Clear();
                }

            }

           ChatterService.isAlive = false;
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }
    }
}
