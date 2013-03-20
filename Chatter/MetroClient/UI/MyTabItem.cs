using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using MetroClient.ChatterService;
using Chatter.MetroClient.P2P;
using Chatter.Log;

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
        /// 如果当前TabItem是用来放置好友列表，则userGroupId为当前好友的分组ID，如果是群组列表，则用来存放群组id，其他时候为-1
        /// </summary>
        private string baseRoleId=String.Empty;
      

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
        public MyTabItem(MyType type,string baseRoleId="-1")
        {

 
         
            ///设置分组ID
            this.baseRoleId = baseRoleId;

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
                     
                         myGrid = new MyGrid(MyType.User,this.baseRoleId);

                    

                        break;
                    }

                case MyType.UserInGroup:
                    {

                        if (DataUtil.GetGroupById(baseRoleId).OwnerId == DataUtil.Member.id)
                        {
                            ///右键菜单
                            ContextMenu cm = new ContextMenu();
                            MenuItem addMemberToGroupMenuItem = new MenuItem();
                            cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                            cm.Foreground = new SolidColorBrush(Colors.White);

                            addMemberToGroupMenuItem.Header = "添加成员";
                            addMemberToGroupMenuItem.Click += addMemberToGroupMenuItem_Click;
                            cm.Items.Add(addMemberToGroupMenuItem);
                            this.ContextMenu = cm;

                            myGrid = new MyGrid(MyType.UserInGroup, this.baseRoleId);

                        }

                        myGrid = new MyGrid(MyType.UserInGroup, this.baseRoleId);
                        break;
                    }


                case MyType.Group:
                        {
                            ///右键菜单
                            ContextMenu cm = new ContextMenu();
                            MenuItem addGroupMenuItem = new MenuItem();
                            cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                            cm.Foreground = new SolidColorBrush(Colors.White);

                            addGroupMenuItem.Header = "建立群组";
                            addGroupMenuItem.Click += addGroupMenuItem_Click;
                            cm.Items.Add(addGroupMenuItem);
                            this.ContextMenu = cm;

                            myGrid = new MyGrid(MyType.Group,this.baseRoleId);
                            DataUtil.Client.AddGroupCompleted += Client_AddGroupCompleted;
                            break;
                        }
            }


            ///将控件加到当前TabItem
            scrollViewer.Content = myGrid;
            this.Content = scrollViewer;
            
        }

        private void addMemberToGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                AddBaseRoleDialog dialog = new AddBaseRoleDialog(MyType.UserInGroup);
                dialog.ShowDialog();
                string memberId = dialog.GetString();
                if (String.IsNullOrEmpty(memberId))
                {
                    return;
                }

                if (!DataUtil.IsFriend(memberId))
                {
                    MessageBox.Show("不是好友");
                    return;
                }
                if (DataUtil.IsAlreadyMember(memberId, baseRoleId))
                {
                    MessageBox.Show("已经是组内成员");
                    return;
                }

                DataUtil.Client.AddFriend2Group(memberId, this.baseRoleId);
                P2PClient.GetP2PClient(baseRoleId).AddGroupMember(DataUtil.GetFriendById(memberId));
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("点击添加成员到群组时出现错误",ex);
            }

        }

        void Client_AddGroupCompleted(object sender, AddGroupCompletedEventArgs e)
        {
            try
            {
                ///判断是否有异常

                if (e.Error != null)
                    throw e.Error;
                ///判断添加分组是否成功
                if (e.Result.Status == MessageStatus.Failed)
                {
                    MessageBox.Show("建立群组失败");
                }
                else
                {


                   

                    ///在界面上添加组
                    myGrid.AddButton(MyType.Group, e.Result.Group);

                    TabControl tabControl = this.Parent as TabControl;
                    ///添加组内成员的TabItem
                    MyTabItem tabItem = new MyTabItem(MyType.UserInGroup,e.Result.Group.GroupId);
                    tabControl.Items.Add(tabItem);
                   
                    DataUtil.GroupMemberTabItems.Add(e.Result.Group.GroupId, tabItem);

                    ///添加group messageTabItem
                    var msgTabItem = new MyMessageTabItem(MyType.Group, e.Result.Group);
                    DataUtil.MessageTabControl.Items.Add(msgTabItem);
                    DataUtil.GroupMessageTabItems.Add(e.Result.Group.GroupId, msgTabItem);

                    ///群组udpclinet
                    DataUtil.P2PClients.Add(e.Result.Group.GroupId, P2PClient.GetP2PClient(e.Result.Group.GroupId));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络出现异常，请检查网络连接");
                return;
            }
        }

        /// <summary>
        /// 建群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddBaseRoleDialog dialog = new AddBaseRoleDialog(MyType.Group);
                dialog.ShowDialog();
                string groupName = dialog.GetString();
                if (String.IsNullOrEmpty(groupName))
                {
                    return;
                }

                if (DataUtil.IsGroupNameExist(groupName))
                {
                    MessageBox.Show("群组名字已经存在");
                    return;
                }

                Group group = new Group();
                group.Name = groupName;
                group.OwnerId = DataUtil.Member.id;

                DataUtil.Client.AddGroupAsync(group);
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("点击添加群组菜单错误",ex);
            }

        }

        /// <summary>
        /// 添加好友事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
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
                DataUtil.Client.AddFriend(friendId, baseRoleId);

            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("点击添加好友菜单错误", ex);
            }
        }
        
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
                 if (e.Result.Status == MessageStatus.Failed)
                {
                    MessageBox.Show("添加失败");
                }
                else
                {
                
                    ///读取添加的分组名和ID
                    string userGroupId = e.Result.UserGroup.userGroupId;
                    string userGroupName = e.Result.UserGroup.userGroupName;


                    

                    UserGroup ug = new UserGroup() { userGroupId = userGroupId, userGroupName = userGroupName, members = new Member[0] { } };
                   
               
                    ///在界面上添加分组
                    myGrid.AddButton(MyType.UserGroup, ug);
                    TabControl tabControl = this.Parent as TabControl;
                    ///添加分组对应的好友的TabItem
                    MyTabItem tabItem= new MyTabItem(MyType.User,  ug.userGroupId);
                    tabControl.Items.Add(tabItem);
                    DataUtil.FriendTabItems.Add(ug.userGroupId,tabItem);
                     ///将分组添加到记录里面
                  //  DataUtil.UserGroups.Add(ug);
                    
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
            try
            {
                AddBaseRoleDialog dialog = new AddBaseRoleDialog(MyType.UserGroup);
                dialog.ShowDialog();
                string userGroupName = dialog.GetString();
                if (userGroupName == null || userGroupName.Length == 0)
                    return;

                ///判断分组是否存在

                if (DataUtil.UserGroups.FirstOrDefault<UserGroup>(new Func<UserGroup, bool>((ug) =>
                        {
                            return ug.userGroupName == userGroupName;

                        })) != null)
                {
                    MessageBox.Show("分组" + userGroupName + "已经存在");
                    return;
                }
                DataUtil.Client.AddUserGroupAsync(new UserGroup() { userGroupName = userGroupName });
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("点击添加群组时出错",ex);
            }
        }
    }
}
