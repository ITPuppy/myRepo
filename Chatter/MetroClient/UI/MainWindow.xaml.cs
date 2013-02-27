
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chatter.Log;
using MetroClient.ChatterService;
using System.Threading;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color selectedColor = Color.FromArgb(255, 114, 119, 123);
        Grid selectedGrid;



        private MyTabControl tabControl;
        private bool isAlive=true;
        private Thread sendHearBeatThread;
        public MainWindow()
        {
            InitializeComponent();

        }



        private void MainWindow_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {

                base.DragMove();

            }
        }



        private void SelectMode_Click(object sender, MouseButtonEventArgs e)
        {

            Grid grid = sender as Grid;
            if (grid != selectedGrid)
                selectedGrid.Background = null;
            selectedGrid = grid;
            grid.Background = new SolidColorBrush(selectedColor);

            if (grid.Name == "btnFriendGrid")
                tabControl.SelectedIndex = 0;
            else if (grid.Name == "btnGroupGrid")
                tabControl.SelectedIndex = 1;
            else if (grid.Name == "btnRecentFriendGrid")
                tabControl.SelectedIndex = 2;
            else if (grid.Name == "btnSetting")
            {
                tabControl.SelectedIndex = 3;
            }

        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


            init();


        }

        public void init()
        {

           
                    try
                    {

                        txtNickName.Text = DataUtil.Member.nickName;
                        DataUtil.MessageTabControl = this.mesgTabControl;

                        selectedGrid = btnFriendGrid;
                        selectedGrid.Background = new SolidColorBrush(selectedColor);
                        DataUtil.Client.GetFriendsCompleted += Client_GetFriendsCompleted;
                        DataUtil.Client.GetFriendsAsync(DataUtil.Member.id);


                        SendHeartBeat();
                       
                    }
                    catch (Exception ex)
                    {
                        MyLogger.Logger.Error("初始化界面出错", ex);
                    }
               
           



        }

        private void SendHeartBeat()
        {
            sendHearBeatThread = new Thread(new ThreadStart(() =>
             {

                 while (isAlive)
                 {
                     Thread.Sleep(1000);
                     try
                     {
                         DataUtil.Client.SendHeartBeat();
                     }
                     catch (Exception ex)
                     {
                         if (isAlive == false)
                             return;
                         else
                             MyLogger.Logger.Warn("发送心跳包出现错误", ex);
                     }
                 }
             }));
            sendHearBeatThread.Start();
        }

        void Client_GetFriendsCompleted(object sender, GetFriendsCompletedEventArgs e)
        {
            DataUtil.UserGroups = DataUtil.Client.GetFriends(DataUtil.Member.id).ToList<UserGroup>();
            tabControl = new MyTabControl();
            Grid.SetRow(tabControl, 1);
            MiddleGrid.Children.Add(tabControl);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                isAlive = false;
               
                DataUtil.Client.Abort();
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("退出出现问题", ex);
            }
            finally
            {

                Application.Current.Shutdown();
                base.OnClosing(e);
            }
        }




    }
}
 