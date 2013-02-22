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
using MetroClient.ChatterService;
using Microsoft.Win32;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for MyMenu.xaml
    /// </summary>
    public partial class MyMenu :Grid
    {
        /// <summary>
        /// Button高度
        /// </summary>
        private double height = 35;
        /// <summary>
        /// Button宽度
        /// </summary>
        private double weight = 35;
        /// <summary>
        /// 放大的时候的高度
        /// </summary>
        private double zoomHeight = 40;
        /// <summary>
        /// 放大的时候的宽度
        /// </summary>
        private double zoomWidth = 40;
        /// <summary>
        /// 名称字体的大小
        /// </summary>
        private double fontSize = 12;
        /// <summary>
        /// 放大的时候的名称字体大小
        /// </summary>
        private double zoomFontSize = 15;
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



        private Color offlineColor = Color.FromArgb(255, 192, 192, 192);
        private Color onlineColor = Colors.OrangeRed;


        public MyMenu(BaseRole role,string menuName)
        {
            InitializeComponent();
            this.baseRole = role;
            txtName = new TextBlock();
            txtName.FontSize = fontSize;
            txtName.Foreground = new SolidColorBrush(Colors.White);
            txtName.Text = menuName;
            txtName.TextWrapping = TextWrapping.Wrap;
            txtName.VerticalAlignment = VerticalAlignment.Center;
            txtName.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(txtName,0);
            this.Children.Add(txtName);
            this.Width = weight;
            this.Height = height;
            this.onlineColor = DataUtil.GetRandomColor();


            ///鼠标进入事件，放大
            this.MouseEnter += MyMenu_MouseEnter;
            ///鼠标离开事件，恢复
            this.MouseLeave += MyMenu_MouseLeave;

            ///鼠标点击事件

            //this.MouseLeftButtonUp += MyMenu_MouseLeftButtonUp;
           // this.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.MyMenu_MouseLeftButtonUp), true);
           this.MouseLeftButtonDown += MyMenu_MouseLeftButtonDown;
            if (this.baseRole is Member)
            {
                Member member = this.baseRole as Member;
                if (member.status == MemberStatus.Online)
                    this.Background = new SolidColorBrush(onlineColor);
                else if (member.status == MemberStatus.Offline)
                    this.Background = new SolidColorBrush(offlineColor);
            }
        }

       

        void MyMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (this.baseRole is Member)
            {
                Member member = this.baseRole as Member;
                if (member.status == MemberStatus.Offline)
                {
                    return;
                }

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();

                FileMessage fm = new FileMessage();
                fm.from = DataUtil.Member;
                fm.to = this.baseRole;

                fm.Path = ofd.FileName;
                fm.FileName = ofd.SafeFileName;
                fm.Size = ofd.OpenFile().Length;
                fm.sendTime = DateTime.Now;
                fm.Guid = Guid.NewGuid().ToString();
               

                DataUtil.Transfer.SendFile(fm);

            }
        }

       

        private void MyMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;

            btn.Width = height;
            btn.Height = Width;
            txtName.FontSize = fontSize;
           
        }

        private void MyMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;
            btn.Width = zoomWidth;
            btn.Height = zoomHeight;
            txtName.FontSize = zoomFontSize;
        }


        public void SetStatus(MemberStatus status)
        {

            Member member=this.baseRole as Member;
            switch (status)
            {
                case MemberStatus.Online:
                    {
                        member.status=MemberStatus.Online;
                        this.baseRole = member;
                        this.Background = new SolidColorBrush(this.onlineColor);
                        break;
                    }

                case MemberStatus.Offline:
                    {
                        member.status = MemberStatus.Offline;
                        this.baseRole = member;
                        this.Background = new SolidColorBrush(this.offlineColor);
                        break;
                    }
            }

        }


        private string GetName(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 2);
        }

    }
}
