using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Chatter.Log;
using Chatter.MetroClient.Audio;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UDP
{
    public abstract class AudioUtil
    {


        protected Socket socket = null;
        protected IPEndPoint endPoint;
        private bool isAlive = true;
        private SoundManager sm = null;
        private AudioForm af;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="endPoint">此处endPoint是服务器的，用来打洞</param>
        /// <param name="af"></param>
        public AudioUtil(MyEndPoint endPoint, AudioForm af)
        {

            this.af = af;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            this.endPoint = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);

        }
        /// <summary>
        /// 向服务器发送数据，用来打洞
        /// </summary>
        abstract public void InitWithServerHelp();


        /// <summary>
        /// 开始语音
        /// </summary>
        /// <param name="endPoint">语音对象endPoint</param>
        public void Start(MyEndPoint endPoint)
        {

            this.endPoint = new IPEndPoint(IPAddress.Parse(endPoint.Address), endPoint.Port);

            sm = new SoundManager(this);

            sm.StartRecordAndSend();
            Thread t = new Thread(() =>
              {
                  ///当Stop时，isAlive设置为false
                  while (isAlive)
                  {
                      try
                      {
                          byte[] data = new byte[500];
                          EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                          int n = socket.ReceiveFrom(data, ref remoteEP);
                          byte[] cmd = new byte[1];
                          cmd[0] = data[0];
                          string s = Encoding.UTF8.GetString(cmd);
                          switch (s)
                          {
                                  ///语音数据
                              case "D":
                                  {
                                      byte[] buffer = new byte[n - 1];
                                      Array.Copy(data, 1, buffer, 0, n - 1);
                                      sm.Play(buffer);
                                      break;
                                  }
                                  ///停止命令
                              case "S":
                                  {
                                      Stop();

                                      if (af != null)
                                      {
                                          af.Dispatcher.Invoke(new Action(() =>
                                          {
                                              af.Close();
                                          }));
                                      }


                                      break;
                                  }

                          }
                      }
                      catch (Exception ex)
                      {
                          MyLogger.Logger.Info("接收不到数据，对方可能关闭", ex);
                          if (af != null)
                          {
                              af.Dispatcher.Invoke(new Action(() =>
                                  {
                                      af.Close();
                                  }));
                          }
                     
                          break;
                      }
                  }
                  byte[] stopCmd = Encoding.UTF8.GetBytes("S");


                  this.Send(stopCmd);

              });
            t.IsBackground = true;
            t.Start();

        }


        /// <summary>
        /// 停止语音
        /// </summary>
        internal void Stop()
        {
            isAlive = false;
            if (af.timer.IsEnabled)
            {
                af.timer.Stop();
                af.timer.IsEnabled = false;
            }
            sm.Stop();
        }

        /// <summary>
        /// 向目的端发送数据
        /// </summary>
        /// <param name="buffer">要发送的数据</param>
        public void Send(byte[] buffer)
        {
            socket.SendTo(buffer, endPoint);
        }
        public void Dispose()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }
    }
}
