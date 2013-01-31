using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace Chatter.UIHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAppender
    {
        public MainWindow()
        {
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetLoggerRepository()).Root.AddAppender(this);  
            InitializeComponent();
            btnStop.IsEnabled = false;
        }
        List<ServiceHost> hosts = new List<ServiceHost>();
        bool isAlive = true;

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
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
           
            ServiceHost host1 = new ServiceHost(typeof(RegisterService));

            ServiceHost host2 = new ServiceHost(typeof(ChatterService));
            hosts.Add(host1);
            hosts.Add(host2);
            foreach (ServiceHost host in hosts)
            {
                host.Opened += delegate
                {
                    Console.WriteLine(host.ToString() + "Service Start");
                };
                host.Closed += delegate
                {
                    Console.WriteLine(host.ToString() + "Service Stopped");
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


        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {
            txtLog.Text = txtLog.Text + String.Format("log4net - {0}: {1}", loggingEvent.Level.Name, loggingEvent.MessageObject.ToString());

        }
    }
}
