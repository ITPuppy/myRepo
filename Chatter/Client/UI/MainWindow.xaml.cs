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
using System.ServiceModel;
using Client.ChatterService;
using Chatter.Client.Callback;

namespace Chatter.Client.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private ChatterClient client = null;

        public MainWindow()
        {
            InitializeComponent();
        }

       

        private void DragForm(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.Length == 0)
            {
                MessageBox.Show("请先输入用户名");
                return;
            }
            else if (txtPwd.Password.Length == 0)
            {
                MessageBox.Show("请先输入密码");
                return;
            }
            Member member = new Member();
            member.id = txtId.Text;
            member.password = txtPwd.Password;

            InstanceContext context = new InstanceContext(new ChatterCallback());
            client = new ChatterClient(context);
            client.LoginCompleted += client_LoginCompleted;
            client.LoginAsync(member);

        }

        void client_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Result == MessageStatus.OK)
            {
                MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show("登录失败");
            }
            client.LoginCompleted -= client_LoginCompleted;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtId.Focus();
        }
    }
}
