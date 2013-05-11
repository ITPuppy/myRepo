
using Chatter.Log;
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
      
        public TabItem userGroupTabItem;
        public TabItem groupTabItem;
        public TabItem recentFriendTabItem;
        public TabItem settingTabItem;
      
      
       

        public MyTabControl():base()
        {
            
           
            //this.Background = new SolidColorBrush(Color.FromArgb(255, 76, 141, 174));
           // this.Background = new SolidColorBrush(Colors.White);
            this.Background = new SolidColorBrush(Colors.Transparent);
            Style s = new Style();
            s.TargetType = typeof(TabItem);
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            this.ItemContainerStyle = s;



            DataUtil.Client.GetFriendsCompleted += Client_GetFriendsCompleted;
            DataUtil.Client.GetFriendsAsync(DataUtil.Member.id);


          
         
     
          
        }

       

        void Client_GetFriendsCompleted(object sender, GetFriendsCompletedEventArgs e)
        {
            DataUtil.UserGroups = e.Result.ToList<UserGroup>();
            Dispatcher.Invoke(new Action(() =>
            {

                try
                {
                    ///分组列表
                    userGroupTabItem = new MyTabItem(MyType.UserGroup);
                    userGroupTabItem.Tag = "UserGroup";
                    this.Items.Add(userGroupTabItem);
                    foreach (UserGroup userGroup in DataUtil.UserGroups)
                    {
                        MyTabItem tabItem = new MyTabItem(MyType.User, userGroup.userGroupId);
                        this.Items.Add(tabItem);
                        DataUtil.FriendTabItems.Add(userGroup.userGroupId, tabItem);

                    }
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("获取朋友列表出错",ex);
                }
            }));

            DataUtil.Client.GetGroupsCompleted += Client_GetGroupsCompleted;
            DataUtil.Client.GetGroupsAsync(DataUtil.Member.id);
           
        }



        void Client_GetGroupsCompleted(object sender, GetGroupsCompletedEventArgs e)
        {

            try
            {
                if (e.Result != null)
                {
                    DataUtil.Groups = e.Result.ToList<Group>();
                    DataUtil.GetP2PClient();
                }
                Dispatcher.Invoke(new Action(() =>
                {

                    ///群组
                    groupTabItem = new MyTabItem(MyType.Group);
                    groupTabItem.Tag = "Group";
                    foreach (Group group in DataUtil.Groups)
                    {
                        MyTabItem tabItem = new MyTabItem(MyType.UserInGroup, group.GroupId);
                        this.Items.Add(tabItem);
                        DataUtil.GroupMemberTabItems.Add(group.GroupId, tabItem);

                    }

                }));

            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("获取群列表出错",ex);
            }
            this.Items.Add(groupTabItem);
            recentFriendTabItem = new TabItem();
            settingTabItem = new TabItem();
            this.Items.Add(recentFriendTabItem);
            this.Items.Add(settingTabItem);
        }


    }
}
