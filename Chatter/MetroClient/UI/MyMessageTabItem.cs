using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    public class MyMessageTabItem:TabItem
    {

     private   RichTextBox rtxtBox = new RichTextBox();
     private Grid grid=new Grid();
     private Image image;
     private Grid menuGrid;
     private string imageSouce = "/MetroClient;component/res/img/default.png";
     private double imageSize = 35;
     private MyType type;
     public BaseRole role;
     private TextBox txtInput;
     public MyMenu sendFileMenu;
        public MyMessageTabItem(MyType type, BaseRole role)
        {
            this.type = type;
            this.role = role;

            switch (type)
            {
                case MyType.User:
                    {

                        Member member = role as Member;
                        sendFileMenu = new MyMenu(role,"发送文件");
                        RowDefinition row1 = new RowDefinition();
                        row1.Height = new GridLength(50);
                        RowDefinition row2 = new RowDefinition();
                        RowDefinition row3 = new RowDefinition();
                        RowDefinition row4 = new RowDefinition();
                        row4.Height = new GridLength(10);
                        row3.Height = new GridLength(80);
                        grid.RowDefinitions.Add(row1);
                        grid.RowDefinitions.Add(row2);
                        grid.RowDefinitions.Add(row3);
                        grid.RowDefinitions.Add(row4);


                        this.rtxtBox.Background = new SolidColorBrush(Colors.Transparent);
                        rtxtBox.IsReadOnly = true;
                        rtxtBox.FontSize = 17;
                        rtxtBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        rtxtBox.Foreground = new SolidColorBrush(Colors.White);
                        rtxtBox.Document.LineHeight = 0.1;
                        rtxtBox.Document.Blocks.Clear();
                        


                        Grid.SetRow(rtxtBox, 1);
                        grid.Children.Add(rtxtBox);



                        menuGrid = new Grid();
                        ColumnDefinition column1 = new ColumnDefinition();
                        column1.Width = new GridLength(40);
                        ColumnDefinition column2 = new ColumnDefinition();
                        column2.Width = new GridLength(200);
                        ColumnDefinition column3 = new ColumnDefinition();
                        column3.Width = new GridLength(40);
                        ColumnDefinition column4 = new ColumnDefinition();
                        column4.Width = new GridLength(40);
                        ColumnDefinition column5 = new ColumnDefinition();
                        column5.Width = new GridLength(40);

                        menuGrid.ColumnDefinitions.Add(column1);
                        menuGrid.ColumnDefinitions.Add(column2);
                        menuGrid.ColumnDefinitions.Add(column3);
                        menuGrid.ColumnDefinitions.Add(column4);
                        menuGrid.ColumnDefinitions.Add(column5);


                        image = new Image();
                        image.Source = new BitmapImage(new Uri(imageSouce, UriKind.Relative));
                        image.Height = imageSize;
                        image.Width = imageSize;
                        Grid.SetRow(image, 0);


                        TextBlock txtName = new TextBlock();
                        txtName.Text = member.nickName;
                        txtName.FontSize = 25;
                        txtName.Foreground = new SolidColorBrush(Colors.SandyBrown);





                        Grid.SetColumn(image, 0);
                        menuGrid.Children.Add(image);
                        Grid.SetColumn(txtName, 1);
                        menuGrid.Children.Add(txtName);
                        Grid.SetColumn(sendFileMenu, 2);
                        menuGrid.Children.Add(sendFileMenu);
                       



                        Grid.SetRow(menuGrid, 0);
                        grid.Children.Add(menuGrid);



                        txtInput = new TextBox();
                        txtInput.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        txtInput.KeyDown += txtInput_KeyDown;
                        txtInput.FontSize = 15;
                        txtInput.TextWrapping = TextWrapping.Wrap;


                        Border border = new Border();
                        border.BorderThickness = new Thickness(2);
                        border.BorderBrush = new SolidColorBrush(Colors.SkyBlue);

                        border.Child = txtInput;


                        Grid.SetRow(border,2);
                        grid.Children.Add(border);

                        this.AddChild(grid);
                        

                        break;
                    }
            }



           
        }

        void txtInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
          
            MyMessageTabItem item= DataUtil.MessageTabControl.SelectedItem as MyMessageTabItem;

            if (item == null)
            {
                return;
            }
            if (DataUtil.CurrentRole == null)
            {
                DataUtil.CurrentMessageTabItem = DataUtil.MessageTabControl.SelectedItem as MyMessageTabItem;
                if (DataUtil.CurrentMessageTabItem != null)
                {
                    DataUtil.CurrentRole = DataUtil.CurrentMessageTabItem.role;
                }
                else
                    return;
            }

            if (DataUtil.CurrentRole is Member)
            {
                Member member = DataUtil.CurrentRole as Member;
                if (member.status == MemberStatus.Offline)
                {
                    return;
                }
            }
           

            if (e.Key == Key.Enter)
            {
                if (!Keyboard.IsKeyDown(Key.LeftShift))
                {
                    if (txtInput.Text.Trim() != String.Empty)
                    {
                        item.SendMesg(txtInput.Text);

                        txtInput.Text = "";
                      
                    }
                }
                else
                {
                    txtInput.AppendText(Environment.NewLine);
                    txtInput.SelectionStart = txtInput.Text.Length;
                }
            }
        
        }

       

        public void SendMesg(string mesg)
        {

            if (this.role is Member)
            {

                TextMessage msg = new TextMessage();

                msg.from = DataUtil.Member;
                msg.to = role;
                msg.sendTime = DateTime.Now;
                msg.type = MessageType.TextMessage;
                msg.msg = mesg;


                var status = DataUtil.Client.SendMesg(msg);
                if (status == MessageStatus.Failed)
                {

                }
                else if (status == MessageStatus.Refuse)
                {
                    MessageBox.Show("您不是对方的好友，不可以给对方发消息，你可以先删除该好友然后添加");
                    return;
                }



                Paragraph pa = new Paragraph();



                string nickName = DataUtil.Member.nickName + "   " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
                pa.Inlines.Add(new  Run(nickName) {FontSize=20, Foreground = new SolidColorBrush(Colors.Wheat), FontFamily = new FontFamily("Avenir Book")});

               
              

                pa.Inlines.Add(new Run(mesg));
                rtxtBox.Document.Blocks.Add(pa);
                pa.LineStackingStrategy = LineStackingStrategy.MaxHeight;
                rtxtBox.ScrollToEnd();



               

               
            }
        }


        public void ReceiveMessage(Message mesg)
        {


            if (mesg.from is Member)
            {


                if (mesg is TextMessage)
                {
                    TextMessage tMsg = mesg as TextMessage;

                    Member m = mesg.from as Member;

                    Paragraph pa = new Paragraph();



                    string nickName = m.nickName + "   " + mesg.sendTime.ToLongTimeString() + Environment.NewLine;
                    pa.Inlines.Add(new Run(nickName) {FontSize=20, Foreground = new SolidColorBrush(Colors.Tomato), FontFamily = new FontFamily("Avenir Book") });

                    pa.Inlines.Add(new Run(tMsg.msg));
                    rtxtBox.Document.Blocks.Add(pa);

                    rtxtBox.ScrollToEnd();



                 
                }
            }
        }
    }
}
