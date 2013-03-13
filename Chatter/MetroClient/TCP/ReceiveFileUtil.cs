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
using Chatter.MetroClient.Sound;
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
            try
            {
              
              

                myListener = new TcpListener(DataUtil.LocalIPAddress, 0);


                myListener.Start();



                return Convert.ToInt32(myListener.LocalEndpoint.ToString().Split(':')[1]);
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("初始化接收文件服务器时出现错误"+fm.EndPoint.Address,ex);
                return -1;
            }
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
                    SoundPlayer.Play();
                    
                    MyLogger.Logger.Info("接收文件线程退出");
                    return;
                })) {  Name="ReceiveThread", IsBackground=true}.Start();

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
                using (ns = client.GetStream())
                {

                    using (BufferedStream bs = new BufferedStream(ns))
                    {

                        using (File.Create(fm.Path))
                        {
                            File.SetAttributes(fm.Path, FileAttributes.Hidden);

                        }


                        using (fs = new FileStream(fm.Path, FileMode.Open, FileAccess.Write))
                        {

                            long length = 0;
                            byte[] array = new byte[BufferSize];
                            SetProgress();
                            while (length < fm.Size && transferState == TransferState.Running)
                            {

                                int n = bs.Read(array, 0, BufferSize);

                                if (n == 0)
                                    throw new IOException();

                                fs.Write(array, 0, n);


                                length += n;

                                progress = (long)((double)length / fm.Size * 100);
                            }


                        }
                    }
                }


             
                if (transferState != TransferState.Running)
                {
                    if (File.Exists(fm.Path))
                    {
                        File.Delete(fm.Path);
                    }
                }
                else
                {
                    try
                    {
                        File.Move(fm.Path, fm.Path.Replace(fm.Guid, fm.FileName));
                        File.SetAttributes(fm.Path.Replace(fm.Guid, fm.FileName), FileAttributes.Normal);
                    }
                    catch (Exception ex)
                    {
                        MyLogger.Logger.Info("文件已经存在", ex);
                        Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                        {
                            MessageBox.Show("同名文件已经存在");
                        }));
                    }
                }
            }

            catch (IOException ex)
            {

                if (fs != null)
                {

                    fs.Close();

                }
                if (ns != null)
                {
                    ns.Close();
                }
                transferState = TransferState.CanceledByTheOther;
                MyLogger.Logger.Info("对方取消发送", ex);
                if (File.Exists(fm.Path))
                {
                    File.Delete(fm.Path);
                }
            }
            catch (Exception ex)
            {

                if (fs != null)
                {

                    fs.Close();

                }
                if (ns != null)
                {
                    ns.Close();
                }
                transferState = TransferState.InternetError;
                MyLogger.Logger.Info("网络出现问题", ex);
                if (File.Exists(fm.Path))
                {
                    File.Delete(fm.Path);
                }
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
