
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
    class MyGrid:Grid
    {
        private double columnWidth = 100;
        private double rowHeigth = 100;
        private int rowCount=0;
        private int columnCount = 3;
        private string imageSouce="/Metro;component/res/img/default.png";
         

        public MyGrid(Friend [] friends)
            : base()
        {
            rowCount = friends.Length / columnCount + 1;
            InitRowAndColumn();

            for (int i = 0; i < friends.Length; i++)
            {
                MyButton button = new MyButton(ButtonType.User,friends[i],imageSouce,Colors.Green);
                Grid.SetColumn(button,i%columnCount);
                Grid.SetRow(button,i/columnCount);
                this.Children.Add(button);
            }


        }

        public MyGrid(Dictionary<string,string> userGroups)
            : base()
        {
            rowCount = userGroups.Count / columnCount + 1;
            InitRowAndColumn();
            int i=0;
            foreach(KeyValuePair<string,string> keyValue in userGroups)
            {
                 MyButton button = new MyButton(ButtonType.UserGroup,keyValue.Value,null,Colors.Green);
                Grid.SetColumn(button,i%columnCount);
                Grid.SetRow(button,i/columnCount);
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
