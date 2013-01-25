
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
    public class MyTabControl:TabControl
    {
      
        private TabItem userGroupTabItem;
        private TabItem groupTabItem;
        private TabItem recentFriendTabItem;
      
      
       

        public MyTabControl():base()
        {
            
           
            this.Background = new SolidColorBrush(Color.FromArgb(255, 76, 141, 174));
            Style s = new Style();
            s.TargetType = typeof(TabItem);
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            this.ItemContainerStyle = s;

            ///好友列表
            userGroupTabItem = new MyTabItem(MyType.UserGroup);
            this.Items.Add(userGroupTabItem);
            ///群组列表
            this.Items.Add(new TabItem());
            ///最近联系人列表
            this.Items.Add(new TabItem());
            ///设置
            this.Items.Add(new TabItem());
            
            foreach (UserGroup userGroup in DataUtil.UserGroups)
            {
                MyTabItem tabItem= new MyTabItem(MyType.User,userGroup.userGroupId);
                this.Items.Add(tabItem);
                DataUtil.FriendTabItems.Add(userGroup.userGroupId, tabItem);
            }
        }

      

      

    }
}
