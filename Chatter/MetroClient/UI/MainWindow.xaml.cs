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
using System.Windows.Shapes;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color selectedColor = Color.FromArgb(255, 114, 119, 123);
        Grid selectedGrid;
     //   Member[] member = new []Member();
     
        public MainWindow()
        {
            InitializeComponent();
            selectedGrid = btnFriend;
            selectedGrid.Background = new SolidColorBrush(selectedColor);
            MyButton b1 = new MyButton(ButtonType.User, "我的好友", "/MetroClient;component/res/img/default.png", Color.FromArgb(92, 0, 58, 43));
            MyButton b2 = new MyButton(ButtonType.User, "我的好友", "/MetroClient;component/res/img/default.png", Color.FromArgb(92, 0, 58, 43));
            Grid.SetRow(b1,1);
            Grid.SetColumn(b1,1);
            friendGrid.Children.Add(b1);
            Grid.SetRow(b2, 2);
            Grid.SetColumn(b2, 1);
            grid1.Children.Add(b2);
            MiddleGrid.Children.Remove(friendGrid);
            TabControl tab = new TabControl();
           
        }

        

        private void MainWindow_Drag(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void SelectMode_Click(object sender, MouseButtonEventArgs e)
        {

            Grid grid = sender as Grid;
            if (grid != selectedGrid)
                selectedGrid.Background = null;
            selectedGrid = grid;
            grid.Background = new SolidColorBrush(selectedColor);
            
          
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            
            Button btn = sender as Button;
            btn.Width = btn.Width + 15;
            btn.Height = btn.Height + 15;
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Width = btn.Width - 15;
            btn.Height = btn.Height -15;
        }
    }
}
