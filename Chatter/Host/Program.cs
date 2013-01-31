using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chatter.Log;
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

            Console.WriteLine("***************输入\"exit\" 关闭程序***************");
            Console.WriteLine();
            foreach (ServiceHost host in hosts)
            {
                host.Opened += delegate
                {
                   ;
                   MyLogger.Logger.Info(host.Description.Name+"已经启动");
                };
                host.Closed += delegate
                {
                    
                    MyLogger.Logger.Info(host.Description.Name+"已经关闭");
                };
                host.Open();
                
            }
            bool isAlive=true;

            new Thread(new ThreadStart(() => {
                MyLogger.Logger.Info("开始发送心跳包");

                while (isAlive)
                {
                    Thread.Sleep(1000);
                    var hashTable=ChatterService.Online.Clone() as Hashtable;
                    foreach (DictionaryEntry pair in hashTable)
                    {
                        ChatterService service = (pair.Value as ChatterService.ChatEventHandler).Target as ChatterService;
                        service.SendHearBeat();
                    }
                }

                MyLogger.Logger.Info("停止发送心跳包");
            })).Start();

            string s=String.Empty;
            while(s.Trim().ToLower()!="exit")
            {
                s=Console.ReadLine();
            }

            isAlive=false;
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }
    }
}
