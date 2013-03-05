using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Chatter.Contract.DataContract;
using Chatter.Contract.ServiceContract;
using Chatter.DAL;
using Chatter.Log;
using System.Threading;
using System.ServiceModel.PeerResolvers;



namespace Chatter.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatterService : IChatter, IDisposable
    {

        #region 属性


        public static Hashtable lastUpdateTable = new Hashtable();


        /// <summary>
        /// 保存在线人的信息，键为id，值为ChatterEventHandler，通过ChatterEvent可以获取所在对象的信息
        /// 类所有
        /// </summary>
        public static Hashtable Online = new Hashtable();
        /// <summary>
        /// 定义委托类型
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e"></param>
        public delegate void ChatEventHandler(Object sender, ChatEventArgs e);
        /// <summary>
        /// 定义事件，当登录退出时。事件触发
        /// </summary>
        private event ChatEventHandler ChatEvent;
        /// <summary>
        /// 事件处理函数
        /// </summary>
        public ChatEventHandler myEventHandler;
        /// <summary>
        /// 在线好友，对象所有
        /// </summary>
        private Dictionary<string, Member> friends;
        /// <summary>
        /// 回调句柄
        /// </summary>
        public IChatterCallback callback;

        static private object lockObj = new object();
        /// <summary>
        /// 保存当前用户
        /// </summary>
        private Member member;

        /// <summary>
        /// 客户端IP端口
        /// </summary>
        MyEndPoint endpoint = null;


        private Hashtable groupHost = null;

        #endregion

        #region 函数

        #region 登录退出
        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="member">包含用户名密码的member</param>
        /// <returns>返回登录结果，如果登录成功，Result.Member 包含用户的信息</returns>
        public Result Login(Member member)
        {


            MyLogger.Logger.Info(String.Format("用户{0}请求登录", member.Id + " " + member.NickName));


            try
            {
                ///判断用户名和密码是否正确
                if (!DALService.IsMember(member.Id, member.Password))
                {
                    MyLogger.Logger.Info(String.Format("用户{0}登录失败", member.Id + " " + member.NickName));
                    return new Result() { Status = MessageStatus.Failed };

                }

                this.member = DALService.GetMember(member.Id);


                GetEndPoint();
                ///获得回调句柄
                callback = OperationContext.Current.GetCallbackChannel<IChatterCallback>();
                ///获得好友们的id
                RefreshFriendsList();
                ///新建消息处理函数,处理登录、退出消息
                myEventHandler = new ChatEventHandler(HandleEvent);

                ///新建消息参数
                ChatEventArgs e = new ChatEventArgs();
                ///消息发出者的id
                e.Id = member.Id;
                ///消息发出者的昵称
                e.NickName = member.NickName;
                ///消息类型为登录
                e.Type = MessageType.Login;
                ///为每个在线好友订阅事件
                foreach (string friendId in friends.Keys)
                {
                    if (Online.ContainsKey(friendId))
                    {

                        ChatEvent += Online[friendId] as ChatEventHandler;
                    }
                }


                ///广播消息 将登录退出的消息发给好友
                BroadCatMessage(e);
                ///加锁，防止多线程同时访问
                lock (lockObj)
                {

                    ///将用户加到在线列表
                    Online.Add(member.Id, myEventHandler);

                }

                MyLogger.Logger.Info(String.Format("用户{0}登录成功", member.Id + " " + member.NickName));
                ///打印在线人数
                PrintOnLineNumber();
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error(member.Id + "登录时候出现错误", e);
                return new Result() { Status = MessageStatus.Failed, Member = null };
            }

            ///返回登录结果
            return new Result() { Status = MessageStatus.OK, Member = DALService.GetMember(member.Id) };
        }

        private void GetEndPoint()
        {
            OperationContext context = OperationContext.Current;
            //获取传进的消息属性
            System.ServiceModel.Channels.MessageProperties properties = context.IncomingMessageProperties;
            //获取消息发送的远程终结点IP和端口
           var ep = properties[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name] as System.ServiceModel.Channels.RemoteEndpointMessageProperty;
           endpoint = new MyEndPoint();
           endpoint.Address = ep.Address;
           endpoint.Port = ep.Port;
        }



        public void Logoff(Member member)
        {
            MyLogger.Logger.Info(String.Format("用户{0}退出", member.Id + " " + member.NickName));
            ChatEventArgs e = new ChatEventArgs();
            e.Id = member.Id;
            e.NickName = member.NickName;
            e.Type = MessageType.Logoff;
            if (Online.ContainsKey(member.Id))
                Online.Remove(member.Id);
            if (lastUpdateTable.ContainsKey(member.Id))
                lastUpdateTable.Remove(member.Id);
            BroadCatMessage(e);

            PrintOnLineNumber();

        }

        #endregion

        #region 获取列表：分组，群

        /// <summary>
        /// 获取好友列表。包括分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserGroup> GetFriends(string id)
        {
            try
            {
                List<UserGroup> ugs = DALService.GetFriendList(id);
                foreach (UserGroup ug in ugs)
                {
                    foreach (Member member in ug.Members)
                    {
                        if (Online.ContainsKey(member.Id))
                            member.Status = MemberStatus.Online;
                        else
                            member.Status = MemberStatus.Offline;
                    }
                }
                return ugs;
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("获取好友列表失败",ex);
                return null;
            }
            
        }


        public List<Group> GetGroups(string id)
        {
            return DALService.GetGroup(id);
        }
        #endregion

        #region 群

        public Result AddGroup(Group group)
        {

           string groupId= DALService.AddGroup(group);

           if (groupId == null)
                return new Result() { Status = MessageStatus.Failed };
            else
                return new Result() { Status = MessageStatus.OK,Group=new Group(){ GroupId=groupId,Name=group.Name, OwnerId=group.OwnerId}};
        }

        public MessageStatus AddFriend2Group(string friendId, string groupId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 消息
        public MessageStatus SendMesg(Message mesg)
        {

            try
            {
                if (mesg is TextMessage)
                {
                    return SendTextMessage(mesg);
                }

                else if (mesg is FileMessage)
                {
                    return SendFileMessage(mesg);
                }
                else if (mesg is CommandMessage)
                {
                   return SendTextMessage(mesg);
                }

                else
                {
                    throw new Exception("not suppor message type");
                }
            }


            catch (Exception ex)
            {
                MyLogger.Logger.Error("转发信息出错", ex);
                return MessageStatus.Failed;
            }


        }

        private MessageStatus SendFileMessage(Message mesg)
        {
            if (mesg.To is Member)
            {
                Member to = mesg.To as Member;
                if (!Online.ContainsKey(to.Id))
                {
                    return MessageStatus.Failed;
                }

                ChatEventHandler handler = Online[to.Id] as ChatEventHandler;
                ChatterService service = handler.Target as ChatterService;

                if (!service.friends.ContainsKey(member.Id))
                {
                    return MessageStatus.Refuse;
                }

               
                
                FileMessage fm=mesg as FileMessage;
                fm.EndPoint = service.endpoint;
                mesg = fm;
                service.callback.OnSendMessage(mesg);

              



                return MessageStatus.OK;
            }
            else
            {
                throw new Exception("not support user type ");
            }
        }

        private MessageStatus SendTextMessage(Message mesg)
        {



            if (mesg.To is Member)
            {
                Member to = mesg.To as Member;
                if (!Online.ContainsKey(to.Id))
                {
                    return MessageStatus.Failed;
                }

                ChatEventHandler handler = Online[to.Id] as ChatEventHandler;
                ChatterService service = handler.Target as ChatterService;

                if (!service.friends.ContainsKey(member.Id))
                {
                    return MessageStatus.Refuse;
                }

                service.callback.OnSendMessage(mesg);



                return MessageStatus.OK;
            }
            else
            {
                throw new Exception("not support user type ");
            }
        }

        #endregion


        #region 服务器处理

        /// <summary>
        /// 从数据库查询好友列表
        /// </summary>
        /// <param name="p">id</param>
        /// <returns></returns>
        public void RefreshFriendsList()
        {
            string id = this.member.Id;
            friends = new Dictionary<string, Member>();

            ///从数据库读出好友列表，遍历之，将好友信息放到字典里。
            foreach (UserGroup userGroup in DALService.GetFriendList(id))
            {
                if (userGroup != null)
                {
                    foreach (Member member in userGroup.Members)
                    {
                        friends.Add(member.Id, member);

                    }
                }
            }


        }

        /// <summary>
        /// 向好友广播消息，比如登录、退出
        /// </summary>
        /// <param name="e"></param>
        private void BroadCatMessage(ChatEventArgs e)
        {
            ///当在线好友数为0时
            if (ChatEvent == null)
                return;
            ///对好友广播消息
            foreach (ChatEventHandler hanlder in ChatEvent.GetInvocationList())
            {
                hanlder.BeginInvoke(this, e, new AsyncCallback(EndAsync), hanlder);
            }

        }
        /// <summary>
        /// 异步调用机制
        /// </summary>
        /// <param name="ar"></param>
        private void EndAsync(IAsyncResult ar)
        {

            ChatEventHandler hanlder = ar.AsyncState as ChatEventHandler;
            ///阻塞等待异步调用结束
            hanlder.EndInvoke(ar);


        }
        /// <summary>
        /// 事件处理函数，用户向好友广播消息，登录、退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleEvent(object sender, ChatEventArgs e)
        {

            ChatterService friendChatterService = sender as ChatterService;


            ///    判断是否为自己的好友，正常情况都是 ，因为只给好友广播消息
            if (friends.ContainsKey(e.Id) && Online.ContainsKey(member.Id))
            {
                try
                {
                    switch (e.Type)
                    {



                        ///账户为id的好友登录
                        case MessageType.Login:
                            {
                                ///为好友订阅事件，以便自己登录退出通知他
                                ChatEvent += friendChatterService.myEventHandler;
                                callback.OnLogin(e.Id);
                                break;
                            }
                        ///用户名为id的好友退出
                        case MessageType.Logoff:
                            {
                                ///为好友取消订阅
                                ChatEvent -= friendChatterService.myEventHandler;

                                callback.OnLogoff(e.Id);
                                break;
                            }

                    }
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Warn("回调失败" + member.Id);


                }

            }
        }


        void PrintOnLineNumber()
        {

            MyLogger.Logger.Info("当前在线人数:" + Online.Count);

        }

        #endregion

        #region 分组

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>包含用户组id的Result</returns>
        public Result AddUserGroup(UserGroup userGroup)
        {
            string userGroupId = DALService.AddUserGroup(member.Id, userGroup.UserGroupName);
            if (userGroupId == null)
                return new Result() { Status = MessageStatus.Failed };
            else
                return new Result() { Status = MessageStatus.OK, UserGroup = new UserGroup() { UserGroupId = userGroupId, UserGroupName = userGroup.UserGroupName } };
        }







        public Result DeleteUserGroup(string id, UserGroup userGroup)
        {
            if (DALService.DeleteUserGroup(id, userGroup.UserGroupId))
                return new Result() { Status = MessageStatus.OK };
            else return new Result() { Status = MessageStatus.Failed };
        }

        #endregion

        #region 好友

        /// <summary>
        /// 添加好友，源客户调用
        /// </summary>
        /// <param name="friendId">对方id</param>
        /// <param name="userGroupId">将好友放到某个分组ID</param>
        public void AddFriend(string friendId, string userGroupId)
        {
            ///添加完好友把 所属的UserGroup信息填上
            ///得先请求好友同意
            ///

            if (!Online.ContainsKey(friendId))
            {
                new Thread(new ThreadStart(() =>
                {
                    callback.ReponseToSouceClient(new Result() { Type= MessageType.AddFriend ,Status = MessageStatus.Failed, Mesg = "对方不在线" });

                })).Start();
                return;
            }


            ChatEventHandler handler = Online[friendId] as ChatEventHandler;
            ChatterService service = handler.Target as ChatterService;

            new Thread(new ThreadStart(() =>
            {
                service.callback.RequestToTargetClient(new Message()
                {
                    Type = MessageType.AddFriend,
                    From = this.member,
                    ///此处TO保存了分组
                    To = new UserGroup() { UserGroupId = userGroupId }
                });
            })).Start();

        }


        public void ResponseToSendFile(Result result)
        {
           
            ChatEventHandler handler = Online[result.Member.Id] as ChatEventHandler;
            ChatterService service = handler.Target as ChatterService;
            if (result.Status == MessageStatus.Accept)
            {
                service.callback.ReponseToSouceClient(new Result() {Status=MessageStatus.Accept,Member=this.member,Type=MessageType.File, Guid=result.Guid, EndPoint=result.EndPoint });
            }

            else if (result.Status == MessageStatus.Refuse)
            {
                service.callback.ReponseToSouceClient(new Result() { Status = MessageStatus.Refuse, Member = this.member, Type = MessageType.File, Guid = result.Guid, EndPoint = result.EndPoint });
            }

           
        
        }
        
        /// <summary>
        /// 目的好友收到加好友请求调用
        /// 将是否接受的信息发给服务器
        /// 服务器回调源客户端函数通知源客户端
        /// </summary>
        /// <param name="result">包含是否接受、源客户端好友的id，分组id</param>
        /// <returns></returns>
        public Result ResponseToAddFriend(Result result)
        {
            if (!Online.ContainsKey(result.Member.Id))
            {
                return new Result() { Status = MessageStatus.Failed, Mesg = "对方已经下线", Type = MessageType.AddFriend };
            }
            ChatEventHandler handler = Online[result.Member.Id] as ChatEventHandler;
            ChatterService service = handler.Target as ChatterService;
            if (result.Status == MessageStatus.Accept)
            {



                ///相互绑定登录退出事件

                ChatEventHandler sourceHandler = Online[result.Member.Id] as ChatEventHandler;

                bool isHandlerExist = false;
                if (ChatEvent != null)
                {
                    foreach (ChatEventHandler tempHandler in ChatEvent.GetInvocationList())
                    {
                        if (tempHandler.Equals(sourceHandler))
                        {
                            isHandlerExist = true;
                            break;
                        }
                    }
                }
                if (!isHandlerExist)
                {
                    ChatEvent += Online[result.Member.Id] as ChatEventHandler;

                }
                isHandlerExist = false;
                if (service.ChatEvent != null)
                {
                    foreach (ChatEventHandler tempHandler in service.ChatEvent.GetInvocationList())
                    {
                        if (tempHandler.Equals(sourceHandler))
                        {
                            isHandlerExist = true;
                            break;
                        }
                    }
                }
                if (!isHandlerExist)
                {
                    service.ChatEvent += this.myEventHandler;
                    isHandlerExist = false;
                }
                if (!DALService.IsFriend(this.member.Id, result.Member.Id))
                    DALService.AddFriend(this.member.Id, result.Member.Id);
                if (!DALService.IsFriend(result.Member.Id, this.member.Id))
                    DALService.AddFriend(result.Member.Id, this.member.Id, result.UserGroup.UserGroupId);

                service.RefreshFriendsList();
                RefreshFriendsList();

                service.callback.ReponseToSouceClient(new Result() { Mesg = "对方同意添加好友请求", Status = MessageStatus.Accept, Member = this.member, UserGroup = result.UserGroup, Type = MessageType.AddFriend });
                return new Result() { Status = MessageStatus.OK, Mesg = "成功通知对方", Member = result.Member, Type = MessageType.AddFriend };
            }

            else if (result.Status == MessageStatus.Refuse)
            {
                service.callback.ReponseToSouceClient(new Result() { Mesg = "对方拒绝了您的添加好友请求", Status = MessageStatus.Refuse, Member = this.member, UserGroup = result.UserGroup ,Type=MessageType.AddFriend});
                return new Result() { Status = MessageStatus.Refuse, Mesg = "成功通知对方", Type = MessageType.AddFriend };
            }

            return new Result() { Status = MessageStatus.OK, Mesg = "成功通知对方", Member = result.Member, Type = MessageType.AddFriend };
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="userGroupId">分组ID</param>
        /// <param name="friendId">好友ID</param>
        /// <returns></returns>
        public Result DeleteFriend(string id, string userGroupId, string friendId)
        {
            if (DALService.DeleteFriend(id, userGroupId, friendId))
            {
                if (Online.ContainsKey(friendId))
                {

                    try
                    {
                        ChatEventHandler handler = Online[friendId] as ChatEventHandler;
                        ChatterService service = handler.Target as ChatterService;
                        service.ChatEvent -= myEventHandler;
                        RefreshFriendsList();
                    }
                    catch (Exception ex)
                    {
                        MyLogger.Logger.Warn("对方已经下线", ex);
                    }
                }
                return new Result() { Status = MessageStatus.OK, UserGroup = new UserGroup() { UserGroupId = userGroupId }, Member = new Member() { Id = friendId } };
            }
            else
            {
                return new Result() { Status = MessageStatus.Failed, UserGroup = new UserGroup() { UserGroupId = userGroupId }, Member = new Member() { Id = friendId } };
            }
        }
        #endregion



        public void Dispose()
        {

            Logoff(this.member);

        }



        public void SendHeartBeat()
        {

            if (lastUpdateTable.ContainsKey(this.member.Id))
            {
                lastUpdateTable[this.member.Id] = DateTime.Now;
            }
            else
            {
                lastUpdateTable.Add(this.member.Id, DateTime.Now);
            }
        }
        #endregion


        



        public bool IsOnlie(string friendId)
        {
            return Online.ContainsKey(friendId);
        }
    }




    #region 自定义事件参数
    /// <summary>
    /// 自定义事件参数
    /// </summary>
    public class ChatEventArgs : EventArgs
    {
        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string nickName;

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        MessageType type;

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }
    }
    #endregion
}
