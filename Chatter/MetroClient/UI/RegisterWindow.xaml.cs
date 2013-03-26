using MetroClient.RegisterService;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        int year = -1;
        public RegisterWindow()
        {
            InitializeComponent();
            for(int i=1980;i<=DateTime.Now.Year;i++)
                cmboYear.Items.Add(i);
            for (int i = 1; i <= 12; i++)
                cmboMonth.Items.Add(i);

            cmboSex.Items.Add("男");
            cmboSex.Items.Add("女");

        }

        private void RegisterWindow_Drag(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

       

        private void YearMonth_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cmboYear.SelectedIndex == -1 || cmboMonth.SelectedIndex == -1)
                return;
            string s=String.Empty;
            if(cmboDay.SelectedItem !=null)
             s = cmboDay.SelectedItem.ToString();
            cmboDay.Items.Clear();
            year = Convert.ToInt32(cmboYear.SelectedItem.ToString());
            string  strI = cmboMonth.SelectedItem.ToString();
            int month = Convert.ToInt32(strI);
            if (month == 1 || month == 3 || month == 5 | month == 7 || month == 8 || month == 10 || month == 12)
            {
                for (int i = 1; i <= 31; i++)
                {
                    cmboDay.Items.Add(i);
                }
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                for (int i = 1; i <= 30; i++)
                {
                    cmboDay.Items.Add(i);
                }
            }
            else if (month == 2)
            {

              
                if (year % 4 == 0 && year % 100 == 0 || year % 400 == 0)
                {
                    for (int i = 1; i <= 29; i++)
                    {
                        cmboDay.Items.Add(i);
                    }
                }
                else
                {
                    for (int i = 1; i <= 28; i++)
                    {
                        cmboDay.Items.Add(i);
                    }
                }

                
            }
            if(s!=null&&s!=string.Empty)
            cmboDay.SelectedIndex = Convert.ToInt32(s)-1;
        }

        private void bntRegister_Click(object sender, RoutedEventArgs e)
        {
            if (txtNickName.Text.Trim() == "")
            {
                MessageBox.Show("昵称不能为空");
                return;

            }
            if (txtPwd.Password.Trim() == "")
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            if (txtPwd.Password.Trim() != txtRepeatPwd.Password.Trim())
            {
                MessageBox.Show("密码不一致");
                return;
            }
            if (cmboSex.SelectedIndex == -1)
            {
                MessageBox.Show("性别不能为空");
                return;
            }

            if (cmboYear.SelectedIndex == -1 || cmboMonth.SelectedIndex == -1 || cmboDay.SelectedIndex == -1)
            {
                MessageBox.Show("请正确填写生日");
                return;
            }

            Member member = new Member();
            member.nickName = txtNickName.Text.Trim();
            member.password = txtPwd.Password.Trim();
            member.sex = cmboSex.SelectedItem.ToString();
            member.birthday = new DateTime(Convert.ToInt32(cmboYear.SelectedItem.ToString()), Convert.ToInt32(cmboMonth.SelectedItem.ToString()),Convert.ToInt32( cmboDay.SelectedItem.ToString()));


            RegisterClient registerClient = new RegisterClient();
            registerClient.RegisterCompleted += registerClient_RegisterCompleted;
            txtNickName.IsEnabled = false;
            txtPwd.IsEnabled = false;
            txtRepeatPwd.IsEnabled = false;
            cmboSex.IsEnabled = false;
            cmboDay.IsEnabled = false;
            cmboMonth.IsEnabled = false;
            cmboYear.IsEnabled = false;
            registerClient.RegisterAsync(member);
            
        }

        void registerClient_RegisterCompleted(object sender, RegisterCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                File.AppendAllText("c:/user_" + e.Result.id + ".txt","账号：" + e.Result.id );
                File.AppendAllText("c:/user_" + e.Result.id + ".txt","密码：" + e.Result.password );
                MessageBox.Show("成功注册，用户信息保存在c:/user_" + e.Result.id + ".txt");

                this.Close();
            }
            else
            {
                MessageBox.Show("注册失败");
                txtNickName.IsEnabled = true;
                txtPwd.IsEnabled = true;
                txtRepeatPwd.IsEnabled = true;
                cmboSex.IsEnabled = true;
                cmboDay.IsEnabled = true;
                cmboMonth.IsEnabled = true;
                cmboYear.IsEnabled = true;
            }

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
