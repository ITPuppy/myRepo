using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    class ToAudioUtil : AudioUtil
    {
        private MyEndPoint iPEndPoint;
        private Socket socket = null;

        public ToAudioUtil(MyEndPoint iPEndPoint)
            : base(iPEndPoint)
        {

        }

       override public void InitWithServerHelp()
        {
           
            byte[] buffer = Encoding.UTF8.GetBytes("2");
            Send(buffer);

        }


    }
}
