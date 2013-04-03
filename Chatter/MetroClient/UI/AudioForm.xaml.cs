using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Chatter.MetroClient.UDP;
using MetroClient.ChatterService;


namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for AudioForm.xaml
    /// </summary>
    public partial class AudioForm : Window
    {
        private string imageSouce = "/MetroClient;component/res/img/default.png";
   
        private AudioMessage am;
        bool flag = true;
        private bool isFrom;
        AudioUtil fau = null;
        AudioUtil tau = null;
        private bool isStart=false;
        private BaseRole role;
        private int timerCount = 0;
        public DispatcherTimer timer = new DispatcherTimer();
        public AudioForm()
        {
            InitializeComponent();
            image.Source = new BitmapImage(new Uri(imageSouce, UriKind.Relative));
            image.Stretch = Stretch.Fill;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timerCount++;
            int min = timerCount / 60;
            int sec = timerCount % 60;
            string s = String.Format("{0:##}:{1:##}",min,sec);
            timerTxt.Text = s;
        }

        public AudioForm(AudioMessage am,bool isFrom)
        {
            InitializeComponent();
           
            image.Source = new BitmapImage(new Uri(imageSouce, UriKind.Relative));
            image.Stretch = Stretch.Fill;
            this.am = am;
            this.isFrom = isFrom;

            Member from=am.from as Member;
            Member to =am.to as Member;
            this.Closed += AudioForm_Closed;
            ///语音发起者
            if (isFrom)
            {
                btnAccept.Visibility = Visibility.Collapsed;
                grid.ColumnDefinitions.RemoveAt(4);
                this.Width = 240;
                this.role = am.to;
                nickName.Text = to.nickName;
                DataUtil.AudioForms.Add(to.id, this);
            }
            ///语音接受者
            else
            {
                this.role = am.from;
                nickName.Text = from.nickName;
                DataUtil.AudioForms.Add(from.id, this);
            }
           
        }
        /// <summary>
        /// 语音窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AudioForm_Closed(object sender, EventArgs e)
        {
            if (!isFrom)
            {
              

                if (DataUtil.AudioForms.ContainsKey((am.from as Member).id))
                    DataUtil.AudioForms.Remove((am.from as Member).id);
            }
            else
            {
               
                if (DataUtil.AudioForms.ContainsKey((am.to as Member).id))
                    DataUtil.AudioForms.Remove((am.to as Member).id);
              
            }
        }

        

        private void Move(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }


        /// <summary>
        /// 语音接受者接受语音请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccept_Click(object sender, MouseButtonEventArgs e)
        {
            btnAccept.Visibility = Visibility.Collapsed;
            grid.ColumnDefinitions.RemoveAt(4);
            this.Width = 240;

            flag = false;
            InitReceive();


                DataUtil.Client.ResponseToRequest(new Result()
                {
                    Member = am.from as Member,
                    Type = MessageType.Audio,
                    EndPoint = am.ServerEndPoint,
                    Status = MessageStatus.Accept

                });
         
        }

        

        /// <summary>
        /// 关闭按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, MouseButtonEventArgs e)
        {

            Shut();
        }

       
       public  void Shut()
        {
           ///语音接受者
            if (!isFrom)
            {
                ///还没接受，拒绝语音请求
                if (flag)
                {
                   Thread t= new Thread(() =>
                    {
                        DataUtil.Client.ResponseToRequest(new Result()
                        {
                            Member = am.from as Member,
                            Type = MessageType.Audio,
                            EndPoint = am.ServerEndPoint,
                            Status = MessageStatus.Refuse

                        });
                    });
                   t.IsBackground = true;
                   t.Start();
                }

                ///已经接受，挂断语音
                else
                {

                    tau.Stop();
                }
              

            }
            ///语音发起者
            else
            {
                ///已经开始语音，取消语音
                if (isStart)
                {
                    fau.Stop();
                }
                ///还没开始语音，发送取消指令
                else
                {
                    CommandMessage cm = new CommandMessage()
                    {
                        CommandType = MyCommandType.CanceledAudioRequest,
                        from = DataUtil.Member,
                        to = role,
                        type = MessageType.Command
                    };
                    DataUtil.Client.SendMesg(cm);
                }
               
            }
            Dispatcher.Invoke(new Action(() =>
            {

                this.Close();
            }));
        }




       internal void Start(MyEndPoint endPoint)
       {
           isStart = true;
           timer.Start();
           if (isFrom)
           {
              
               fau.Start(endPoint);
           }
           else
           { 
              
               tau.Start(endPoint);
           }
       }
        /// <summary>
        /// 向服务器发送'1'，以便udp打洞
        /// </summary>
        /// <param name="myEndPoint"></param>
       internal void InitSend(MyEndPoint myEndPoint)
       {
            fau = new FromAudioUtil(myEndPoint,this);
           fau.InitWithServerHelp();
       }
        /// <summary>
        /// 向服务器发送'2'，以供udp打洞使用
        /// </summary>
       private void InitReceive()
       {
            tau = new ToAudioUtil(am.ServerEndPoint,this);
           tau.InitWithServerHelp();
       }
        /// <summary>
        /// 语音发起者取消语音请求
        /// </summary>
       internal void TheOtherCanceled()
       {
           if (tau != null)
               tau.Stop();
           else
           {
               if (DataUtil.AudioForms.ContainsKey((am.from as Member).id))
                   DataUtil.AudioForms.Remove((am.from as Member).id);
               this.Close();
           }
       }
    }
}
