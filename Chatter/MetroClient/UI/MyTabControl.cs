
using MetroClient.ChatterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
           
            userGroupTabItem=NewUserGroupTab();

            this.SetValue(TabItem.VisibilityProperty, Visibility.Collapsed);
        }

        private TabItem NewUserGroupTab()
        {
            TabItem tabItem = new TabItem();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
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
