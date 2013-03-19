using System;
using System.Windows;
using System.Windows.Input;
using System.ServiceModel;
using MetroClient.ChatterService;
using Chatter.MetroClient.Callback;
using System.IO;
using Chatter.Log;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using Chatter.MetroClient.Sound;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {


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

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler); 
           
        }

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            MyLogger.Logger.Error("Unhandled Exception", e.ExceptionObject as Exception);
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
            DataUtil.Client = new ChatterClient(context);
            begin++;
            IsCurrentCanceled = false;

            DataUtil.Client.GetFriends(member.id);

            DataUtil.Client.LoginCompleted += client_LoginCompleted;
            DataUtil.Client.LoginAsync(member);

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
                if (e.Result.Status == MessageStatus.OK)
                {
                    DataUtil.Member = e.Result.Member;

                    this.IsVisibleChanged += LoginWindow_IsVisibleChanged;

                    this.Visibility = Visibility.Collapsed;
                   
                    

                }
                else
                {
                    MessageBox.Show("用户名或者密码错误");
                    WaitGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                }

                WriteIDAndPwd();
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
                DataUtil.Client.LoginCompleted -= client_LoginCompleted;
            }
        }

        void LoginWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                mainWindow.init();
            }
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            WaitGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;

            
            IsCurrentCanceled = true;
            DataUtil.Client.Abort();
        }

        private void LoginForm_Loaded(object sender, RoutedEventArgs e)
        {
           
             LoadIDAndPwd(); 
        }
        private void WriteIDAndPwd()
        {
            FileStream fs = null;
            string path = DataUtil.Path + "\\Netalk\\data.db";
            try
            {
                if ((bool)cbSavePwd.IsChecked)
                {

                    if (!Directory.Exists(DataUtil.Path))
                        Directory.CreateDirectory(path);
                    if (!Directory.Exists(path.Substring(0, path.LastIndexOf("\\"))))
                        Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }



                    fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

                    StringBuilder data = new StringBuilder();
                    data.Append(member.id + "_" + member.password + "_");
                    bool isAutoLogin = false;
                    if ((bool)cbAutoLogin.IsChecked)
                    {
                        isAutoLogin = true;
                    }
                    else
                    {
                        isAutoLogin = false;
                    }
                    data.Append(isAutoLogin.ToString());

                    byte[] byteArray = Encrypt(Encoding.ASCII.GetBytes(data.ToString()), "netalk24");

                    fs.Write(byteArray, 0, byteArray.Length);

                    fs.Flush();


                }
                else
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Warn("储存密码出错", ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        private void LoadIDAndPwd()
        {
            string path = DataUtil.Path + "\\Netalk\\data.db";
            FileStream fs = null;
            if (!File.Exists(path))
            {
                return;
            }
            try
            {

                fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                byte[] byteArray = new byte[fs.Length];

                int n = fs.Read(byteArray, 0, byteArray.Length);
                fs.Close();

                string text = Encoding.ASCII.GetString(Decrypt(byteArray, "netalk24"));
                string[] s = text.Split('_');
                txtId.Text = s[0];
                txtPwd.Password = s[1];
                cbAutoLogin.IsChecked = Boolean.Parse(s[2]);
                cbSavePwd.IsChecked = true;
                txtId.SelectionStart = txtId.Text.Length ;
                this.txtId.Focus();
                if ((bool)cbAutoLogin.IsChecked)
                {
                    this.btnLogin_Click(btnLogin, null);
                }
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Warn("创建信息文件出错", ex);

                File.Delete(path);
            }

        }

        private byte[] Encrypt(byte[] sourcebytes, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = sourcebytes;
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return memStream.ToArray();
        }

        private byte[] Decrypt(byte[] encryptBytes, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = encryptBytes;
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            return memStream.ToArray();
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




        private void cbAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            cbSavePwd.IsChecked = true;

        }

        private void cbSavePwd_UnChecked(object sender, RoutedEventArgs e)
        {
            cbAutoLogin.IsChecked = false;
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtPwd.Focus();
                txtPwd.SelectAll();
            }
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(btnLogin, e);
            }
        }


    }
}
