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
        FromAudioUtil fau = null;
        ToAudioUtil tau = null;
        public AudioForm()
        {
            InitializeComponent();
            image.Source = new BitmapImage(new Uri(imageSouce, UriKind.Relative));
            image.Stretch = Stretch.Fill;
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

            if (isFrom)
            {
                btnAccept.Visibility = Visibility.Collapsed;
                grid.ColumnDefinitions.RemoveAt(4);
                this.Width = 240;

                nickName.Text = to.nickName;
                DataUtil.AudioForms.Add(to.id, this);
            }
            else
            {
                nickName.Text = from.nickName;
                DataUtil.AudioForms.Add(from.id, this);
            }
           
        }

        

        private void Move(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }



        private void btnAccept_Click(object sender, MouseButtonEventArgs e)
        {
            btnAccept.Visibility = Visibility.Collapsed;
            grid.ColumnDefinitions.RemoveAt(4);
            this.Width = 240;

            InitReceive();


                DataUtil.Client.ResponseToRequest(new Result()
                {
                    Member = am.from as Member,
                    Type = MessageType.Audio,
                    EndPoint = am.ServerEndPoint,
                    Status = MessageStatus.Accept

                });
         
        }

        


        private void btnStop_Click(object sender, MouseButtonEventArgs e)
        {

            Close();
        }

       public  void Close()
        {

            if (!isFrom)
            {
                ///拒绝语音请求
                if (flag)
                {
                    new Thread(() =>
                    {
                        DataUtil.Client.ResponseToRequest(new Result()
                        {
                            Member = am.from as Member,
                            Type = MessageType.Audio,
                            EndPoint = am.ServerEndPoint,
                            Status = MessageStatus.Refuse

                        });
                    }).Start();
                }

                ///挂断语音
                else
                {


                }

                DataUtil.AudioForms.Remove((am.from as Member).id);

            }

            else
            {
                DataUtil.AudioForms.Remove((am.to as Member).id);
            }
            base.Close();
        }




       internal void Start(MyEndPoint endPoint)
       {
           if (isFrom)
           {
              
               fau.Start(endPoint);
           }
           else
           { 
              
               tau.Start(endPoint);
           }
       }

       internal void InitSend(MyEndPoint myEndPoint)
       {
            fau = new FromAudioUtil(myEndPoint);
           fau.InitWithServerHelp();
       }
       private void InitReceive()
       {
            tau = new ToAudioUtil(am.ServerEndPoint);
           tau.InitWithServerHelp();
       }
    }
}
