using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chatter.Log;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.TCP
{
   public class ReceiveFileUtil
    {
        //private string path="c:\\save.txt";
        FileMessage fm;
        TcpListener myListener;
        TcpClient client;
        private int BufferSize=1024*1024;
        public ReceiveFileUtil(FileMessage fm)
        {
            this.fm=fm;
        }


        public int initTcpHost()
        {
            
            IPAddress ipa = IPAddress.Parse(fm.EndPoint.Address);

             myListener = new TcpListener(ipa, 0);

            
            myListener.Start();
            

            
            return Convert.ToInt32(myListener.LocalEndpoint.ToString().Split(':')[1]);
        }

        public void Receive()
        {
            try
            {
                new Thread(new ThreadStart(() =>
                {
                    client = myListener.AcceptTcpClient();
                    BeginReceive();
                })).Start();
               
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("监听端口出错",ex);
            }
        }

        private void BeginReceive()
        {

            NetworkStream ns = client.GetStream();


            
            FileStream fs = new FileStream(fm.Path, FileMode.Create, FileAccess.Write);

            long length = 0;
            byte[] array = new byte[BufferSize];
            while (length <fm.Size)
            {

                int n = ns.Read(array, 0, BufferSize);


                 fs.Write(array, 0, n);

              
                length += n;
            }

            fs.Close();
          


        }

       
    }
}
