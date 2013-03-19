
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


        private string baseRoleId;




        public MyGrid(MyType type, string baseRoleuserGroupId = "-1")
            : base()
        {

            this.baseRoleId = baseRoleuserGroupId;

            int i = 0;
            Random random = new Random((int)DateTime.Now.Ticks);

            switch (type)
            {
                case MyType.User:
                    {


                        List<Member> friends = DataUtil.GetMemberList(baseRoleuserGroupId);
                        rowCount = friends.Count / columnCount + 1;
                        InitRowAndColumn();



                        for (i = 0; i < friends.Count; i++)
                        {

                            MyButton button = new MyButton(MyType.User, friends[i], imageSouce, baseRoleuserGroupId);
                            Grid.SetColumn(button, i % columnCount);
                            Grid.SetRow(button, i / columnCount);
                            this.Children.Add(button);


                            var tabItem = new MyMessageTabItem(MyType.User, friends[i]);
                            DataUtil.MessageTabControl.Items.Add(tabItem);
                            DataUtil.FriendMessageTabItems.Add(friends[i].id, tabItem);
                        }



                        break;

                    }
                case MyType.UserGroup:
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
                        break;
                    }

                case MyType.UserInGroup:
                    {
                        List<Member> friends = DataUtil.GetGroupMemberList(baseRoleuserGroupId);
                        rowCount = friends.Count / columnCount + 1;
                        InitRowAndColumn();



                        for (i = 0; i < friends.Count; i++)
                        {

                            MyButton button = new MyButton(MyType.UserInGroup, friends[i], imageSouce, baseRoleuserGroupId);
                            Grid.SetColumn(button, i % columnCount);
                            Grid.SetRow(button, i / columnCount);
                            this.Children.Add(button);


                            //var tabItem = new MyMessageTabItem(MyType.UserInGroup, friends[i]);
                            //DataUtil.MessageTabControl.Items.Add(tabItem);
                            //DataUtil.MessageTabItems.Add(friends[i].id, tabItem);
                        }

                        break;

                    }

                case MyType.Group:
                    {
                        List<Group> groups = DataUtil.Groups;
                        rowCount = groups.Count / columnCount + 1;
                        InitRowAndColumn();




                        for (i = 0; i < groups.Count; i++)
                        {

                            MyButton button = new MyButton(MyType.Group, groups[i], null);
                            Grid.SetColumn(button, i % columnCount);
                            Grid.SetRow(button, i / columnCount);
                            this.Children.Add(button);


                            var tabItem = new MyMessageTabItem(MyType.Group, groups[i]);
                            DataUtil.MessageTabControl.Items.Add(tabItem);
                            DataUtil.GroupMessageTabItems.Add(groups[i].GroupId, tabItem);

                        }

                        break;
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
                int index = DataUtil.UserGroups.Count;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();




                MyButton button = new MyButton(MyType.UserGroup, role, null);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);
                // DataUtil.UserGroups.Add(userGroup);

            }

            else if (type == MyType.User)
            {
                int index = DataUtil.GetMemberList(baseRoleId).Count;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();


                Random random = new Random((int)DateTime.Now.Ticks);


                MyButton button = new MyButton(MyType.User, role, imageSouce, baseRoleId);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);


                // DataUtil.AddMemberTo(member,userGroupId);
            }

            else if (type == MyType.Group)
            {
                int index = DataUtil.Groups.Count - 1;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();


                Random random = new Random((int)DateTime.Now.Ticks);


                MyButton button = new MyButton(MyType.Group, role, imageSouce);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);


            }
            else if (type == MyType.UserInGroup)
            {
                int index = DataUtil.GetGroupMemberList(baseRoleId).Count;

                rowCount = (index) / columnCount + 1;
                InitRowAndColumn();


                Random random = new Random((int)DateTime.Now.Ticks);


                MyButton button = new MyButton(MyType.UserInGroup, role, imageSouce, baseRoleId);
                Grid.SetColumn(button, index % columnCount);
                Grid.SetRow(button, index / columnCount);
                this.Children.Add(button);
            }


        }




        public void RemoveButton(MyType type, MyButton btn)
        {


            switch (type)
            {
                case MyType.User:
                    {
                        #region 好友
                        Member member = btn.baseRole as Member;

                        int currentIndex = this.Children.IndexOf(btn);

                        ///删掉分组
                        this.Children.Remove(btn);

                        ///删除全局的分组记录
                        DataUtil.DeleteFriend(member.id, this.baseRoleId);

                        ///将后面的分组移除
                        List<MyButton> temp = new List<MyButton>();
                        for (; currentIndex < this.Children.Count; )
                        {
                            temp.Add(this.Children[currentIndex] as MyButton);
                            this.Children.RemoveAt(currentIndex);

                        }
                        ///将后面的分组前移后加上
                        foreach (MyButton button in temp)
                        {
                            Grid.SetRow(button, currentIndex / 3);
                            Grid.SetColumn(button, currentIndex % 3);
                            this.Children.Add(button);
                            currentIndex++;
                        }


                        ///将message windows 删掉
                        DataUtil.MessageTabControl.Items.Remove(DataUtil.FriendMessageTabItems[member.id]);
                        DataUtil.FriendMessageTabItems.Remove(member.id);
                        break;
                        #endregion
                    }

                case MyType.UserInGroup:
                    {
                        #region 群组成员
                        Member member = btn.baseRole as Member;



                        int currentIndex = this.Children.IndexOf(btn);

                        ///删掉成员
                        this.Children.Remove(btn);

                        ///删除组中的成员记录
                        DataUtil.DeleteMemberFromGroup(member.id, this.baseRoleId);

                        ///将后面的成员移除
                        List<MyButton> temp = new List<MyButton>();
                        for (; currentIndex < this.Children.Count; )
                        {
                            temp.Add(this.Children[currentIndex] as MyButton);
                            this.Children.RemoveAt(currentIndex);

                        }
                        ///将后面的成员前移后加上
                        foreach (MyButton button in temp)
                        {
                            Grid.SetRow(button, currentIndex / 3);
                            Grid.SetColumn(button, currentIndex % 3);
                            this.Children.Add(button);
                            currentIndex++;
                        }




                        break;
                        #endregion
                    }

                case MyType.UserGroup:
                    {
                        #region 分组
                        UserGroup userGroup = btn.baseRole as UserGroup;
                        ///将好友移至默认分组

                        MyTabItem tabItem = DataUtil.FriendTabItems[userGroup.userGroupId];
                        MyButton[] friendArray = new MyButton[tabItem.myGrid.Children.Count];
                        tabItem.myGrid.Children.CopyTo(friendArray, 0);

                        MyTabItem defaultTabItem = DataUtil.FriendTabItems["0"];
                        for (int i = 0; i < friendArray.Length; i++)
                        {
                            defaultTabItem.myGrid.AddButton(MyType.User, (friendArray[i].baseRole as Member));
                        }



                        int currentIndex = this.Children.IndexOf(btn);
                        ///删掉分组对应的好友分组
                        btn.ParentTabControl.Items.Remove(DataUtil.FriendTabItems[userGroup.userGroupId]);
                        DataUtil.FriendTabItems.Remove(userGroup.userGroupId);
                        ///删掉分组
                        this.Children.Remove(btn);

                        ///删除全局的分组记录
                        DataUtil.DeleteUserGroup(userGroup.userGroupId);

                        ///将后面的分组移除
                        List<MyButton> temp = new List<MyButton>();
                        for (; currentIndex < this.Children.Count; )
                        {
                            temp.Add(this.Children[currentIndex] as MyButton);
                            this.Children.RemoveAt(currentIndex);

                        }
                        ///将后面的分组前移后加上
                        foreach (MyButton button in temp)
                        {
                            Grid.SetRow(button, currentIndex / 3);
                            Grid.SetColumn(button, currentIndex % 3);
                            this.Children.Add(button);
                            currentIndex++;
                        }
                        #endregion
                        break;
                    }

                case MyType.Group:
                    {
                        #region 群组

                        Group group = btn.baseRole as Group;
                        ///将好友移至默认分组




                        int currentIndex = this.Children.IndexOf(btn);
                        ///删掉群组对应的成员列表
                        DataUtil.TabControl.Items.Remove(DataUtil.GroupMemberTabItems[group.GroupId]);
                        DataUtil.GroupMemberTabItems.Remove(group.GroupId);
                        ///删除群组对应的聊天窗口
                        DataUtil.MessageTabControl.Items.Remove(DataUtil.GroupMessageTabItems[group.GroupId]);
                        DataUtil.GroupMessageTabItems.Remove(group.GroupId);
              


                        ///删掉分组
                        this.Children.Remove(btn);

                        ///删除全局的分组记录
                        DataUtil.DeleteGroup(group.GroupId);

                        ///将后面的分组移除
                        List<MyButton> temp = new List<MyButton>();
                        for (; currentIndex < this.Children.Count; )
                        {
                            temp.Add(this.Children[currentIndex] as MyButton);
                            this.Children.RemoveAt(currentIndex);

                        }
                        ///将后面的分组前移后加上
                        foreach (MyButton button in temp)
                        {
                            Grid.SetRow(button, currentIndex / 3);
                            Grid.SetColumn(button, currentIndex % 3);
                            this.Children.Add(button);
                            currentIndex++;
                        }

                        break;
                        #endregion
                    }

            }


        }

        public MyButton GetButton(MyType myType, string id)
        {
            MyButton button = null;
            switch (myType)
            {
                case MyType.User:
                    {
                        foreach (MyButton btn in this.Children)
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
                case MyType.Group:
                    {
                        foreach (MyButton btn in this.Children)
                        {
                            if ((btn.baseRole as Group).GroupId == id)
                            {
                                button = btn;
                                break;
                            }
                        }
                        break;
                    }

                case MyType.UserInGroup:
                    {
                        foreach (MyButton btn in this.Children)
                        {
                            if ((btn.baseRole as Member).id == id)
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
