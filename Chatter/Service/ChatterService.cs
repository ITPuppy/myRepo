using System;
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
   public class ChatterService:IChatter
    {

        private Dictionary<string, ChatEventArgs> Online = new Dictionary<string, ChatEventArgs>();

        private  delegate void ChatEventHandler(Object sender,ChatEventArgs e);
        private event ChatEventHandler ChatEvent;
        private ChatEventHandler myEventHandler;
        private Dictionary<string, Member> friends;
        IChatterCallback callback;

       
        public MessageStatus Login(Member member)
        {
            Logger.Info(String.Format("用户{0}登录",member.Id+" "+member.NickName));

            if (!DALService.IsMember(member.Id,member.Password))
            {
                return MessageStatus.Failed;
            }

            callback = OperationContext.Current.GetCallbackChannel<IChatterCallback>();
            friends = GetFriendsList(member.Id);
            myEventHandler = new ChatEventHandler(HandleEvent);
            
            ChatEventArgs e = new ChatEventArgs();
            e.Id = member.Id;
            e.NickName = member.NickName;
            e.Type = MessageType.Login;
            BroadCatMessage(e);
            ChatEvent += HandleEvent;

            return MessageStatus.OK;
        }
        /// <summary>
        /// 从数据库查询好友列表
        /// </summary>
        /// <param name="p">id</param>
        /// <returns></returns>
        private Dictionary<string, Member> GetFriendsList(string id)
        {
            List<string> friendsId= DALService.GetFriendList(id);
            Dictionary<string,Member> friends=new Dictionary<string,Member>();
           if(friends!=null)
           {
               foreach(string friendId in friendsId)
               {
                   friends.Add(friendId,DALService.GetMember(friendId));
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
            foreach( ChatEventHandler hanlder in ChatEvent.GetInvocationList())
            {
                hanlder.BeginInvoke(this,e,new AsyncCallback(EndAsync),hanlder);
            }

        }

        private void EndAsync(IAsyncResult ar)
        {
             ChatEventHandler hanlder= ar.AsyncState as ChatEventHandler;
             hanlder.EndInvoke(ar);
        }

        

       
        public List<Member> GetFriends(string id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        void HandleEvent(object sender, ChatEventArgs e)
        {
            if (friends.ContainsKey(e.Id))
            {
                switch (e.Type)
                {
                    case MessageType.Login:
                        {
                            callback.OnLogin(e.Id);
                            break;
                        }
                    case MessageType.Logoff:
                        {
                            callback.OnLogoff(e.Id);
                            break;
                        }
                }
            }
        }
    }


    /// <summary>
    /// 自定义事件参数
    /// </summary>
    class ChatEventArgs : EventArgs
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
