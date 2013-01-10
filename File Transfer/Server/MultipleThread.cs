using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace Server
{
    class MultipleThread:IProtocol
    {
        private IProtocol protocol;
        Socket socket;
        public MultipleThread(IProtocol protocol)
        {

            this.protocol = protocol; 

        }



        public void service(Socket socket)
        {

            new Thread(() => { protocol.service(socket); }).Start();
        }
    }
}
