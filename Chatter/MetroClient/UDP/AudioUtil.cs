using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Chatter.MetroClient.Audio;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    public abstract class AudioUtil
    {


        protected Socket socket = null;
        protected IPEndPoint endPoint;



        public AudioUtil(MyEndPoint endPoint)
        {

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            this.endPoint = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);


        }
        abstract public void InitWithServerHelp();



        public void Start(MyEndPoint endPoint)
        {

            this.endPoint = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);

            SoundManager sm = new SoundManager(this);

            sm.StartRecordAndSend();
            new Thread(() => 
            {
                while (true)
                {
                    byte[] data = new byte[500];
                    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    int n = socket.ReceiveFrom(data, ref remoteEP);

                    byte[] buffer=new byte[n-1];
                    Array.Copy(data,1,buffer,0,n-1);
                    sm.Play(buffer);


                }
            }).Start();

        }



      




        public void Send(byte[] buffer)
        {
            socket.SendTo(buffer, endPoint);
        }
        public void Dispose()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }
    }
}
