using Chatter.MetroClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using MetroClient.ChatterService;
using System.Windows.Threading;
using Chatter.Log;
using System.Threading;
using Chatter.MetroClient.Sound;
using Chatter.MetroClient.P2P;
using Chatter.MetroClient.UDP;



namespace Chatter.MetroClient.Callback
{
    class ChatterCallback : IChatterCallback
    {

        /// <summary>
        /// 有人登陆
        /// </summary>
        /// <param name="id"></param>
        public void OnLogin(string id)
        {

            ///如果是好友把好友点亮
            foreach (MyTabItem item in DataUtil.FriendTabItems.Values)
            {
                var btn = item.myGrid.GetButton(MyType.User, id);
                if (btn != null)
                {
                    btn.ChangeMemberStatus(MemberStatus.Online);
                    DataUtil.FriendMessageTabItems[id].sendFileMenu.SetStatus(MemberStatus.Online);
                    DataUtil.FriendMessageTabItems[id].audioMenu.SetStatus(MemberStatus.Online);
                    break;
                }
            }
            ///如果是群组成员，也点亮
            foreach (MyTabItem item in DataUtil.GroupMemberTabItems.Values)
            {
                var btn = item.myGrid.GetButton(MyType.UserInGroup, id);
                if (btn != null)
                {
                    btn.ChangeMemberStatus(MemberStatus.Online);
                }
            }

        }


        /// <summary>
        /// 有人退出
        /// </summary>
        /// <param name="id"></param>
        public void OnLogoff(string id)
        {

            foreach (MyTabItem item in DataUtil.FriendTabItems.Values)
            {
                var btn = item.myGrid.GetButton(MyType.User, id);
                if (btn != null)
                {
                    btn.ChangeMemberStatus(MemberStatus.Offline);
                    DataUtil.FriendMessageTabItems[id].sendFileMenu.SetStatus(MemberStatus.Offline);
                    DataUtil.FriendMessageTabItems[id].audioMenu.SetStatus(MemberStatus.Offline);
                    break;
                }
            }

            foreach (MyTabItem item in DataUtil.GroupMemberTabItems.Values)
            {
                var btn = item.myGrid.GetButton(MyType.UserInGroup, id);
                if (btn != null)
                {
                    btn.ChangeMemberStatus(MemberStatus.Offline);
                }
            }

        }

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="mesg"></param>
        public void OnSendMessage(Message mesg)
        {


            ///播放声音
            SoundPlayer.Play();


            try
            {

                ///文本消息
                if (mesg is TextMessage)
                {
                    ReceiveTextMessage(mesg);
                }
                ///文件请求
                else if (mesg is FileMessage)
                {
                    ReceiveFileMessage(mesg);
                }
                ///命令
                else if (mesg is CommandMessage)
                {

                    ReceiveCommandMessage(mesg);
                }
                ///语音请求
                else if (mesg is AudioMessage)
                {
                    ReceiveAudioMessage(mesg);
                }
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("显示信息出错", ex);
            }


        }
        /// <summary>
        /// 处理语音请求消息
        /// </summary>
        /// <param name="mesg"></param>
        private void ReceiveAudioMessage(Message mesg)
        {
            ///已经语音，不能再跟其他人语音
            DataUtil.FriendMessageTabItems[(mesg.from as Member).id].audioMenu.IsEnabled = false;

            AudioMessage am = mesg as AudioMessage;
            AudioForm af = new AudioForm(am, false);

            ///结束语音，enable语音按钮，能继续跟其他人语音
            af.Closed += new EventHandler((obj, args) =>
            {
                DataUtil.FriendMessageTabItems[(mesg.from as Member).id].audioMenu.IsEnabled = true;
            }
            );
            af.Show();


        }
        /// <summary>
        /// 处理命令消息
        /// </summary>
        /// <param name="mesg"></param>
        private void ReceiveCommandMessage(Message mesg)
        {

            try
            {
                CommandMessage cmdMesg = mesg as CommandMessage;
                ///取消发送文件
                if (cmdMesg.CommandType == MyCommandType.CanceledSendFile)
                {
                    ///判断文件传输窗口是否存在
                    if (DataUtil.HasTransfer())
                    {
                        ///判断是否存在想取消的传输项
                        if (DataUtil.Transfer.transferTask.ContainsKey(cmdMesg.Guid))
                        {
                            DataUtil.Transfer.transferTask[cmdMesg.Guid].TheOtherCancel(false);
                        }
                    }
                }
                ///取消语音请求
                else if (cmdMesg.CommandType == MyCommandType.CanceledAudioRequest)
                {
                    DataUtil.AudioForms[(mesg.from as Member).id].TheOtherCanceled();
                }
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("收到命令Message，处理出错", ex);
            }
        }
        /// <summary>
        /// 处理发送文件请求
        /// </summary>
        /// <param name="mesg"></param>
        private void ReceiveFileMessage(Message mesg)
        {

            DataUtil.Transfer.ReceiveFile((FileMessage)mesg);

        }

