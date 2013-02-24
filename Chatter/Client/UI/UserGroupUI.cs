using Client.ChatterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chatter.MetroClient.UI
{
    public class UserGroupUI : StackPanel
    {
        StackPanel spUserGroupName;
        StackPanel spUserList;
        private StackPanel selectedUser;
        #region Constructor
        /// <summary>
        /// 新建分组
        /// </summary>
        /// <param name="groupName">分组名</param>
        /// <param name="users">分组下的member</param>
        public UserGroupUI(string groupName, Member [] users)
            : base()
        {
            ///分组名
            spUserGroupName = NewUserGoupNameStackPanel(groupName);
            ///分组下的members
            spUserList = NewUserListStackPanel(users);
            ///分组下的分组名和分组member按流式布局排列，垂直方向
            this.Orientation = Orientation.Vertical;

            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            ///将分组名和分组下的用户加到分组stackpanel
            this.Children.Add(spUserGroupName);
            this.Children.Add(spUserList);
        }
        #endregion
        #region New StackPanel
        /// <summary>
        /// 新建一个stackpanel,里面放该分组下的member
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <returns>含有member的stackPanel</returns>
        private StackPanel NewUserListStackPanel(Member[] users)
        {
            StackPanel sp = new StackPanel();
            sp.Visibility = Visibility.Collapsed;
            for (int i = 0; i < users.Length;i++ )
            {
                ///将含有用户的stackPanel加到stackPanel
                sp.Children.Add(NewUserStackPanel(users[i].nickName));
            }
            return sp;
        }

        /// <summary>
        ///新建一个stackPanel，里面含有一个用户
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <returns></returns>
        private StackPanel NewUserStackPanel(string nickName)
        {
            StackPanel sp = new StackPanel();

            ///注册鼠标进入事件
            sp.MouseEnter += sp_MouseEnter;
            ///注册鼠标离开事件
            sp.MouseLeave += sp_MouseLeave;
            ///注册鼠标左键按下事件
            sp.MouseLeftButtonDown += sp_MouseLeftButtonDown;
            sp.Orientation = Orientation.Horizontal;

            ///用户头像
            Image image = new Image(); ;
            image.Height = 40;
            image.Width = 40;
            image.Source = new BitmapImage(new Uri("/Client;component/res/img/default.jpg", UriKind.Relative));
            ///用户昵称textBlock  
            TextBlock tb = new TextBlock();
            tb.Text = nickName;
            tb.FontSize = 15;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Margin = new Thickness(20, 0, 0, 0);
            sp.Height = 45;
            sp.Children.Add(image);
            sp.Children.Add(tb);

            return sp;
        }
        #endregion
        #region EventHandler
        /// <summary>
        /// 鼠标进入事件，当鼠标进入某个member的StackPanel，stackpanel 颜色发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sp_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            if (sp == selectedUser)
            {
                sp.Background = new SolidColorBrush(Colors.Wheat);
            }
            else
            {

                sp.Background = new SolidColorBrush(Colors.SkyBlue);
            }

        }
        /// <summary>
        /// 鼠标左击事件，鼠标点击时，颜色发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            StackPanel sp = sender as StackPanel;
            if (selectedUser != null && sp != selectedUser)
            {
                selectedUser.Background = new SolidColorBrush(Colors.Snow);
            }
            selectedUser = sp;

        }
        /// <summary>
        /// 鼠标离开事件，鼠标离开某个member的stackpanel，stackpanel发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sp_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            ///如果当前stackpanel被点击，
            if (sp != selectedUser)
                sp.Background = new SolidColorBrush(Colors.Snow);
            else
                sp.Background = new SolidColorBrush(Colors.Wheat);
        }
        /// <summary>
        /// 新建一个包含分组名的stackpanel
        /// </summary>
        /// <param name="groupName">分组名</param>
        /// <returns></returns>
        private StackPanel NewUserGoupNameStackPanel(string groupName)
        {

            StackPanel sp = new StackPanel();
            sp.Height = 30;
            ///标记该分组为展开
            sp.Tag = "0";
            ///注册展开事件
            sp.MouseLeftButtonDown += ToggleList;
            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Stretch;
            ///分组名前的箭头
            Image image = new Image(); ;
            image.Width = 8;
            image.Height = 8;
            image.Source = new BitmapImage(new Uri("/Client;component/res/img/rightarrow.gif", UriKind.Relative));
            ///分组名
            TextBlock tb = new TextBlock();
            tb.Text = groupName;
            tb.FontSize = 15;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Margin = new Thickness(40, 0, 0, 0);
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(image);
            sp.Children.Add(tb);
            return sp;
        }
        /// <summary>
        /// 分组展开或者合上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleList(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            ///未展开时，展开
            if (spUserGroupName.Tag.ToString() == "0")
            {
                Image image = spUserGroupName.Children[0] as Image;
                ///箭头改为向下
                image.Source = image.Source = new BitmapImage(new Uri("/Client;component/res/img/downarrow.gif", UriKind.Relative));
                ///显示列表
                this.spUserList.Visibility = Visibility.Visible;
                spUserGroupName.Tag = "1";
            }
            ///展开时，合上
            else if (spUserGroupName.Tag.ToString() == "1")
            {
                //箭头改为向右
                Image image = spUserGroupName.Children[0] as Image;
                image.Source = image.Source = new BitmapImage(new Uri("/Client;component/res/img/rightarrow.gif", UriKind.Relative));
                ///隐藏列表
                this.spUserList.Visibility = Visibility.Collapsed;
                spUserGroupName.Tag = "0";
            }
        }
        #endregion
    }
}
