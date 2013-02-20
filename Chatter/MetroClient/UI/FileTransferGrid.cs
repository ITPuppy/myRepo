using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chatter.MetroClient.UI
{
    class FileTransferGrid:Grid
    {

        TextBlock file;
        ProgressBar bar;
        Button saveAsBtn;
        Button cancleBtn;

        public FileTransferGrid(bool isSend,string fileName)
        {


           
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
            column1.Width = new GridLength(100);
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(300);
            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(100);
              ColumnDefinition column4 = new ColumnDefinition();
            column4.Width = new GridLength(100);
            this.ColumnDefinitions.Add(column1);
            this.ColumnDefinitions.Add(column2);
            this.ColumnDefinitions.Add(column3);
            this.ColumnDefinitions.Add(column4);


            file = new TextBlock();
            file.Text = fileName;

            Grid.SetRow(file,1);
            Grid.SetColumn(file,0);
            this.Children.Add(file);


            bar = new ProgressBar();
            bar.Maximum = 100;
            bar.Margin=new Thickness(10,0,10,0);
            Grid.SetRow(bar, 1);
            Grid.SetColumn(bar, 1);
            this.Children.Add(bar);



            saveAsBtn = new Button();
            saveAsBtn.Content = "另存为";
            saveAsBtn.Margin = new Thickness(10, 0, 10, 0);

            cancleBtn = new Button();
            cancleBtn.Content = "取消";
            cancleBtn.Margin = new Thickness(10, 0, 10, 0);

            if (isSend)
            {
                Grid.SetRow(cancleBtn, 1);
                Grid.SetColumn(cancleBtn, 2);
                this.Children.Add(cancleBtn);

            }

            else
            {
                Grid.SetRow(saveAsBtn, 1);
                Grid.SetColumn(saveAsBtn, 2);
                this.Children.Add(saveAsBtn);

                Grid.SetRow(cancleBtn, 1);
                Grid.SetColumn(cancleBtn, 3);
                this.Children.Add(cancleBtn);
            }
        }


    }
}
