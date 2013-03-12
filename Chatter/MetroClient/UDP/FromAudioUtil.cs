using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    class FromAudioUtil : AudioUtil
    {


        public FromAudioUtil(MyEndPoint iPEndPoint)
            : base(iPEndPoint)
        {

        }


        public override void InitWithServerHelp()
        {
          
            byte[] buffer = Encoding.UTF8.GetBytes("1");
            socket.SendTo(buffer, endPoint);

        }
    }
}
