using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.Windows;
using System.Windows.Threading;
using Chatter.Log;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.TCP
{
    public class ReceiveFileUtil : TransferFileUtil
    {
        private TcpListener myListener;


        public ReceiveFileUtil(FileMessage fm, UI.FileTransferGrid fileTransferGrid)
            : base(fm, fileTransferGrid)
        {
        }


        public int initTcpHost()
        {

            IPAddress ipa = IPAddress.Parse(fm.EndPoint.Address);

            myListener = new TcpListener(ipa, 0);


            myListener.Start();



            return Convert.ToInt32(myListener.LocalEndpoint.ToString().Split(':')[1]);
        }

        public override void Transfer()
        {
            Receive();
        }

        private void Receive()
        {

            if (transferState != TransferState.Wating)
                return;
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
                MyLogger.Logger.Error("监听端口出错", ex);
            }
        }

        private void BeginReceive()
        {
            FileStream fs = null;
            NetworkStream ns = null;
            try
            {

                transferState = TransferState.Running;
                ns = client.GetStream();


                fs = new FileStream(fm.Path, FileMode.Create, FileAccess.Write);

                long length = 0;
                byte[] array = new byte[BufferSize];
                SetProgress();
                while (length < fm.Size && transferState==TransferState.Running)
                {

                    int n = ns.Read(array, 0, BufferSize);

                    if (n == 0)
                        throw new IOException();

                    fs.Write(array, 0, n);


                    length += n;

                    progress = (long)((double)length / fm.Size * 100);
                }


            }

            catch (IOException ex)
            {
                transferState = TransferState.CanceledByTheOther;
                MyLogger.Logger.Info("对方取消发送", ex);
            }
            catch (Exception ex)
            {
                transferState = TransferState.InternetError;
                MyLogger.Logger.Info("网络出现问题", ex);
            }

            finally
            {
                if (fs != null)
                {

                    fs.Close();

                }
                if (ns != null)
                {
                    ns.Close();
                }
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
            fileTransferGrid.bar.Dispatcher.Invoke(new Action(()=>
            {
                fileTransferGrid.CompletReceive(transferState);

            }));
        }


    }
}
