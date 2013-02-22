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
    public class SendFileUtil
    {
        private FileMessage fm;
        private TcpClient client;
        private int BufferSize=1024*1024;
        public SendFileUtil(FileMessage fm)
        {

            this.fm = fm;
        }



        public void Send()
        {
            new Thread(new ThreadStart(() =>
            {

                try
                {
                    initTCPClient();


                    BeginSend();
                    client.Close();
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("发送文件错误",ex);
                }

            })).Start();

          
        }

        private void BeginSend()
        {

            NetworkStream ns= client.GetStream();

            FileStream fs = new FileStream(fm.Path,FileMode.Open,FileAccess.Read);

            long length=0;
            byte []array=new byte[BufferSize];
            while (length<fm.Size)
            {
               int n= fs.Read(array,0,BufferSize);

               ns.Write(array, 0, n);
               MyLogger.Logger.Info("发送文件"+n);
               length += n;
            }

            fs.Close();
          



        }

        private void initTCPClient()
        {
            client = new TcpClient();
            IPAddress address = IPAddress.Parse(fm.EndPoint.Address);

            client.Connect(address, fm.EndPoint.Port);

        }
    }
}
