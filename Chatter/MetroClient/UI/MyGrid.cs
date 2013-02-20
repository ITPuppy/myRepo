
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
                rowCount = friends.Count / columnCount + 1;
                InitRowAndColumn();


               
                for (i = 0; i < friends.Count; i++)
                {
                   
                    MyButton button = new MyButton(MyType.User, friends[i], imageSouce,userGroupId);
                    Grid.SetColumn(button, i % columnCount);
                    Grid.SetRow(button, i / columnCount);
                    this.Children.Add(button);


                    var tabItem=new MyMessageTabItem(MyType.User,friends[i]);
                       DataUtil.MessageTabControl.Items.Add(tabItem);
                       DataUtil.MessageTabItems.Add(friends[i].id, tabItem);
                }


             


            }
            else if (type == MyType.UserGroup)
            {

                List<UserGroup> userGroups = DataUtil.UserGroups;
                rowCount = userGroups.Count / columnCount + 1;
                InitRowAndColumn();




                for (i = 0; i < userGroups.Count; i++)
                {
                   
                    MyButton button = new MyButton(MyType.UserGroup, userGroups[i], null);
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
           
             

                UserGroup userGroup = role as UserGroup;
                MyButton button = new MyButton(MyType.UserGroup, userGroup, null);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);
               // DataUtil.UserGroups.Add(userGroup);

            }

            else if (type == MyType.User)
            {
                int index = DataUtil.GetMemberList(userGroupId).Count;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();

                Member member = role as Member;
                Random random = new Random((int)DateTime.Now.Ticks);

              
                MyButton button = new MyButton(MyType.User, member, imageSouce, userGroupId);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);


               // DataUtil.AddMemberTo(member,userGroupId);
            }


        }

        public MyButton GetButton(MyType myType, string id)
        {
            MyButton button = null;
            switch (myType)
            {
                case MyType.User:
                    {
                        foreach(MyButton btn in  this.Children)
                        {
                            if ((btn.baseRole as Member).id == id)
                            {
                                button = btn;
                                break;
                            }
                        }
                        break;
                    }
                case MyType.UserGroup:
                    {
                        foreach (MyButton btn in this.Children)
                        {
                            if ((btn.baseRole as UserGroup).userGroupId == id)
                            {
                                button = btn;
                                break;
                            }
                        }
                        break;
                    }

                   
            }

            return button;
        }

    }
}
