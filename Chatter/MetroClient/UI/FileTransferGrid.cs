using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chatter.MetroClient.TCP;
using MetroClient.ChatterService;
using Microsoft.Win32;
using Chatter.Log;

namespace Chatter.MetroClient.UI
{
   public class FileTransferGrid:Grid
    {

        TextBlock file;
        public ProgressBar bar;
        TextBlock saveAsBtn;
        TextBlock cancleBtn;
        Border saveBorder;
        FileMessage fm;
        private TransferFileUtil transferFileUtil;
        public FileTransferGrid(bool isSend,FileMessage fm)
        {


            this.fm = fm;
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(20); 
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(20); 
            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(20);
            this.RowDefinitions.Add(row1);
            this.RowDefinitions.Add(row2);
            this.RowDefinitions.Add(row3);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(200);
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(100);
            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(100);
              ColumnDefinition column4 = new ColumnDefinition();
            column4.Width = new GridLength(100);
            this.ColumnDefinitions.Add(column1);
            this.ColumnDefinitions.Add(column2);
            this.ColumnDefinitions.Add(column3);
            this.ColumnDefinitions.Add(column4);


            file = new TextBlock();
            file.Text = fm.FileName;
            file.Margin = new Thickness(20,0,0,0);
            Grid.SetRow(file,1);
            Grid.SetColumn(file,0);
            this.Children.Add(file);


            bar = new ProgressBar();
            bar.Maximum = 100;
            bar.Margin=new Thickness(10,0,10,0);
            bar.Height = 10;
            Grid.SetRow(bar, 1);
            Grid.SetColumn(bar, 1);
            this.Children.Add(bar);


           
            saveAsBtn = new TextBlock();
            saveAsBtn.Background = new SolidColorBrush(Colors.Transparent);
            saveAsBtn.Text = "另存为";
            saveAsBtn.TextAlignment = TextAlignment.Center;

             saveBorder = new Border();
            saveBorder.BorderThickness = new Thickness(1);
            saveBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            saveBorder.CornerRadius = new CornerRadius(10, 10, 10, 10);
            saveBorder.Background = new SolidColorBrush(Colors.White);
            saveBorder.Child = saveAsBtn;
            saveBorder.Width = 60;
            saveBorder.MouseLeftButtonDown += saveBorder_MouseLeftButtonDown;
           
           cancleBtn = new TextBlock();
          
            cancleBtn.Background = new SolidColorBrush(Colors.Transparent);
            cancleBtn.TextAlignment = TextAlignment.Center;


            Border cancleBorder = new Border();
            cancleBorder.CornerRadius = new CornerRadius(10, 10, 10, 10);
            cancleBorder.Background = new SolidColorBrush(Colors.White);
            cancleBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            cancleBorder.BorderThickness = new Thickness(1);
            cancleBorder.Child = cancleBtn;
            cancleBtn.MouseLeftButtonDown += cancleBtn_MouseLeftButtonDown;
            cancleBorder.Width = 60;

            if (isSend)
            {
                cancleBtn.Text = "取消";
                Grid.SetRow(cancleBorder, 1);
                Grid.SetColumn(cancleBorder, 2);
                this.Children.Add(cancleBorder);

                this.Loaded += FileTransferGrid_Loaded;
                

            }

            else
            {
                cancleBtn.Text = "拒绝";
                Grid.SetRow(saveBorder, 1);
                Grid.SetColumn(saveBorder, 2);
                this.Children.Add(saveBorder);

                Grid.SetRow(cancleBorder, 1);
                Grid.SetColumn(cancleBorder, 3);
                this.Children.Add(cancleBorder);
            }
        }

        void FileTransferGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataUtil.Client.SendMesg(fm);
        }

      

