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
using Chatter.Log;
using System.Threading;



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


        public void OnSendMessage(Message mesg)
        {
            try
            {
                if (mesg.from is Member)
                {
                    Member member = mesg.from as Member;
                    DataUtil.SetCurrentMessageWindow(member);
                    MyMessageTabItem item = DataUtil.MessageTabItems[member.id];
                    if (item != null)
                        item.ReceiveMessage(mesg);
                }
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("显示信息出错",ex);
            }
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
                        MessageStatus status = MessageStatus.Refuse;
                        if (DataUtil.IsFriend(friend.id))
                        {
                            this.OnLogin(friend.id);
                           new Thread(
                               new ThreadStart(
                                   ()=>{
                                       DataUtil.Client.ResponseToAddFriend(new Result() { member = friend, userGroup = mesg.to as UserGroup, status = MessageStatus.Accept });
                                   })).Start();
                           return;
                        }
                        MessageBoxResult mbr = MessageBox.Show(friend.id + friend.nickName + "请求添加好友", "请求", MessageBoxButton.YesNoCancel);
                     
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
                if (e.Result.status == MessageStatus.Refuse)
                {
                   
                    return;
                }
                else if (e.Result.status == MessageStatus.Failed)
                {
                    MessageBox.Show(e.Result.mesg);
                    return;
                }
                else if (e.Result.status == MessageStatus.OK)
                {
                    MyTabItem tabItem = DataUtil.FriendTabItems["0"];
                    tabItem.myGrid.AddButton(MyType.User, e.Result.member);


                    MyMessageTabItem item = new MyMessageTabItem(MyType.User, e.Result.member);
                    DataUtil.MessageTabItems.Add(e.Result.member.id, item);
                    DataUtil.MessageTabControl.Items.Add(item);

                }
            }
            catch (Exception ex)
            {
             //  Logger.Error("在响应添加好友时候出错"+ex.Message);
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
            try
            {
               
                if (result.status == MessageStatus.Accept)
                {
                    MessageBox.Show("您已经与" + result.member.nickName + "成为好友");
                    MyTabItem tabItem = DataUtil.FriendTabItems[result.userGroup.userGroupId];
                    result.member.status = MemberStatus.Online;
                    tabItem.myGrid.AddButton(MyType.User, result.member);

                    MyMessageTabItem item = new MyMessageTabItem(MyType.User, result.member);
                    DataUtil.MessageTabItems.Add(result.member.id, item);
                    DataUtil.MessageTabControl.Items.Add(item);
                }
                else
                {
                    MessageBox.Show(result.mesg);
                }
            }
            catch (Exception ex)
            {
               MyLogger.Logger.Error("接收服务器加好友相应出错" ,ex);
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


        public string SendHeartBeat()
        {
            return "1";
        }

        public IAsyncResult BeginSendHeartBeat(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string EndSendHeartBeat(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public IAsyncResult BeginOnSendMessage(Message mesg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }
    }
}
