using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    class ReceiveAudioUtil
    {
        private MyEndPoint iPEndPoint;
        private Socket socket = null;

        public ReceiveAudioUtil(MyEndPoint iPEndPoint)
        {
            // TODO: Complete member initialization
            this.iPEndPoint = iPEndPoint;
        }

        public void Init()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte []buffer=Encoding.UTF8.GetBytes("2");
            socket.SendTo(buffer,new IPEndPoint(IPAddress.Parse(iPEndPoint.Address),iPEndPoint.Port));

        }
    }
}