        void cancleBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (cancleBtn.Text == "拒绝")
            {
                DataUtil.Client.ResponseToSendFile(new Result()
                {
                    Member = fm.from as Member,
                    Status = MessageStatus.Refuse,
                    Type = MessageType.File,
                   
                    Guid = fm.Guid
                });
                RemoveMySelf();

            }
            else if (cancleBtn.Text == "取消")
            {
                //已经开始传送
                if (transferFileUtil != null)
                {
                    transferFileUtil.Cancle();
                }
                 ///还没有开始
                else
                {
                    CancelSend();
                }
            }
            else if (cancleBtn.Text == "移除")
            {
                RemoveMySelf();
            }

        }

        private void CancelSend()
        {
            CompletSend(TransferState.CanceledByMyself);
            CommandMessage cm = new CommandMessage();
            cm.from = DataUtil.Member;
            cm.to = this.fm.to;
            cm.CommandType = MyCommandType.Canceled;
            cm.Guid = fm.Guid;
            DataUtil.Client.SendMesg(cm);
        }

        private void CancelReceive()
        {
            CompletReceive(TransferState.CanceledByMyself);
            CommandMessage cm = new CommandMessage();
            cm.from = DataUtil.Member;
            cm.to = this.fm.to;
            cm.CommandType = MyCommandType.Canceled;
            cm.Guid = fm.Guid;
            DataUtil.Client.SendMesg(cm);
        }

        private void RemoveMySelf()
        {
            StackPanel sp = this.Parent as StackPanel;
            ScrollViewer scrollViewer = sp.Parent as ScrollViewer;
            Border border = scrollViewer.Parent as Border;
            TransferFileWindow tfw = border.Parent as TransferFileWindow;
            tfw.Remove(fm.Guid);
        }

        void saveBorder_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            saveBorder.Visibility = Visibility.Hidden;
            cancleBtn.Text = "取消";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = fm.FileName;
            sfd.RestoreDirectory = true;
            sfd.ShowDialog();
          
            fm.Path = sfd.FileName;
            fm.Path = fm.Path.Remove(fm.Path.LastIndexOf(sfd.SafeFileName));
            fm.Path = fm.Path + fm.Guid;
            fm.FileName = sfd.SafeFileName;
            transferFileUtil = new ReceiveFileUtil(fm, this);

            int port = ((ReceiveFileUtil)transferFileUtil).initTcpHost();
            if (port == -1)
            {
                MyLogger.Logger.Error("文件取消接收");
                CancelReceive();
                return;
            }

            transferFileUtil.Transfer();

            DataUtil.Client.ResponseToSendFile(new Result()
            {
                Member = fm.from as Member,
                Status = MessageStatus.Accept,
                Type = MessageType.File,
                EndPoint = new MyEndPoint() { Address = fm.EndPoint.Address, Port = port },
                Guid=fm.Guid
            });
            
            

                   
            }

        public void BeginSendFile(MyEndPoint endpoint)
        {
            fm.EndPoint = endpoint;
             transferFileUtil = new SendFileUtil(fm,this);

             transferFileUtil.Transfer();
        }


        internal void CompletReceive(TransferState transferState)
        {
            this.Children.Remove(bar);
            TextBlock txt = new TextBlock();
            if (transferState == TransferState.Running)
                txt.Text = "已接收文件";
            else if (transferState == TransferState.CanceledByMyself)
                txt.Text = "已取消接收";
            else if (transferState == TransferState.CanceledByTheOther)
                txt.Text = "对方已取消发送";
            Grid.SetRow(txt, 1);
            Grid.SetColumn(txt,1);
            this.Children.Add(txt);
            this.cancleBtn.Text = "移除";
        }

        internal void CompletSend(TransferState transferState)
        {
            this.Children.Remove(bar);
            TextBlock txt = new TextBlock();
            if (transferState == TransferState.Running)
                txt.Text = "已发送文件";
            else if (transferState == TransferState.CanceledByMyself)
                txt.Text = "已取消发送";
            else if (transferState == TransferState.CanceledByTheOther)
                txt.Text = "对方已取消接收";
            else if (transferState == TransferState.RefusedByTheOther)
                txt.Text = "对方拒绝接收";
            //else if(transferState==TransferState)
            Grid.SetRow(txt, 1);
            Grid.SetColumn(txt, 1);
            this.Children.Add(txt);
            this.cancleBtn.Text = "移除";
        }

        internal void TheOtherCancel(bool isSend)
        {
            if (!isSend)
                CompletReceive(TransferState.CanceledByTheOther);
            else
                CompletSend(TransferState.RefusedByTheOther);
            
            saveBorder.Visibility = Visibility.Collapsed;
        }
    }
}