        /// <summary>
        /// 处理文本消息
        /// </summary>
        /// <param name="mesg"></param>
        private void ReceiveTextMessage(Message mesg)
        {


            if (mesg.from is Member)
            {
                Member member = mesg.from as Member;
                DataUtil.SetCurrentMessageWindow(member);
                MyMessageTabItem item = DataUtil.FriendMessageTabItems[member.id];
                if (item != null)
                    item.ReceiveMessage(mesg);
            }


        }
        /// <summary>
        /// 处理服务器发过来的请求
        /// </summary>
        /// <param name="mesg"></param>
        public void RequestToTargetClient(Message mesg)
        {



            switch (mesg.type)
            {
                ///添加好友请求
                case MessageType.AddFriend:
                    {



                        Member friend = mesg.from as Member;
                        MessageStatus status = MessageStatus.Refuse;
                        ///判断是否为好友
                        if (DataUtil.IsFriend(friend.id))
                        {
                            this.OnLogin(friend.id);
                            new Thread(
                                new ThreadStart(
                                    () =>
                                    {
                                        DataUtil.Client.ResponseToRequest(new Result() { Member = friend, UserGroup = mesg.to as UserGroup, Status = MessageStatus.Accept, Type = MessageType.AddFriend });
                                    })).Start();
                            return;
                        }
                        MessageBoxResult mbr = MessageBox.Show(friend.id + friend.nickName + "请求添加好友", "请求", MessageBoxButton.YesNoCancel);

                        if (mbr == MessageBoxResult.Yes)
                        {
                            status = MessageStatus.Accept;
                        }
                        else if (mbr == MessageBoxResult.No)
                        {
                            status = MessageStatus.Refuse;
                        }
                        else
                        {
                            return;
                        }

                        ///注册添加好友成功事件
                        DataUtil.Client.ResponseToRequestCompleted += Client_ResponseToAddFriendCompleted;
                        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                        {
                            DataUtil.Client.ResponseToRequestAsync(new Result() { Type = MessageType.AddFriend, Member = friend, UserGroup = mesg.to as UserGroup, Status = status });
                        }));
                        break;
                    }
                ///添加好友到群组请求
                case MessageType.AddFriend2Group:
                    {
                        Group group = mesg.from as Group;


                        Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                            {
                                MyTabItem groupTabItem = DataUtil.TabControl.groupTabItem as MyTabItem;


                                ///在界面上添加组
                                groupTabItem.myGrid.AddButton(MyType.Group, group);


                                ///添加组内成员的TabItem
                                MyTabItem tabItem = new MyTabItem(MyType.UserInGroup, group.GroupId);
                                DataUtil.TabControl.Items.Add(tabItem);

                                DataUtil.GroupMemberTabItems.Add(group.GroupId, tabItem);

                                var groupMesgTabItem = new MyMessageTabItem(MyType.Group, group);
                                DataUtil.MessageTabControl.Items.Add(groupMesgTabItem);
                                DataUtil.GroupMessageTabItems.Add(group.GroupId, groupMesgTabItem);

                            }));

                        ///接收群消息
                        P2PClient.GetP2PClient(group.GroupId);

