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
    /// <summary>
    /// Interaction logic for AddBaseRoleDialog.xaml
    /// </summary>
    public partial class AddBaseRoleDialog : Window
    {
        public AddBaseRoleDialog(MyType type)
        {
            InitializeComponent();
            switch (type)
            {
                case MyType.User:
                    {
                        lbl.Content = "好友ID";
                        break;
                    }
                case MyType.UserGroup:
                    {
                        lbl.Content = "分组名";
                        break;
                    }
                case MyType.Group:
                    {
                        lbl.Content = "群组名";
                        break;
                    }
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public string GetString()
        {
            return txtName.Text.Trim();
            
        }
    }
}
