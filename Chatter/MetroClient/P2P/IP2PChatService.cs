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


        [OperationContract]
        void SendP2PMessage(Member member,string to,Message mesg);

        [OperationContract]
        void AddMember(string  memberId,string groupId);

        [OperationContract]
        void DeleteMember(string memberId,string groupId);
    }
}
