
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
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color selectedColor = Color.FromArgb(255, 114, 119, 123);
        Grid selectedGrid;
       
       
     
        private MyTabControl tabControl;
        public MainWindow()
        {
            InitializeComponent();
            
          
           
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

            if (grid.Name == "btnFriendGrid")
                tabControl.SelectedIndex = 0;
            else if (grid.Name == "btnGroupGrid")
                tabControl.SelectedIndex = 1;
            else if (grid.Name == "btnRecentFriendGrid")
                tabControl.SelectedIndex = 2;
            else if (grid.Name == "btnSetting")
            {
                tabControl.SelectedIndex = 3;
            }
          
        }

     
       
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           

            init();

         
        }

        private void init()
        {
            txtNickName.Text = DataUtil.Member.nickName;
            selectedGrid = btnFriendGrid;
            selectedGrid.Background = new SolidColorBrush(selectedColor);
            DataUtil.UserGroups = DataUtil.Client.GetFriends(DataUtil.Member.id).ToList<UserGroup>();
          tabControl = new MyTabControl();
          Grid.SetRow(tabControl,1);
          MiddleGrid.Children.Add(tabControl);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (DataUtil.Client != null)
                    DataUtil.Client.Logoff(DataUtil.Member);
            }
            catch (Exception ex)
            {

            }
            finally
            {

                Application.Current.Shutdown();
                base.OnClosing(e);
            }
        }
    }
}
