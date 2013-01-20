using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Chatter.Contract.DataContract;
using Chatter.Contract.ServiceContract;
using Chatter.DAL;
using Chatter.Log;

namespace Chatter.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ChatterService : IChatter
    {
        /// <summary>
        /// 保存在线人的信息，键为id，值为ChatterEventHandler，通过ChatterEvent可以获取所在对象的信息
        /// 类所有
        /// </summary>
        private static Hashtable Online = new Hashtable();
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
        IChatterCallback callback;

        static private object lockObj = new object();
        /// <summary>
        /// 保存当前用户
        /// </summary>
        private Member member;

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="member">包含用户名密码的member</param>
        /// <returns>返回登录结果，如果登录成功，Result.Member 包含用户的信息</returns>
        public Result Login(Member member)
        {


            Logger.Info(String.Format("用户{0}登录", member.Id + " " + member.NickName));


            try
            {
                ///判断用户名和密码是否正确
                if (!DALService.IsMember(member.Id, member.Password))
                {
                    return new Result() { Status = MessageStatus.Failed };
                }
                this.member = member;
                ///获得回调句柄
                callback = OperationContext.Current.GetCallbackChannel<IChatterCallback>();
                ///获得好友们的id
                friends = GetFriendsList(member.Id);
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



                    ///将用户加到在线人数
                    Online.Add(member.Id, myEventHandler);

                }
                ///打印在线人数
                PrintOnLineNumber();
            }
            catch (Exception e)
            {
                Logger.Error(member.Id + "登录时候出现错误" + e.Message);
                return new Result() { Status = MessageStatus.Failed, Member = null };
            }

            ///返回登录结果
            return new Result() { Status = MessageStatus.OK, Member = DALService.GetMember(member.Id) };
        }
        /// <summary>
        /// 从数据库查询好友列表
        /// </summary>
        /// <param name="p">id</param>
        /// <returns></returns>
        private Dictionary<string, Member> GetFriendsList(string id)
        {
            ///新建字典，键为好友id，值为好友信息。
            Dictionary<string, Member> friends = new Dictionary<string, Member>();

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

            return friends;
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
        /// 获取好友列表。包括分组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserGroup> GetFriends(string id)
        {
            return DALService.GetFriendList(id);

        }


        public List<Group> GetGroups(string id)
        {
            throw new NotImplementedException();
        }


        public MessageStatus AddFriend(string id, string friendId)
        {
            throw new NotImplementedException();
        }

        public MessageStatus AddGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public MessageStatus AddFriend2Group(string friendId, string groupId)
        {
            throw new NotImplementedException();
        }

        public MessageStatus SendMesg(Message mesg)
        {
            throw new NotImplementedException();
        }

        public MessageStatus Logoff(Member member)
        {
            Logger.Info(String.Format("用户{0}退出", member.Id + " " + member.NickName));
            ChatEventArgs e = new ChatEventArgs();
            e.Id = member.Id;
            e.NickName = member.NickName;
            e.Type = MessageType.Logoff;
            Online.Remove(member.Id);
            BroadCatMessage(e);

            PrintOnLineNumber();
            return MessageStatus.OK;
        }
        /// <summary>
        /// 事件处理函数，用户向好友广播消息，登录、退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleEvent(object sender, ChatEventArgs e)
        {

            ChatterService friendChatterService = sender as ChatterService;


            ///  判断是否为自己的好友，正常情况都是 ，因为只给好友广播消息
            if (friends.ContainsKey(e.Id))
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
                    Logger.Error("回调出现问题"+ex.Message);
                }
            }
        }


        void PrintOnLineNumber()
        {

            Logger.Info("当前在线人数:" + Online.Count);

        }

        /// <summary>
        /// 添加用户组
        /// </summary>
        /// <param name="userGroup">用户组信息</param>
        /// <returns>包含用户组id的Result</returns>
        public Result AddUserGroup(UserGroup userGroup)
        {
           string userGroupId= DALService.AddUserGroup(member.Id, userGroup.UserGroupName);
           if (userGroupId == null)
               return new Result() { Status = MessageStatus.Failed };
           else
               return new Result() { Status = MessageStatus.OK, UserGroup = new UserGroup() {  UserGroupId=userGroupId} };
        }
    }


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
}
