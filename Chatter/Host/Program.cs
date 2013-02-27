using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

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
                    MyLogger.Logger.Info(host.Description.Name + "已经启动");
                };
                host.Closed += delegate
                {

                    MyLogger.Logger.Info(host.Description.Name + "已经关闭");
                };
                host.Open();

            }
            bool isAlive = true;

            new Thread(new ThreadStart(() =>
            {
                MyLogger.Logger.Info("开始接收心跳包");

                while (isAlive)
                {
                    Thread.Sleep(1000);
                    var hashTable = ChatterService.lastUpdateTable.Clone() as Hashtable;
                    foreach (DictionaryEntry pair in hashTable)
                    {
                        if (new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(Convert.ToDateTime(pair.Value).Ticks)).Seconds > 3)
                        {

                            var handler = ChatterService.Online[pair.Key] as ChatterService.ChatEventHandler;
                            if (handler != null)
                            {
                                ChatterService service = handler.Target as ChatterService;
                                if (service != null)
                                {

                                    service.Dispose();

                                }
                            }


                            
                        }


                    }
                }

                MyLogger.Logger.Info("停止接收心跳包");
            })).Start();

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

            isAlive = false;
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }
    }
}
