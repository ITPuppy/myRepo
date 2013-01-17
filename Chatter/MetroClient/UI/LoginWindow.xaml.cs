using System;
using System.Windows;
using System.Windows.Input;
using System.ServiceModel;
using MetroClient.ChatterService;
using Chatter.MetroClient.Callback;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {

        private ChatterClient client = null;
        /// <summary>
        /// 当前调用是否取消
        /// </summary>
        private bool IsCurrentCanceled = false;
        /// <summary>
        /// 异步调用的次数
        /// </summary>
        private int begin = 0;
        /// <summary>
        /// 异步调用后回调的次数
        /// </summary>
        private int end = 0;
        private Member member = new Member();

        public LoginWindow()
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

            member.id = txtId.Text;
            member.password = txtPwd.Password;

            InstanceContext context = new InstanceContext(new ChatterCallback());
            client = new ChatterClient(context);
            begin++;
            IsCurrentCanceled = false;
            client.LoginCompleted += client_LoginCompleted;
            client.LoginAsync(member);

            LoginGrid.Visibility = Visibility.Collapsed;
            WaitGrid.Visibility = Visibility.Visible;

        }



        void client_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            ///调用结束标志加1
            end++;
            ///如果begin！=end说明 此次end已经被取消，如果等于end说明此次是最后一次调用的回调函数，再去判断IsCancel
            if (begin != end || IsCurrentCanceled)
            {

                return;
            }

            try
            {
                if (e.Error != null)
                    throw (e.Error);
                if (e.Result.status == MessageStatus.OK)
                {
                    MainWindow mainWindow = new MainWindow(client, e.Result.member);
                    mainWindow.Show();
                    this.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("用户名或者密码错误");
                    WaitGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                }

            }

            catch (EndpointNotFoundException ex)
            {

                MessageBox.Show("网络出现问题");
                WaitGrid.Visibility = Visibility.Collapsed;
                LoginGrid.Visibility = Visibility.Visible;

                return;
            }

            catch (Exception ex)
            {

                MessageBox.Show("登录出现问题");
                WaitGrid.Visibility = Visibility.Collapsed;
                LoginGrid.Visibility = Visibility.Visible;

                return;
            }
            finally
            {
                client.LoginCompleted -= client_LoginCompleted;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;

            IsCurrentCanceled = true;
        }

        private void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtId.Focus();
        }

        private void btnCancel_Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       

        private void register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.ShowDialog();
        }


    }
}
