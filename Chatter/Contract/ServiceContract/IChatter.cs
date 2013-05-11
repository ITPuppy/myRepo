using System.Collections.Generic;
using System.ServiceModel;
using Chatter.Contract.DataContract;


namespace Chatter.Contract.ServiceContract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatterCallback))]

    public interface IChatter
    {
        #region 登录退出
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        Result Login(Member member);
        [OperationContract(IsInitiating = false, IsTerminating = true,IsOneWay=true)]
        void Logoff(Member member);
        #endregion


        #region 获取列表：好友群组
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        List<UserGroup> GetFriends(string id);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        List<Group> GetGroups(string id);


        [OperationContract(IsInitiating = false, IsTerminating = false)]
        bool IsOnlie(string friendId);
        #endregion

        #region 好友操作：添加，删除
        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void AddFriend(string friendId, string userGroupId = "0");


       

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Result DeleteFriend(string id, string userGroupId,string friend);
        #endregion

        #region 群相关
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Result AddGroup(Group group);

        [OperationContract(IsInitiating = false, IsTerminating = false,IsOneWay=true)]
        void AddFriend2Group(string friendId, string groupId);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void DeleteMember(string memberId, string groupId);

        [OperationContract(IsInitiating = false, IsTerminating = false, IsOneWay = true)]
        void DeleteGroup( string groupId);

        #endregion

        #region 发送消息
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        MessageStatus SendMesg(Message mesg);
        #endregion

        
      
        #region 分组相关

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Result AddUserGroup(UserGroup userGroup);
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Result DeleteUserGroup(string id, UserGroup userGroup);
        #endregion


         [OperationContract(IsInitiating = false, IsTerminating = false)]
         void SendHeartBeat();


         [OperationContract(IsInitiating = false, IsTerminating = false)]
         Result ResponseToRequest(Result result);


         
    }
}
