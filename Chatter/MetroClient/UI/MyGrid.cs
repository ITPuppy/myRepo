
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
    public class MyGrid : Grid
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
        private string userGroupId;



        public MyGrid(MyType type, string userGroupId = "-1")
            : base()
        {

            this.userGroupId = userGroupId;

            int i = 0;
            Random random = new Random((int)DateTime.Now.Ticks);
            if (type == MyType.User)
            {


                List<Member> friends = DataUtil.GetMemberList(userGroupId);
                InitRowAndColumn();


                rowCount = friends.Count / columnCount + 1;
                for (i = 0; i < friends.Count; i++)
                {
                    int j = random.Next(colors.Length / 3);
                    MyButton button = new MyButton(MyType.User, friends[i], imageSouce, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]),userGroupId);
                    Grid.SetColumn(button, i % columnCount);
                    Grid.SetRow(button, i / columnCount);
                    this.Children.Add(button);
                }
            }
            else if (type == MyType.UserGroup)
            {

                List<UserGroup> userGroups = DataUtil.UserGroups;
                rowCount = userGroups.Count / columnCount + 1;
                InitRowAndColumn();




                for (i = 0; i < userGroups.Count; i++)
                {
                    int j = random.Next(colors.Length / 3);
                    MyButton button = new MyButton(MyType.UserGroup, userGroups[i], null, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]));
                    Grid.SetColumn(button, i % columnCount);
                    Grid.SetRow(button, i / columnCount);
                    this.Children.Add(button);

                }
            }




        }




        /// <summary>
        /// 初始化行列
        /// 根据内容的多少，创建行列
        /// </summary
        private void InitRowAndColumn()
        {

            while (this.ColumnDefinitions.Count < columnCount)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(columnWidth) });
            }
            while (this.RowDefinitions.Count < rowCount + 1)
            {
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(rowHeigth) });
            }
        }

        /// <summary>
        /// 添加MyButton
        /// </summary>
        /// <param name="type">Button的类型</param>
        /// <param name="role">Member或者UserGroup 或者Group</param>
        public void 
            AddButton(MyType type, BaseRole role)
        {
            
            if (type == MyType.UserGroup)
            {
                int index = DataUtil.UserGroups.Count ;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();
                Random random = new Random((int)DateTime.Now.Ticks);
                int j = random.Next(colors.Length / 3);

                UserGroup userGroup = role as UserGroup;
                MyButton button = new MyButton(MyType.UserGroup, userGroup, null, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]));
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);
                DataUtil.UserGroups.Add(userGroup);

            }

            else if (type == MyType.User)
            {
                int index = DataUtil.GetMemberList(userGroupId).Count;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();

                Member member = role as Member;
                Random random = new Random((int)DateTime.Now.Ticks);

                int j = random.Next(colors.Length / 3);
                MyButton button = new MyButton(MyType.User, member, imageSouce, Color.FromArgb(255, (byte)colors[j, 0], (byte)colors[j, 1], (byte)colors[j, 2]),userGroupId);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);


                DataUtil.AddMemberTo(member,userGroupId);
            }


        }
    }
}
