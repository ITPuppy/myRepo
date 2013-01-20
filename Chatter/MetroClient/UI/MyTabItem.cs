using Chatter.MetroClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.UI
{
    public class MyTabItem:TabItem
    {
        public ScrollViewer scrollViewer;
        private MyGrid myGrid;
        private ChatterClient client;
        
        public MyTabItem(MyType type,BaseRole[] role,ChatterClient client)
        {
            this.client = client;
            scrollViewer = new ScrollViewer();
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;



            switch (type)
            {
                case MyType.UserGroup:
                    {

                        UserGroup[] userGroups = role as UserGroup[];

                        ContextMenu cm = new ContextMenu();
                        MenuItem addUserGroupMenuItem = new MenuItem();
                        cm.Background = new SolidColorBrush(Color.FromArgb(255, 114, 119, 123));
                        cm.Foreground = new SolidColorBrush(Colors.White);
                        this.ContextMenu = cm;
                        addUserGroupMenuItem.Header = "添加分组";
                        addUserGroupMenuItem.Click += addUserGroupMenuItem_Click;
                        cm.Items.Add(addUserGroupMenuItem);
                       
                        

                         myGrid = new MyGrid(userGroups);
                         client.AddUserGroupCompleted += client_AddUserGroupCompleted;
                       
                        break;
                    }
                case MyType.User:
                    {

                        Member[] friends = role as Member[];

                       
                     
                         myGrid = new MyGrid(friends);
                       
                      

                        break;
                    }
            }



            scrollViewer.Content = myGrid;
            this.Content = scrollViewer;
            
        }


        void client_AddUserGroupCompleted(object sender, AddUserGroupCompletedEventArgs e)
        {
            if (e.Error != null)
                throw e.Error;
            if (e.Result.status == MessageStatus.Failed)
            {
                MessageBox.Show("添加失败");
            }
            else
            {
                string userGroupId = e.Result.userGroup.userGroupId;

            }
        }


        void addUserGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
           
            client.AddUserGroupAsync(new UserGroup() { userGroupName = "我的家人" });
        }
    }
}
