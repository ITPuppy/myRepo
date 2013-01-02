using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Channels;

namespace WCFChatService
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding bind = new NetTcpBinding();
            Uri uri = new Uri(ConfigurationManager.AppSettings["conAddress"]);//从配置文件中读取服务的Url
            ServiceHost host = new ServiceHost(typeof(WCFChatService.ChatService), uri);
            //if中的代码可以没有，但是如果想利用Svctuil.exe生成代理类的时候，就需要下面的代码，否则将会报错，无法解析元数据
            if (host.Description.Behaviors.Find<System.ServiceModel.Description.ServiceMetadataBehavior>() == null)
            {
                BindingElement metaElement = new TcpTransportBindingElement();
                CustomBinding metaBind = new CustomBinding(metaElement);
                host.Description.Behaviors.Add(new System.ServiceModel.Description.ServiceMetadataBehavior());
                host.AddServiceEndpoint(typeof(System.ServiceModel.Description.IMetadataExchange), metaBind, "MEX");
            }
            host.Open();        
            Console.WriteLine("聊天室服务器开始监听: endpoint {0}", uri.ToString());
            Console.WriteLine("按 ENTER 停止服务器监听...");
            Console.ReadLine();
            host.Abort();
            host.Close(); 
        }
    }
}     