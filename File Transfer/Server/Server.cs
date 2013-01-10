using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Server
    {
        private MultipleThread thread;
        Socket serverSocket;
        int Port = 0;
        IPAddress ip;
        static public Dictionary<int, Socket> dic;
        public static int n = 1000;
        public Server(MultipleThread thread)
        {

            Port = 9877;
            this.thread = thread;
            dic = new Dictionary<int, Socket>();
        }

        public void Start()
        {

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 9877));
            serverSocket.Listen(100);

            while (true)
            {
                Socket socket = serverSocket.Accept();
                Console.WriteLine(socket.RemoteEndPoint.ToString());

                thread.service(socket);
            }
        }

    }
}
