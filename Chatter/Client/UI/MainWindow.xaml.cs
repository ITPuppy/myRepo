using Client.ChatterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chatter.Client.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatterClient client;
        private Member member;

        public MainWindow()
        {
            InitializeComponent();
           
        }

        public MainWindow(ChatterClient client,Member member)
        {
           
            InitializeComponent();
            this.client = client;
            this.member = member;
        }
        
        private void init()
        {
            ///注册退出事件
            Application.Current.Exit += new ExitEventHandler((sender, e) => { if(client!=null&&member!=null) client.Logoff(member); });
            ///获得好友列表,加到好友列表
           Member[] friends= client.GetFriends(member.id);
            
           FriendListView.Children.Add(new UserGroupUI("我的好友", friends));
            ///设置昵称
           this.txtNickName.Text = member.nickName;
           
        }

        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragForm(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }
        /// <summary>
        /// form加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }
        /// <summary>
        /// 选择显示的列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

             Point point = ListGrid.TransformToVisual(MainGrid).Transform(new Point());
            if (btn.Content.ToString() == "好友")
            {
                
                tabList.SelectedIndex = 0;
               
            }
          
            else if (btn.Content.ToString() == "最近联系人")
            {
                
                tabList.SelectedIndex = 1;
            }
            else if (btn.Content.ToString() == "群组")
            {
              
                tabList.SelectedIndex = 2;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }
    }
}
