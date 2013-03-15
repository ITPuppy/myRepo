﻿using System;
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
       


        public ReceiveFileUtil(FileMessage fm, UI.FileTransferGrid fileTransferGrid)
            : base(fm, fileTransferGrid)
        {
        }


        public void initTcpHost()
        {
            try
            {


                Socket socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                 localEP = new IPEndPoint(DataUtil.LocalIPAddress, 0);

                socket.Bind(localEP);
                socket.Connect(new IPEndPoint(IPAddress.Parse(fm.EndPoint.Address),fm.EndPoint.Port));

                byte[] buffer = Encoding.UTF8.GetBytes("2");

                socket.Send(buffer);

                localEP = socket.LocalEndPoint as IPEndPoint;
                myListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                myListener.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReuseAddress, true);


                myListener.Bind(localEP);
                myListener.Listen(1);

                MyLogger.Logger.Info("receive :"+myListener.LocalEndPoint.ToString());
               
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("初始化接收文件服务器时出现错误"+fm.EndPoint.Address,ex);
               
            }
        }

        public override void Transfer(MyEndPoint endPoint)
        {
            this.remoteEP = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);
            Receive();
        }

        private void Receive()
        {

            if (transferState != TransferState.Wating)
                return;
            try
            {

                new Thread(() =>
                {
                    int i = 0;
                    while (i++ < 2)
                    {
                        Socket punchingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        punchingSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        try
                        {
                            punchingSocket.Bind(localEP);
                       
                            punchingSocket.Connect(remoteEP);
                            break;
                        }
                        catch (Exception ex)
                        {
                            MyLogger.Logger.Info("tcp打洞", ex);
                        }
                    }
                   
                }).Start();
                 new Thread(new ThreadStart(() =>
                {

                    client = myListener.Accept();
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
            
            try
            {

                transferState = TransferState.Running;
               

                  

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

                                int n = client.Receive(array);

                                if (n == 0)
                                    throw new IOException();

                                fs.Write(array, 0, n);


                                length += n;

                                progress = (long)((double)length / fm.Size * 100);
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
