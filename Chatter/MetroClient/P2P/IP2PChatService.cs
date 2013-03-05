using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.P2P
{
    [ServiceContract(CallbackContract=typeof(IP2PChatService))]
    interface IP2PChatService
    {


        [OperationContract(IsOneWay=true)]
        void SendP2PMessage(Member member,string to,Message mesg);

         [OperationContract(IsOneWay = true)]
        void AddMember(string  memberId,string groupId);

         [OperationContract(IsOneWay = true)]
        void DeleteMember(string memberId,string groupId);
    }
}
