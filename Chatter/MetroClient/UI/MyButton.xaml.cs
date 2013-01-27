
using MetroClient.ChatterService;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// MyButton.xaml 的交互逻辑
    /// </summary>
    public partial class MyButton : Grid
    {
        /// <summary>
        /// Button高度
        /// </summary>
        private double height = 95;
        /// <summary>
        /// Button宽度
        /// </summary>
        private double weight = 95;
        /// <summary>
        /// 放大的时候的高度
        /// </summary>
        private double zoomHeight = 105;
        /// <summary>
        /// 放大的时候的宽度
        /// </summary>
        private double zoomWidth = 105;
        /// <summary>
        /// 名称字体的大小
        /// </summary>
        private double fontSize = 15;
        /// <summary>
        /// 放大的时候的名称字体大小
        /// </summary>
        private double zoomFontSize = 20;
        /// <summary>
        /// 图片大小
        /// </summary>
        private double imageSize = 35;
        /// <summary>
        /// 放大图片大小
        /// </summary>
        private double zoomImageSize = 40;
        /// <summary>
        /// 存放当前对象，Member或者UserGroup或者Group
        /// </summary>
        public BaseRole baseRole;
        /// <summary>
        /// Button类型
        /// </summary>
        private MyType type;
        /// <summary>
        /// 当前Button在MyGrid中的index，用于指定位置
        /// </summary>
        private string userGroupId;


        private Color offlineColor = Color.FromArgb(255,192, 192, 192);
        private Color onlineColor = Colors.OrangeRed;
        public String Text
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }
        private MyTabControl parentTabControl;
        public MyTabControl ParentTabControl
        {

            get
            {
                if (parentTabControl != null)
                    return parentTabControl;

                MyGrid grid = this.Parent as MyGrid;
                ScrollViewer scrollViewer = grid.Parent as ScrollViewer;
                TabItem tabItem = scrollViewer.Parent as TabItem;
                parentTabControl = tabItem.Parent as MyTabControl;
                return parentTabControl;
            }

        }

        public MyButton()
            : base()
        {
            InitializeComponent();

        }
        /// <summary>
        /// MyButton构造函数
        /// </summary>
        /// <param name="type">Button类型</param>
        /// <param name="baseRole">Member或者UserGroup或者Group</param>
        /// <param name="imagesouce">如果是用户，用户头像，其他目前为空</param>
        /// <param name="color">Button颜色</param>
        public MyButton(MyType type, BaseRole baseRole, string imagesouce, Color color, string userGroupId = "-1")
            : base()
        {
            ///设置分组ID
            this.userGroupId = userGroupId;
            ///设置类型
            this.type = type;
            ///设置对象Member或者UserGroup或者Group
            this.baseRole = baseRole;
            this.onlineColor = color;


            #region 分组
            if (type == MyType.UserGroup)
            {
                UserGroup userGroup = baseRole as UserGroup;
                ///分组名称
                txtName = new TextBlock();
                txtName.Text = userGroup.userGroupName;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                this.Children.Add(txtName);





                ///右键菜单
                ContextMenu cm = new ContextMenu();
                ///菜单的背景色
                cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                ///菜单的前景色
                cm.Foreground = new SolidColorBrush(Colors.White);
                ///删除分组菜单项
                MenuItem deleteUserGroupMenuItem = new MenuItem();
                deleteUserGroupMenuItem.Header = "删除分组";
                ///删除分组事件
                deleteUserGroupMenuItem.Click += deleteUserGroupMenuItem_Click;
                cm.Items.Add(deleteUserGroupMenuItem);

                ///更改分组名菜单项
                MenuItem changeUserGroupNameItem = new MenuItem();
                changeUserGroupNameItem.Header = "更改分组名";
                ///更改分组名事件
                changeUserGroupNameItem.Click += changeUserGroupNameItem_Click;

                cm.Items.Add(changeUserGroupNameItem);

                this.ContextMenu = cm;
                this.Background = new SolidColorBrush(color);
            }
            #endregion

            #region 好友
            else if (type == MyType.User)
            {
                Member member = baseRole as Member;

                ///第一行用来放置头像
                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(zoomImageSize);
                ///第二行用来放置昵称
                RowDefinition row2 = new RowDefinition();
                //  row2.Height = new GridLength(imageHeight);
                this.RowDefinitions.Add(row1);
                this.RowDefinitions.Add(row2);

                ///用户头像
                image = new Image();
                image.Source = new BitmapImage(new Uri(imagesouce, UriKind.Relative));
                image.Height = imageSize;
                image.Width = imageSize;
                Grid.SetRow(image, 0);
                ///用户昵称
                txtName = new TextBlock();
                txtName.Text = member.nickName;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txtName, 1);
                ///添加用户头像
                this.Children.Add(image);
                ///添加昵称
                this.Children.Add(txtName);



                ///右键菜单
                ContextMenu cm = new ContextMenu();
                ///菜单的背景色
                cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                ///菜单的前景色
                cm.Foreground = new SolidColorBrush(Colors.White);
                ///删除好友菜单项
                MenuItem deleteFriendMenuItem = new MenuItem();
                deleteFriendMenuItem.Header = "删除好友";
                ///删除好友事件
                deleteFriendMenuItem.Click += deleteFriendMenuItem_Click;
                cm.Items.Add(deleteFriendMenuItem);

                ///查看好友资料菜单项
                MenuItem viewFriendMenuItemItem = new MenuItem();
                viewFriendMenuItemItem.Header = "查看资料";
                ///更改分组名事件
                viewFriendMenuItemItem.Click += viewFriendMenuItemItem_Click;

                cm.Items.Add(viewFriendMenuItemItem);

                this.ContextMenu = cm;
                if (member.status == MemberStatus.Online)
                    this.Background = new SolidColorBrush(onlineColor);
                else if (member.status == MemberStatus.Offline)
                    this.Background = new SolidColorBrush(offlineColor);
                

            #endregion
            }

            ///背景色为MyGrid里面传进来的颜色
           
            this.Width = weight;
            this.Height = height;

            ///鼠标进入事件，放大
            this.MouseEnter += MyButton_MouseEnter;
            ///鼠标离开事件，恢复
            this.MouseLeave += MyButton_MouseLeave;

            ///鼠标点击事件
            this.MouseLeftButtonUp += MyButton_LeftButtonDown;
        }
       /// <summary>
       /// 删除好友出发事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        void deleteFriendMenuItem_Click(object sender, RoutedEventArgs e)
        {
           
        }
        /// <summary>
        /// 查看好友资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void viewFriendMenuItemItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 更改分组名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void changeUserGroupNameItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("确定要更改");
        }


        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deleteUserGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            UserGroup userGroup = baseRole as UserGroup;
            if (userGroup.userGroupId == "0")
            {
                MessageBox.Show("默认分组不允许删除");
                return;
            }
            MessageBoxResult result = MessageBox.Show("确定要删除" + userGroup.userGroupName + ",并将分组下好友移至默认分组?", "确认", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                DataUtil.Client.DeleteUserGroupCompleted += Client_DeleteUserGroupCompleted;
                DataUtil.Client.DeleteUserGroupAsync(DataUtil.Member.id, userGroup);


            }

        }
        /// <summary>
        /// 删除分组回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_DeleteUserGroupCompleted(object sender, DeleteUserGroupCompletedEventArgs e)
        {

            try
            {
                if (e.Error != null)
                    throw e.Error;
                if (e.Result.status == MessageStatus.Failed)
                {
                    MessageBox.Show("删除失败");
                    return;
                }



                UserGroup userGroup = baseRole as UserGroup;




                ///将好友移至默认分组

                MyTabItem tabItem = DataUtil.FriendTabItems[userGroup.userGroupId];
                MyButton[] friendArray = new MyButton[tabItem.myGrid.Children.Count];
                tabItem.myGrid.Children.CopyTo(friendArray, 0);

                MyTabItem defaultTabItem = DataUtil.FriendTabItems["0"];
                for (int i = 0; i < friendArray.Length; i++)
                {
                    defaultTabItem.myGrid.AddButton(MyType.User, (friendArray[i].baseRole as Member));
                }


                ///删除原分组。并将后面的分组前移
                MyGrid myGrid = this.Parent as MyGrid;

                int currentIndex = myGrid.Children.IndexOf(this);
                ///删掉分组对应的好友分组
                ParentTabControl.Items.Remove(DataUtil.FriendTabItems[userGroup.userGroupId]);
                DataUtil.FriendTabItems.Remove(userGroup.userGroupId);
                ///删掉分组
                myGrid.Children.Remove(this);

                ///删除全局的分组记录
                DataUtil.DeleteUserGroup(userGroup.userGroupId);

                ///将后面的分组移除
                List<MyButton> temp = new List<MyButton>();
                for (; currentIndex < myGrid.Children.Count; )
                {
                    temp.Add(myGrid.Children[currentIndex] as MyButton);
                    myGrid.Children.RemoveAt(currentIndex);

                }
                ///将后面的分组前移后加上
                foreach (MyButton button in temp)
                {
                    Grid.SetRow(button, currentIndex / 3);
                    Grid.SetColumn(button, currentIndex % 3);
                    myGrid.Children.Add(button);
                    currentIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，出现异常");
            }
            finally
            {
                DataUtil.Client.DeleteUserGroupCompleted -= Client_DeleteUserGroupCompleted;
            }
        }


        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (type)
            {
                case MyType.User:
                    {
                        DataUtil.CurrentRole = this.baseRole;

                      //  DataUtil.ShowMesgWindow();
                        break;
                    }
                case MyType.UserGroup:
                    {
                        UserGroup userGroup = baseRole as UserGroup;
                        ///根据分组名查找好友列表（字典实现）
                        ParentTabControl.SelectedItem = DataUtil.FriendTabItems[userGroup.userGroupId];
                        break;
                    }
            }
        }



        /// <summary>
        /// 鼠标离开恢复事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;

            btn.Width = height;
            btn.Height = Width;
            txtName.FontSize = fontSize;
            if (image != null)
            {
                image.Height = imageSize;
                image.Width = imageSize;
            }
        }
        /// <summary>
        /// 鼠标进入放大事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;
            btn.Width = zoomWidth;
            btn.Height = zoomHeight;
            txtName.FontSize = zoomFontSize;
            if (image != null)
            {
                image.Height = zoomImageSize;
                image.Width = zoomImageSize;
            }
        }


        public void ChangeMemberStatus(MemberStatus status)
        {
            switch(status)
            {
                case MemberStatus.Online:
                    {
                        this.Background=new SolidColorBrush(onlineColor);
                        break;
                    }
                case MemberStatus.Offline:
                    {
                        this.Background=new SolidColorBrush(offlineColor);
                        break;
                    }

            }
        }
    }



    public enum MyType
    {
        UserGroup,
        User,
        Group,
        RecentFriend
    }
}
