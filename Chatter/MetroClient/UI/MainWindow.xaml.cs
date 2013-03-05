
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
                tabControl.SelectedItem = tabControl.userGroupTabItem;
            else if (grid.Name == "btnGroupGrid")
                tabControl.SelectedItem = tabControl.groupTabItem;
            else if (grid.Name == "btnRecentFriendGrid")
                tabControl.SelectedItem = tabControl.recentFriendTabItem;
            else if (grid.Name == "btnSetting")
            {
                tabControl.SelectedItem = tabControl.settingTabItem;
            }

        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


            init();


        }

        public void init()
        {

           
                  

                        txtNickName.Text = DataUtil.Member.nickName;
                        DataUtil.MessageTabControl = this.mesgTabControl;

                        selectedGrid = btnFriendGrid;
                        selectedGrid.Background = new SolidColorBrush(selectedColor);

                        tabControl = new MyTabControl();
                        Grid.SetRow(tabControl, 1);
                        MiddleGrid.Children.Add(tabControl);

                        SendHeartBeat();
                       
                   
               
           



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
 