using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Chatter.Contract.DataContract;
using Chatter.Log;
using Chatter.Service;

namespace Service
{
    class UDPHolePunching
    {
        private ChatterService from;
        private IPAddress ipa;
        private IPEndPoint serverEndPoint = null;
        private ChatterService to;

        public UDPHolePunching(ChatterService from,ChatterService to)
        {
            // TODO: Complete member initialization
            this.from = from;
            this.to = to;



            Socket udpserver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress[] arrIPAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in arrIPAddresses)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    ipa = ip;
                    break;
                }
            }
            udpserver.Bind(new IPEndPoint(ipa, 0));
            serverEndPoint = udpserver.LocalEndPoint as IPEndPoint;


            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            new Thread(() =>
            {
                try
                {
                    byte[] buffer = new byte[1];
                    bool isFromBack = false;
                    bool isToBack = false;

                    while (true)
                    {
                        udpserver.ReceiveFrom(buffer, ref remoteEP);
                        string s = Encoding.UTF8.GetString(buffer);
                        
                        if(s=="1")
                        {
                            if (isFromBack)
                                continue;

                            isFromBack = true;
                            this.to.callback.SendMyEndPoint(new MyEndPoint()
                            {
                                Address = (remoteEP as IPEndPoint).Address.ToString(),
                                Port = (remoteEP as IPEndPoint).Port
                            },from.member );
                        }
                        else if (s == "2")
                        {
                            if (isToBack)
                                continue;
                            isToBack = true;
                            this.from.callback.SendMyEndPoint(new MyEndPoint()
                            {
                                Address = (remoteEP as IPEndPoint).Address.ToString(),
                                Port = (remoteEP as IPEndPoint).Port
                            },to.member);
                        }

                        if (isFromBack && isToBack)
                            break;
                    }
                    Console.WriteLine("打洞去吧");
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("接收udp包出错", ex);
                }
            }).Start();



        }

        public IPEndPoint GetServerEndPoint()
        {
            return serverEndPoint;
        }

    }
}
