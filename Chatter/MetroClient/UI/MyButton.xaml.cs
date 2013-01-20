
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
        private double height = 95;
        private double weight = 95;
        private double zoomHeight = 105;
        private double zoomWidth = 105;
        private double fontSize = 15;
        private double zoomFontSize = 20;
        private double imageSize = 35;
        private double zoomImageSize = 40;
        public BaseRole baseRole;
        private MyType type;
        private int index=0;
        public String Text
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }


        public MyButton():base()
        {
            InitializeComponent();

        }

        public MyButton(MyType type, BaseRole baseRole, string imagesouce, Color color,int index):base()
        {
            this.index = index;
            this.type = type;
            this.baseRole=baseRole;
            
           
           

            if (type == MyType.UserGroup)
            {
                UserGroup userGroup= baseRole as UserGroup;
                txtName = new TextBlock();
                txtName.Text = userGroup.userGroupName;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                this.Children.Add(txtName);
                this.Background = new SolidColorBrush(color);
                ///右键菜单
                ContextMenu cm = new ContextMenu();
                cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                cm.Foreground = new SolidColorBrush(Colors.White);
                MenuItem deleteUserGroupMenuItem = new MenuItem();
                deleteUserGroupMenuItem.Header = "删除分组";
                deleteUserGroupMenuItem.Click += deleteUserGroupMenuItem_Click;
                cm.Items.Add(deleteUserGroupMenuItem);
                MenuItem changeUserGroupNameItem = new MenuItem();
                changeUserGroupNameItem.Header = "更改分组名";
                changeUserGroupNameItem.Click += changeUserGroupNameItem_Click;
                
                cm.Items.Add(changeUserGroupNameItem);
                
                this.ContextMenu = cm;
            }
            else if (type == MyType.User)
            {
                Member member = baseRole as Member;
                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(zoomImageSize);
                RowDefinition row2 = new RowDefinition();
                //  row2.Height = new GridLength(imageHeight);
                this.RowDefinitions.Add(row1);
                this.RowDefinitions.Add(row2);

                image = new Image();
                image.Source = new BitmapImage(new Uri(imagesouce, UriKind.Relative));
                image.Height = imageSize;
                image.Width = imageSize;
                Grid.SetRow(image, 0);

                txtName = new TextBlock();
                txtName.Text = member.nickName;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txtName, 1);
                ///用户图片
                this.Children.Add(image);
                ///昵称
                this.Children.Add(txtName);
                this.Background = new SolidColorBrush(color);
                
            }
            this.Width = weight;
            this.Height = height;
            this.MouseEnter += MyButton_MouseEnter;
            this.MouseLeave += MyButton_MouseLeave;
            this.MouseLeftButtonUp += MyButton_LeftButtonDown;
        }

        void changeUserGroupNameItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("确定要更改");
        }

        void deleteUserGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            UserGroup userGroup = baseRole as UserGroup;
            MessageBoxResult result= MessageBox.Show("确定要删除"+userGroup.userGroupName+"?","删除",MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

            }

        }

        private void MyButton_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (type)
            {
                case MyType.User:
                    {
                        MessageBox.Show("聊天");
                        break;
                    }
                case MyType.UserGroup:
                    {


                        Grid grid = this.Parent as Grid;
                        ScrollViewer scrollViewer = grid.Parent as ScrollViewer;
                        TabItem tabItem = scrollViewer.Parent as TabItem;
                        TabControl tabControl = tabItem.Parent as TabControl;
                        tabControl.SelectedIndex = index + 4;

                        break;
                    }
            }
        }


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
    }



    public enum MyType
    {
        UserGroup,
        User,
        Group,
        RecentFriend
    }
}
