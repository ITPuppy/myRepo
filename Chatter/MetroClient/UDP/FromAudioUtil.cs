using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    class FromAudioUtil : AudioUtil
    {


        public FromAudioUtil(MyEndPoint iPEndPoint,AudioForm af=null)
            : base(iPEndPoint,af)
        {

        }


        public override void InitWithServerHelp()
        {
          
            byte[] buffer = Encoding.UTF8.GetBytes("1");
            socket.SendTo(buffer, endPoint);

        }
    }
}
