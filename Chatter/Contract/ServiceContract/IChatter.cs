using System.Collections.Generic;
using System.ServiceModel;
using Chatter.Contract.DataContract;


namespace Chatter.Contract.ServiceContract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatterCallback))]
    
    public interface IChatter
    {
         [OperationContract(IsInitiating = true, IsTerminating = false)]
        Result Login(Member member);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
         List<UserGroup> GetFriends(string id);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        List<Group> GetGroups(string id);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void AddFriend( string friendId, string userGroupId = "0");
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Result ResponseToAddFriend(Result result);


         [OperationContract(IsInitiating = false, IsTerminating = false)]
        MessageStatus AddGroup(Group group);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        MessageStatus AddFriend2Group(string friendId, string groupId);

         [OperationContract(IsInitiating = false, IsTerminating = false)]
        MessageStatus SendMesg(Message mesg);

         [OperationContract(IsInitiating = false, IsTerminating = false)]
         Result AddUserGroup(UserGroup userGroup);
         [OperationContract(IsInitiating = false, IsTerminating = false)]
         Result DeleteUserGroup(string id,UserGroup userGroup);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        MessageStatus Logoff(Member member);                                                                                                                                                                                                                                                                                                                                          
        
    }
}
