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
      
       

        public SendFileUtil(FileMessage fm, UI.FileTransferGrid fileTransferGrid):
            base(fm,fileTransferGrid)
        {
        }


        public override void Transfer()
        {
            Send();
        }


        private void Send()
        {

            if (transferState != TransferState.Wating)
                return;
            new Thread(new ThreadStart(() =>
            {

                try
                {
                    initTCPClient();


                    BeginSend();
                    client.Close();
                   
                   
                    MyLogger.Logger.Info("发送文件线程退出");
                    return;
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("发送文件错误", ex);
                }

            })) {  Name="SendThread"}.Start();

          
        }

        private void BeginSend()
        {

            FileStream fs = null;
            NetworkStream ns = null;
            try
            {

                transferState = TransferState.Running;
                using (ns = client.GetStream())
                {
                    using (BufferedStream bs = new BufferedStream(ns))
                    {

                        using (fs = new FileStream(fm.Path, FileMode.Open, FileAccess.Read))
                        {

                            SetProgress();
                            long length = 0;
                            byte[] array = new byte[BufferSize];
                            while (length < fm.Size && transferState == TransferState.Running)
                            {
                                int n = fs.Read(array, 0, BufferSize);

                                bs.Write(array, 0, n);

                                length += n;

                                progress = (long)((double)length / fm.Size * 100);
                            }
                        }
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

            
               
        }

        private void initTCPClient()
        {
            client = new TcpClient();
            IPAddress address = IPAddress.Parse(fm.EndPoint.Address);

            client.Connect(address, fm.EndPoint.Port);

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
