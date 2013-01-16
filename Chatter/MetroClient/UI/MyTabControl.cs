
using MetroClient.ChatterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chatter.MetroClient.UI
{
    class MyTabControl:TabControl
    {
        private Dictionary<string, Friend[]> dicFriends;
        private TabItem userGroupTabItem;
        private TabItem groupTabItem;
        private TabItem recentFriendTabItem;

        public MyTabControl(Dictionary<string,Friend[]> dicFriends):base()
        {
            
            this.dicFriends = dicFriends;
            this.Background = new SolidColorBrush(Color.FromArgb(255, 76, 141, 174));
            Style s = new Style();
            s.TargetType = typeof(TabItem);
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            this.ItemContainerStyle = s;

            ///好友列表
            userGroupTabItem=NewUserGroupTab();
            this.Items.Add(userGroupTabItem);
            ///群组列表
            this.Items.Add(new TabItem());
            ///最近联系人列表
            this.Items.Add(new TabItem());

            foreach(KeyValuePair<string,Friend[]> userGroup in dicFriends)
            {
                this.Items.Add(NewFriendTab(userGroup.Value));
            }

            
        }

        private TabItem NewFriendTab(Friend[] friends)
        {
            TabItem tabItem = new TabItem();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
           
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            MyGrid grid = new MyGrid(friends);
            scrollViewer.Content = grid;
            tabItem.Content = scrollViewer;
            return tabItem;
        }

        private TabItem NewUserGroupTab()
        {
            TabItem tabItem = new TabItem();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            scrollViewer.Foreground = new SolidColorBrush(Colors.Red);
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Friend[]> keyValue in dicFriends)
            {
                dic.Add(keyValue.Key,keyValue.Value[0].userGroupName);
            }
             MyGrid grid= new MyGrid(dic);
            scrollViewer.Content=grid;
            tabItem.Content = scrollViewer;
            return tabItem;
            
        }
    }
}
