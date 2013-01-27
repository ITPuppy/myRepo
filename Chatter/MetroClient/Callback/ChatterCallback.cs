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
using System.Windows.Threading;



namespace Chatter.MetroClient.Callback
{
    class ChatterCallback : IChatterCallback
    {
        public void OnLogin(string id)
        {
            string userGroupId = DataUtil.GetUserGroupIdByMember(id);
            if (userGroupId != null)
            {
                MyTabItem tabItem=DataUtil.FriendTabItems[userGroupId];
                MyButton btn=tabItem.myGrid.GetButton(MyType.User,id);

                btn.ChangeMemberStatus(MemberStatus.Online);
            }
            else
            {
                MessageBox.Show("没有这个好友"+id);
            }

        }

        public void OnSendMessageCallback(Result result)
        {

        }

        public void OnLogoff(string id)
        {
            string userGroupId = DataUtil.GetUserGroupIdByMember(id);
            if (userGroupId != null)
            {
                MyTabItem tabItem = DataUtil.FriendTabItems[userGroupId];
                MyButton btn = tabItem.myGrid.GetButton(MyType.User, id);

                btn.ChangeMemberStatus(MemberStatus.Offline);
            }
            else
            {
                MessageBox.Show("没有这个好友" + id);
            }
        }


        public IAsyncResult BeginOnLogin(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogin(IAsyncResult result)
        {

        }





        public IAsyncResult BeginOnLogoff(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogoff(IAsyncResult result)
        {

        }


        public void OnSendMessage(Result result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOnSendMessage(Result result, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndOnSendMessage(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public void RequestToTargetClient(Message mesg)
        {

            switch (mesg.type)
            {
                case MessageType.AddFriend:
                    {

                        Member friend = mesg.from as Member;

                        MessageBoxResult mbr = MessageBox.Show(friend.id + friend.nickName + "请求添加好友", "请求", MessageBoxButton.YesNoCancel);
                        MessageStatus status=MessageStatus.Refuse;
                    if (mbr == MessageBoxResult.Yes)
                    {
                        status=MessageStatus.Accept;
                    }
                    else if (mbr ==MessageBoxResult.No)
                    {
                          status=MessageStatus.Refuse;
                    }
                    else 
                    {
                        return;
                    }
                       
                        
                        DataUtil.Client.ResponseToAddFriendCompleted += Client_ResponseToAddFriendCompleted;
                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                        {
                            DataUtil.Client.ResponseToAddFriendAsync(new Result() { member = friend, userGroup = mesg.to as UserGroup, status = status });
                        }));
                        break;
                    }
            }


        }

        void Client_ResponseToAddFriendCompleted(object sender, ResponseToAddFriendCompletedEventArgs e)
        {
            try
            {
                if (e.Result.status == MessageStatus.Failed)
                {
                    MessageBox.Show(e.Result.mesg);
                }
                else if(e.Result.status==MessageStatus.OK)
                {
                    MyTabItem tabItem=DataUtil.FriendTabItems["0"];
                    tabItem.myGrid.AddButton(MyType.User, e.Result.member);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DataUtil.Client.ResponseToAddFriendCompleted -= Client_ResponseToAddFriendCompleted;
            }
        }

        public IAsyncResult BeginRequestToTargetClient(Message mesg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndRequestToTargetClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void ReponseToSouceClient(Result result)
        {
            if (result.status == MessageStatus.Accept)
            {
                MessageBox.Show("您已经与" + result.member.nickName + "成为好友");
                MyTabItem tabItem = DataUtil.FriendTabItems[result.userGroup.userGroupId];
                tabItem.myGrid.AddButton(MyType.User, result.member);
            }
            else
            {
                MessageBox.Show(result.mesg);
            }
        }


        public IAsyncResult BeginReponseToSouceClient(Result result, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReponseToSouceClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
