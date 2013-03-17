using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Chatter.Log;
using MetroClient.ChatterService;


namespace Chatter.MetroClient.TCP
{
    public class SendFileUtil:TransferFileUtil
    {

        Thread t;

        public SendFileUtil(FileMessage fm, UI.FileTransferGrid fileTransferGrid):
            base(fm,fileTransferGrid)
        {
            initTCPClient();
        }

        private void initTCPClient()
        {

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            localEP = new IPEndPoint(DataUtil.LocalIPAddress, 0);

            socket.Bind(localEP);
            socket.Connect(new IPEndPoint(IPAddress.Parse(fm.EndPoint.Address), fm.EndPoint.Port));

            byte[] buffer = Encoding.UTF8.GetBytes("1");

            socket.Send(buffer);


            localEP = socket.LocalEndPoint as IPEndPoint;
            socket.Close();
            myListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            myListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);


            myListener.Bind(localEP);
            myListener.Listen(1);
            t = new Thread(() =>
             {
                 try
                 {
                     MyLogger.Logger.Info("我是发送端，开始等待接收端的connect");
                     client = myListener.Accept();
                     myListener.Close();
                     MyLogger.Logger.Info("我是发送端，accept接收端的connect");
                 }
                 catch (Exception)
                 {
                     MyLogger.Logger.Info("发送端与接收端连接成功，停止accept");
                 }
             }) { IsBackground = true };
            t.Start();
            MyLogger.Logger.Info("send :" + myListener.LocalEndPoint.ToString());

        }
        public override void Transfer(MyEndPoint endPoint)
        {
            this.remoteEP = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);
            Send();
        }


        private void Send()
        {

            if (transferState != TransferState.Wating)
                return;

            new Thread(() =>
            {
                int i = 0;
                while (i++ < 10)
                {
                    Socket punchingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    punchingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    try
                    {
                        if (client != null)
                            break;
                        punchingSocket.Bind(localEP);

                        punchingSocket.Connect(remoteEP);

                        t.Abort();


                        if (client == null)
                            client = punchingSocket;
                        MyLogger.Logger.Info("发送端与接收端连接成功");
                        t.Join();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        MyLogger.Logger.Info("tcp打洞", ex);

                    }
                }

            }) { IsBackground=true}.Start();
            new Thread(new ThreadStart(() =>


            {

                try
                {
                 

                  
                   while (client == null) ;
                    BeginSend();
                    client.Close();
                   
                   
                    MyLogger.Logger.Info("发送文件线程退出");
                    return;
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("发送文件错误", ex);
                }

            })) {  Name="SendThread",IsBackground=true}.Start();

          
        }

        private void BeginSend()
        {

            FileStream fs = null;

            try
            {

                transferState = TransferState.Running;



                using (fs = new FileStream(fm.Path, FileMode.Open, FileAccess.Read))
                {

                    SetProgress();
                    long length = 0;
                    byte[] array = new byte[BufferSize];
                    while (length < fm.Size && transferState == TransferState.Running)
                    {
                        int n = fs.Read(array, 0, BufferSize);

                        client.Send(array, 0, n, SocketFlags.None);

                        length += n;

                        progress = (long)((double)length / fm.Size * 100);
                    }
                }





            }
            catch (IOException ex)
            {
                transferState = TransferState.CanceledByTheOther;
                MyLogger.Logger.Info("对方取消接收", ex);
            }
            catch (Exception ex)
            {
                transferState = TransferState.InternetError;
                MyLogger.Logger.Info("网络出现问题", ex);
            }
            finally
            {
                client.Close();
            }
            
               
        }

     


        private void SetProgress()
        {



            new Thread(new ThreadStart(() =>
            {

                while (progress < 100 && transferState==TransferState.Running)
                {

                    
                    fileTransferGrid.bar.Dispatcher.Invoke(new Action(() =>
                    {
                        fileTransferGrid.bar.Value = progress;

                    }));


                    Thread.Sleep(100);
                }


                Completed();

                return;
            })



                 ).Start();

        }

        override public void Completed()
        {
            fileTransferGrid.bar.Dispatcher.Invoke(new Action(() =>
            {
                fileTransferGrid.CompletSend(transferState);

            }));
        }
    }
}