                        break;
                    }
            }


        }

        void Client_ResponseToAddFriendCompleted(object sender, ResponseToRequestCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Status == MessageStatus.Refuse)
                {

                    return;
                }
                else if (e.Result.Status == MessageStatus.Failed)
                {
                    MessageBox.Show(e.Result.Mesg);
                    return;
                }
                else if (e.Result.Status == MessageStatus.OK)
                {
                    MyTabItem tabItem = DataUtil.FriendTabItems["0"];
                    tabItem.myGrid.AddButton(MyType.User, e.Result.Member);


                    MyMessageTabItem item = new MyMessageTabItem(MyType.User, e.Result.Member);
                    DataUtil.FriendMessageTabItems.Add(e.Result.Member.id, item);
                    DataUtil.MessageTabControl.Items.Add(item);

                }
            }
            catch (Exception ex)
            {
                //  Logger.Error("在响应添加好友时候出错"+ex.Message);
            }
            finally
            {
                DataUtil.Client.ResponseToRequestCompleted -= Client_ResponseToAddFriendCompleted;
            }
        }

        /// <summary>
        /// 收到服务器响应消息
        /// </summary>
        /// <param name="result"></param>
        public void ReponseToSouceClient(Result result)
        {


            try
            {
                ///响应添加好友消息
                if (result.Type == MessageType.AddFriend)
                {

                    if (result.Status == MessageStatus.Accept)
                    {
                        MessageBox.Show("您已经与" + result.Member.nickName + "成为好友");

                        MyTabItem tabItem = DataUtil.FriendTabItems[result.UserGroup.userGroupId];
                        result.Member.status = MemberStatus.Online;
                        tabItem.myGrid.AddButton(MyType.User, result.Member);

                        MyMessageTabItem item = new MyMessageTabItem(MyType.User, result.Member);
                        DataUtil.FriendMessageTabItems.Add(result.Member.id, item);
                        DataUtil.MessageTabControl.Items.Add(item);

                    }
                    else
                    {
                        MessageBox.Show(result.Mesg);
                    }
                }
                ///响应发送文件请求消息
                else if (result.Type == MessageType.File)
                {

                    if (DataUtil.HasTransfer())
                    {
                        if (DataUtil.Transfer.transferTask.ContainsKey(result.Guid))
                        {
                            if (result.Status == MessageStatus.Accept)
                            {

                                DataUtil.Transfer.transferTask[result.Guid].BeginSendFile(result.EndPoint);
                            }
                            else
                            {
                                DataUtil.Transfer.transferTask[result.Guid].TheOtherCancel(true);

                            }
                        }
                    }
                }
                    ///响应语音请求消息
                else if (result.Type == MessageType.Audio)
                {
                    if (result.Status == MessageStatus.Accept)
                    {

                        DataUtil.AudioForms[result.Member.id].InitSend(result.EndPoint);


                    }
                    else if (result.Status == MessageStatus.Refuse)
                    {
                        DataUtil.AudioForms[result.Member.id].Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("接收服务器回应消息出错", ex);
            }


        }

        /// <summary>
        /// 服务器发来对方endpoint，用来语音
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="member"></param>
        /// <param name="isFrom"></param>
        public void SendUDPEndPoint(MyEndPoint endPoint, Member member, bool isFrom)
        {

            DataUtil.AudioForms[member.id].Start(endPoint);


        }
        /// <summary>
        /// 服务器发来对方endpoint,用来发文件
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="guid"></param>
        public void SendTCPEndPoint(MyEndPoint endPoint, string guid)
        {
            if (DataUtil.HasTransfer())
            {
                if (DataUtil.Transfer.transferTask.ContainsKey(guid))
                {
                    DataUtil.Transfer.transferTask[guid].BeginTCPHolePunching(endPoint);
                }
            }
        }


        #region useless


        public IAsyncResult BeginRequestToTargetClient(Message mesg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndRequestToTargetClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public void EndOnSendMessage(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginOnLogoff(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogoff(IAsyncResult result)
        {

        }

        public IAsyncResult BeginOnLogin(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogin(IAsyncResult result)
        {

        }

        public IAsyncResult BeginSendUDPEndPoint(MyEndPoint endPoint, Member member, bool isFrom, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSendUDPEndPoint(IAsyncResult result)
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

        public IAsyncResult BeginSendHeartBeat(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginReponseToSouceClient(Result result, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReponseToSouceClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }



        public IAsyncResult BeginSendTCPEndPoint(MyEndPoint endPoint, string guid, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndSendTCPEndPoint(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        #endregion






    }
}
