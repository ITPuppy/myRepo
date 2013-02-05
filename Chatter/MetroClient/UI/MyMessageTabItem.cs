using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
     private StackPanel sp;
     private string imageSouce = "/MetroClient;component/res/img/default.png";
     private double imageSize = 35;
     private MyType type;
     private BaseRole role;

        public MyMessageTabItem(MyType type, BaseRole role)
        {
            this.type = type;
            this.role = role;

            switch (type)
            {
                case MyType.User:
                    {

                        Member member = role as Member;

                        RowDefinition row1 = new RowDefinition();
                        row1.Height = new GridLength(40);
                        RowDefinition row2 = new RowDefinition();

                        grid.RowDefinitions.Add(row1);
                        grid.RowDefinitions.Add(row2);




                        this.rtxtBox.Background = new SolidColorBrush(Colors.Transparent);
                        rtxtBox.IsReadOnly = true;
                        rtxtBox.FontSize = 17;
                        rtxtBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        rtxtBox.Foreground = new SolidColorBrush(Colors.White);
                        rtxtBox.Document.LineHeight = 0.1;
                        rtxtBox.Document.Blocks.Clear();


                        Grid.SetRow(rtxtBox, 1);
                        grid.Children.Add(rtxtBox);



                        sp = new StackPanel();
                        sp.Orientation = Orientation.Horizontal;

                        image = new Image();
                        image.Source = new BitmapImage(new Uri(imageSouce, UriKind.Relative));
                        image.Height = imageSize;
                        image.Width = imageSize;
                        Grid.SetRow(image, 0);


                        TextBlock txtName = new TextBlock();
                        txtName.Text = member.nickName;
                        txtName.FontSize = 25;
                        txtName.Foreground = new SolidColorBrush(Colors.SandyBrown);


                        sp.Children.Add(image);
                        sp.Children.Add(txtName);
                        sp.Margin = new Thickness(10, 0, 0, 10);
                        Grid.SetRow(sp, 0);
                        grid.Children.Add(sp);

                        this.AddChild(grid);
                        

                        break;
                    }
            }



           
        }

       

        public void SendMesg(string mesg)
        {

            if (this.role is Member)
            {
               


                Paragraph pa = new Paragraph();



                string nickName = DataUtil.Member.nickName + "   " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
                pa.Inlines.Add(new  Run(nickName) {FontSize=20, Foreground = new SolidColorBrush(Colors.Wheat), FontFamily = new FontFamily("Avenir Book")});

               
              

                pa.Inlines.Add(new Run(mesg));
                rtxtBox.Document.Blocks.Add(pa);
                pa.LineStackingStrategy = LineStackingStrategy.MaxHeight;
                rtxtBox.ScrollToEnd();



                TextMessage msg = new TextMessage();

                msg.from = DataUtil.Member;
                msg.to = role;
                msg.sendTime = DateTime.Now;
                msg.type = MessageType.TextMessage;
                msg.msg = mesg;


                var status=DataUtil.Client.SendMesg(msg);
                if (status == MessageStatus.Failed)
                {

                }
                else if (status == MessageStatus.Refuse)
                {
                    MessageBox.Show("您不是对方的好友，不可以给对方发消息，你可以先删除该好友然后添加");
                }
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
