using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for SendFileWindow.xaml
    /// </summary>
    public partial class TransferFileWindow : Window
    {
        Grid grid = new Grid();

        public TransferFileWindow()
        {
            InitializeComponent();
            this.AddChild(grid);
            grid.RowDefinitions.Add(new RowDefinition());
           
        }


        


        public void SendFile(string path, BaseRole fromRole, BaseRole toRole)
        {

            grid.Children.Add(new FileTransferGrid(true,"asdfasdf"));

        }

        



    }
}
