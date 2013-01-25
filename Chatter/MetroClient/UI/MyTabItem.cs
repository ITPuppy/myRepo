﻿using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    public class MyTabItem:TabItem
    {
        /// <summary>
        /// 进度条
        /// </summary>
        public ScrollViewer scrollViewer;
        /// <summary>
        /// 用来放MyButton的Grid
        /// </summary>
        public MyGrid myGrid;
        /// <summary>
        /// 代理Client
        /// </summary>
    
        /// <summary>
        /// 如果当前TabItem是用来放置好友列表，则userGroupId为当前好友的分组ID，其他时候为-1
        /// </summary>
        private string userGroupId=String.Empty;
      

        private MyTabControl parentTabControl ;
        public MyTabControl ParentTabControl
        {

            get
            {
                if (parentTabControl != null)
                    return parentTabControl;
              
                parentTabControl = this.Parent as MyTabControl;
                return parentTabControl;
            }
        }
        public MyTabItem(MyType type,string userGroupId="-1")
        {

 
         
            ///设置分组ID
            this.userGroupId = userGroupId;

            ///滚动条设置
            scrollViewer = new ScrollViewer();
            ///垂直对齐stretch
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            ///垂直滚动条可见性设置为Auto，当需要时候出现
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ///水平对齐为Stretch
            scrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
           



            switch (type)
            {
                    ///用户分组
                case MyType.UserGroup:
                    {

                      
                        
                        ///右键菜单
                        ContextMenu cm = new ContextMenu();
                        ///添加分组
                        MenuItem addUserGroupMenuItem = new MenuItem();
                        cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                        cm.Foreground = new SolidColorBrush(Colors.White);
                        
                        addUserGroupMenuItem.Header = "添加分组";
                        addUserGroupMenuItem.Click += addUserGroupMenuItem_Click;
                        cm.Items.Add(addUserGroupMenuItem);
                        this.ContextMenu = cm;
                        
                        ///新建分组MyGrid
                         myGrid = new MyGrid(MyType.UserGroup);
                        ///添加分组结束后回调函数，此行代码不能放到事件处理函数中，因为会多次注册事件
                         DataUtil.Client.AddUserGroupCompleted += client_AddUserGroupCompleted;
                       
                        break;
                    }

                    ///好友
                case MyType.User:
                    {

                    
                        ///右键菜单
                        ContextMenu cm = new ContextMenu();
                        MenuItem addFriendMenuItem = new MenuItem();
                        cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                        cm.Foreground = new SolidColorBrush(Colors.White);

                        addFriendMenuItem.Header = "添加好友";
                        addFriendMenuItem.Click += addFriendMenuItem_Click;
                        cm.Items.Add(addFriendMenuItem);
                        this.ContextMenu = cm;
                     
                         myGrid = new MyGrid(MyType.User,this.userGroupId);

                    

                        break;
                    }
            }


            ///将控件加到当前TabItem
            scrollViewer.Content = myGrid;
            this.Content = scrollViewer;
            
        }

        /// <summary>
        /// 添加好友事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBaseRoleDialog dialog = new AddBaseRoleDialog(MyType.User);
            dialog.ShowDialog();
            string friendId = dialog.GetString();
            if (friendId == null || friendId.Length == 0)
                return;
           ///需要添加是否已经是好友的判断

            if (DataUtil.IsFriend(friendId))
            {
                MessageBox.Show("对方已经是您的好友");
                return;
            }
            if (DataUtil.Member.id == friendId)
            {
                MessageBox.Show("您不能添加自己为好友");
                return;
            }
            Dispatcher.Invoke(new Action(() => { DataUtil.Client.AddFriend(friendId, userGroupId); }));
            

        }
        ///// <summary>
        ///// 添加好友回调函数
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void client_AddFriendCompleted(object sender, AddFriendCompletedEventArgs e)
        //{

        //    try
        //    {
        //        if (e.Error != null)
        //            throw e.Error;

        //        if (e.Result.status == MessageStatus.Failed)
        //        {
        //            MessageBox.Show("对方不在线");
        //            return;
        //        }
        //        else if (e.Result.status == MessageStatus.Refuse)
        //        {
        //            MessageBox.Show("对方拒绝了您的请求");
        //            return;
        //        }
        //        Member friend = e.Result.member;
        //        if (friend == null)
        //        {
        //            MessageBox.Show("添加好友失败");
        //            return;
        //        }
        //        ///将好友加到UI上面
        //        else
        //        {
        //            myGrid.AddButton(MyType.User, friend);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("网络出现异常，请检查网络连接");
        //        return;
        //    }

        //}

        /// <summary>
        /// 添加分组回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddUserGroupCompleted(object sender, AddUserGroupCompletedEventArgs e)
        {
            try
            {
                ///判断是否有异常

                if (e.Error != null)
                    throw e.Error;
                ///判断添加分组是否成功
                 if (e.Result.status == MessageStatus.Failed)
                {
                    MessageBox.Show("添加失败");
                }
                else
                {
                
                    ///读取添加的分组名和ID
                    string userGroupId = e.Result.userGroup.userGroupId;
                    string userGroupName = e.Result.userGroup.userGroupName;


                    ///将新添加的分组加到 数组里面记录
                
                    UserGroup ug = new UserGroup() { userGroupId = userGroupId, userGroupName = userGroupName };
                   
               
                    ///在界面上添加分组
                    myGrid.AddButton(MyType.UserGroup, ug);
                    TabControl tabControl = this.Parent as TabControl;
                    ///添加分组对应的好友的TabItem
                    MyTabItem tabItem= new MyTabItem(MyType.User,  ug.userGroupId);
                    tabControl.Items.Add(tabItem);
                    DataUtil.FriendTabItems.Add(ug.userGroupId,tabItem);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络出现异常，请检查网络连接");
                return;
            }
        }

        /// <summary>
        /// 添加分组事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addUserGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {

            AddBaseRoleDialog dialog = new AddBaseRoleDialog(MyType.UserGroup);
            dialog.ShowDialog();
            string userGroupName=dialog.GetString();
            if (userGroupName == null || userGroupName.Length == 0)
                return;
       
            ///判断分组是否存在
         
            if (DataUtil.UserGroups.FirstOrDefault<UserGroup>( new Func<UserGroup, bool>( (ug) =>      
                    {
                        return ug.userGroupName == userGroupName;

                    } )) != null)
            {
                MessageBox.Show("分组"+userGroupName+"已经存在");
                return;
            }
            DataUtil.Client.AddUserGroupAsync(new UserGroup() { userGroupName = userGroupName });
        }
    }
}