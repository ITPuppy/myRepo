using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chatter.Log;
using Chatter.Service;
using log4net.Appender;  
using log4net.Filter;  
using log4net.Util;  
using log4net.Layout;  
using log4net.Core;  
using System.IO;

namespace Chatter.UIHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<ServiceHost> hosts = new List<ServiceHost>();
        bool isAlive = true;
       
        
        public MainWindow()
        {
          
            InitializeComponent();
           
            init();
        }

        private void init()
        {
            btnStop.IsEnabled = false;


            new Thread(
                () =>
                {
                    while (isAlive)
                    {


                        FileStream fs = new FileStream(@"Log\log_2013_01_31.log",FileMode.Open,FileAccess.Read);
                        TextReader reader = new StreamReader(fs);
                        
                        string s=reader.ReadToEnd();

                        Dispatcher.Invoke(new Action(()=>{txtLog.Text=s;}));

                        reader.Close();
                        fs.Close();

                        Thread.Sleep(1000);

                    }
                }
                ).Start();

           
        }

    

       

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
             
            txtLog.Text = "abc";
            isAlive = false;
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }

            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            hosts.Clear();
            ServiceHost host1 = new ServiceHost(typeof(RegisterService));

            ServiceHost host2 = new ServiceHost(typeof(ChatterService));
            hosts.Add(host1);
            hosts.Add(host2);
          
            foreach (ServiceHost host in hosts)
            {
                host.Opened += delegate
                {
                    MyLogger.Logger.Info(host.ToString() + "Service Start");
                };
                host.Closed += delegate
                {
                    MyLogger.Logger.Info(host.ToString() + "Service Stopped");
                };
                host.Open();
                btnStart.IsEnabled = false;
            }
            

            new Thread(new ThreadStart(() =>
            {
                MyLogger.Logger.Info("开始发送心跳包");
                while (isAlive)
                {
                    Thread.Sleep(1000);
                    var hashTable = ChatterService.Online.Clone() as Hashtable;
                    foreach (DictionaryEntry pair in hashTable)
                    {
                        ChatterService service = (pair.Value as ChatterService.ChatEventHandler).Target as ChatterService;
                        service.SendHearBeat();
                    }
                }

                MyLogger.Logger.Info("停止发送心跳包");
            })).Start();

            btnStop.IsEnabled = true;
        }


      

        
    }
}
