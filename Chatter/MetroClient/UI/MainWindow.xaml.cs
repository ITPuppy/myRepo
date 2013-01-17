
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
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color selectedColor = Color.FromArgb(255, 114, 119, 123);
        Grid selectedGrid;
        ChatterClient client;
        Member member;
        Dictionary<string, Friend[]> dicFriends;
        private MyTabControl tabControl;
        public MainWindow()
        {
            InitializeComponent();
            
          
           
        }
        public MainWindow(ChatterClient client, Member member)
        {
          

            InitializeComponent();
            this.client = client;
            this.member = member;
        }
        

        private void MainWindow_Drag(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
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
          
        }

     
       
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Friend[] friends1 = new Friend[24] {
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } ,
                 new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } ,
                 new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } ,
                 new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } ,
                 new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } ,
                 new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }, 
                new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } 

            };
            Friend[] friends2 = new Friend[2] { new Friend() { member = new Member() { nickName = "曾令根" }, userGroupName = "大学同学", userGroupId = "23423" }, new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } };

            Friend[] friends3 = new Friend[2] { new Friend() { member = new Member() { nickName = "丁得健" }, userGroupName = "高中同学", userGroupId = "23423" }, new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } };
            Friend[] friends4 = new Friend[2] { new Friend() { member = new Member() { nickName = "eva" }, userGroupName = "我的家人", userGroupId = "23423" }, new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" }};
            Friend[] friends5 = new Friend[2] { new Friend() { member = new Member() { nickName = "陈达" }, userGroupName = "朋友", userGroupId = "23423" }, new Friend() { member = new Member() { nickName = "walle" }, userGroupName = "我的好友", userGroupId = "23423" } };
            dicFriends = new Dictionary<string, Friend[]>();
            dicFriends.Add("1223",friends1);
            dicFriends.Add("124323", friends2);
            dicFriends.Add("122af3", friends3);

            dicFriends.Add("122e3", friends4);
            dicFriends.Add("1223aa", friends5);

            init();

            Application.Current.Exit += new ExitEventHandler((obj, args) => {
                if (client != null)
                    client.Logoff(member);
            });
        }

        private void init()
        {
            selectedGrid = btnFriendGrid;
            selectedGrid.Background = new SolidColorBrush(selectedColor);
          dicFriends=  client.GetFriends(member.id);
           tabControl = new MyTabControl(dicFriends);
          Grid.SetRow(tabControl,1);
          MiddleGrid.Children.Add(tabControl);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {

            Application.Current.Shutdown();
            base.OnClosing(e);
        }
    }
}
