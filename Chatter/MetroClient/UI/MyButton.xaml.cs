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
    public partial class MyButton :Grid
    {
       
        public MyButton( )
        {
            InitializeComponent();
           
        }
     
       
      

        private void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;
            
          
            btn.Width = btn.Width - 15;
            btn.Height = btn.Height - 15;
          
        }

        private void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid btn = sender as Grid;
            btn.Width = btn.Width + 15;
            btn.Height = btn.Height + 15;
        }
    }
}
