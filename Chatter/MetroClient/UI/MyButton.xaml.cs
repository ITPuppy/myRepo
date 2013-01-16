
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
        public Friend friend;
        private ButtonType type;
        private int index=0;
        public String Text
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }


        public MyButton()
        {
            InitializeComponent();

        }

        public MyButton(ButtonType type, Object obj, string imagesouce, Color color,int index)
        {
            this.index = index;
            this.type = type;
            string name;
            if (obj is Friend)
            {
                this.friend = obj as Friend;
                name = friend.member.nickName;
            }
            else
            {
                name = obj.ToString();
            }

            if (type == ButtonType.UserGroup)
            {
                txtName = new TextBlock();
                txtName.Text = name;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                this.Children.Add(txtName);
                this.Background = new SolidColorBrush(color);
            }
            else if (type == ButtonType.User)
            {
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
                txtName.Text = name;
                txtName.FontSize = fontSize;
                txtName.Foreground = new SolidColorBrush(Colors.White);
                txtName.VerticalAlignment = VerticalAlignment.Center;
                txtName.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRow(txtName, 1);
                this.Children.Add(image);
                this.Children.Add(txtName);
                this.Background = new SolidColorBrush(color);
            }
            this.Width = weight;
            this.Height = height;
            this.MouseEnter += MyButton_MouseEnter;
            this.MouseLeave += MyButton_MouseLeave;
            this.MouseLeftButtonUp += MyButton_LeftButtonDown;
        }

        private void MyButton_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (type)
            {
                case ButtonType.User:
                    {
                        MessageBox.Show("聊天");
                        break;
                    }
                case ButtonType.UserGroup:
                    {


                        Grid grid = this.Parent as Grid;
                        ScrollViewer scrollViewer = grid.Parent as ScrollViewer;
                        TabItem tabItem = scrollViewer.Parent as TabItem;
                        TabControl tabControl = tabItem.Parent as TabControl;
                        tabControl.SelectedIndex = index + 3;

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



    public enum ButtonType
    {
        UserGroup,
        User,
        Group,
        RecentFriend
    }
}
