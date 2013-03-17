using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Chatter.Contract.DataContract;
using Chatter.Log;

namespace Chatter.Service
{
    class TCPHolePunching
    {
        private ChatterService from;
        private ChatterService to;
        private IPAddress ipa;
        private IPEndPoint serverEndPoint = null;
        private string guid;
        public TCPHolePunching(ChatterService from,ChatterService to,string guid)
        {
            this.from = from;
            this.to = to;
            this.guid = guid;
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    ipa = ip;
                    break;
                }
            }

            tcpServer.Bind(new IPEndPoint(ipa,0));
            serverEndPoint= tcpServer.LocalEndPoint as IPEndPoint;

            tcpServer.Listen(1);
            new Thread(() =>
            {

                bool isFromBack = false;
                bool isToBack = false;
                EndPoint fromRemoteEP = null;
                EndPoint toRemoteEP = null;
                while (true)
                {
                    var client = tcpServer.Accept();

                    byte[] buffer = new byte[1];
                    client.Receive(buffer);
                    var remoteEP = client.RemoteEndPoint as IPEndPoint;
                    string s=Encoding.UTF8.GetString(buffer);
                    if (s == "1")
                    {
                        if (isFromBack)
                            continue;
                        isFromBack = true;
                        fromRemoteEP = remoteEP;
                       
                    }
                    else if (s == "2")
                    {
                        if (isToBack)
                            continue;
                        isToBack = true;
                       
                        toRemoteEP = remoteEP;
                       
                    }

                    if (isFromBack && isToBack)
                    {
                       

                        this.to.callback.SendTCPEndPoint(new MyEndPoint()
                        {
                            Address = (fromRemoteEP as IPEndPoint).Address.ToString(),
                            Port = (fromRemoteEP as IPEndPoint).Port
                        }, guid);

                        this.from.callback.SendTCPEndPoint(new MyEndPoint()
                        {
                            Address = (toRemoteEP as IPEndPoint).Address.ToString(),
                            Port = (toRemoteEP as IPEndPoint).Port
                        }, guid);

                        MyLogger.Logger.Info("from:" + fromRemoteEP.ToString());
                        MyLogger.Logger.Info("to:" + toRemoteEP.ToString());

                        break;
                    }
                    
                }
                Console.WriteLine("tcp打洞");

            }) { IsBackground=true}.Start();


            
        }

        public IPEndPoint GetServerEndPoint()
        {
            return serverEndPoint;
        }
    }
}
