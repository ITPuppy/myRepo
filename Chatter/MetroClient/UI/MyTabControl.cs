
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
      
        private TabItem userGroupTabItem;
        private TabItem groupTabItem;
        private TabItem recentFriendTabItem;
        private UserGroup[] userGroups;
        private ChatterClient client;


        public MyTabControl(UserGroup[] userGroups,ChatterClient client):base()
        {
            this.client = client;
            this.userGroups = userGroups;
            this.Background = new SolidColorBrush(Color.FromArgb(255, 76, 141, 174));
            Style s = new Style();
            s.TargetType = typeof(TabItem);
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            this.ItemContainerStyle = s;

            ///好友列表
            userGroupTabItem = new MyTabItem(MyType.UserGroup, userGroups, client);
            this.Items.Add(userGroupTabItem);
            ///群组列表
            this.Items.Add(new TabItem());
            ///最近联系人列表
            this.Items.Add(new TabItem());
            ///设置
            this.Items.Add(new TabItem());
            
            foreach (UserGroup userGroup in userGroups)
            {
                this.Items.Add(new MyTabItem(MyType.User,userGroup.members,client));
            }
        }

      

      

    }
}
