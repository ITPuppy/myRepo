using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            IProtocol protocol = new Protocol();
            MultipleThread thread = new MultipleThread(protocol);
            Server server = new Server(thread);
            server.Start();
        }
    }
}
