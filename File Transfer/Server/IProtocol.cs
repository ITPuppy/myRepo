using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    interface IProtocol
    {
        void service(Socket socket);
    }
}
