
using MetroClient.ChatterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chatter.MetroClient.UI
{
    class MyGrid : Grid
    {
        private double columnWidth = 100;
        private double rowHeigth = 100;
        private int rowCount = 0;
        private int columnCount = 3;
        private string imageSouce = "/MetroClient;component/res/img/default.png";
        
        private int[,] colors = new int[,]
        {
                                  



            ///faa755" 
              // #8a5d19"
               //"#d71345
               ///b7ba6b
               {250,167,85},
               {138,93,27},
               {215,147,69},
               {183,186,107},
               {238,130,238},
    {255,69,0},
     {60,179,113},
               ///石板蓝
               {106,90,205},
               ///橙色
               {255,97,0},
             ///沙棕色
            {244,164,0},
            ///暗青色
            { 0, 139, 139 }, 
                              
              ///草绿色
                {124,252,0},
                ///桔黄
                       {255,128,0},
                   ///桔红
                       {255,69,0},
                   ///土耳其玉色
                    {0,199,140},
                              
                   

                                };

        public MyGrid(Friend[] friends)
            : base()
        {
            rowCount = friends.Length / columnCount + 1;
            InitRowAndColumn();

            Random random = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < friends.Length; i++)
            {
                int j = random.Next(colors.Length / 3);
                MyButton button = new MyButton(ButtonType.User, friends[i], imageSouce, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]), i);
                Grid.SetColumn(button, i % columnCount);
                Grid.SetRow(button, i / columnCount);
                this.Children.Add(button);
            }


        }

        public MyGrid(Dictionary<string, string> userGroups)
            : base()
        {
            rowCount = userGroups.Count / columnCount + 1;
            InitRowAndColumn();
            int i = 0;
            Random random = new Random((int)DateTime.Now.Ticks);
            foreach (KeyValuePair<string, string> keyValue in userGroups)
            {
                int j = random.Next(colors.Length / 3);
                MyButton button = new MyButton(ButtonType.UserGroup, keyValue.Value, null, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]), i);
                Grid.SetColumn(button, i % columnCount);
                Grid.SetRow(button, i / columnCount);
                this.Children.Add(button);
                i++;
            }
        }

        private void InitRowAndColumn()
        {
            for (int i = 0; i < columnCount; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            }
            for (int i = 0; i < rowCount; i++)
            {
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowHeigth) });
            }
        }
    }
}
