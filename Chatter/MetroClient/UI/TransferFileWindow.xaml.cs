using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Chatter.MetroClient.Sound;
using Chatter.MetroClient.TCP;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    /// <summary>
    /// Interaction logic for SendFileWindow.xaml
    /// </summary>
    public partial class TransferFileWindow : Window
    {


      public  Dictionary<string, FileTransferGrid> transferTask = new Dictionary<string, FileTransferGrid>();
      public List<string> runningTask = new List<string>();
        int count = 0;
        StackPanel sp = new StackPanel();
        Border border = new Border();
        public TransferFileWindow()
        {
            InitializeComponent();

            sp.CanVerticallyScroll = true;
           
             ScrollViewer sv=   new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
             sv.Content = sp;

             border.BorderBrush = new SolidColorBrush(Colors.White);
             border.BorderThickness = new Thickness(1);
             border.Child = sv;
           
            grid.Children.Add(border);

           
            this.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(Move), true);
            this.minBtn.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(minBtn_Click), true);

        }


        

        /// <summary>
        /// 初始化界面，准备发送
        /// </summary>
        /// <param name="fm"></param>
        public void SendFile( FileMessage fm)
        {
            
            var sendFile=new FileTransferGrid(true, fm);
           
            sp.Children.Add(sendFile);
             transferTask.Add(fm.Guid,sendFile);
            count++;

        }
        /// <summary>
        /// 初始化界面，准备接收
        /// </summary>
        /// <param name="fm"></param>
        public void ReceiveFile( FileMessage fm)
        {
           
            var receiveFile = new FileTransferGrid(false, fm);
          
            sp.Children.Add(receiveFile);
            transferTask.Add(fm.Guid, receiveFile);
            count++;
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
          
            if (e.ButtonState == MouseButtonState.Pressed)
            {

                base.DragMove();
              

            }
        }




        internal void Remove(string guid )
        {
            sp.Children.Remove(this.transferTask[guid]);
            transferTask.Remove(guid);
            count--;

            if (count <= 0)
                this.Close();
        }


        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
 