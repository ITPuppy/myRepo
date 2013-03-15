using Chatter.Contract.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Chatter.Contract.ServiceContract
{
    [ServiceContract]
   public  interface IChatterCallback
    {

        [OperationContract(IsOneWay=true)]
        void OnLogin(string id);

        [OperationContract(IsOneWay=true)]
        void OnSendMessage(Message mesg);

        [OperationContract(IsOneWay = true)]
        void OnLogoff(string id);

        [OperationContract(IsOneWay = true)]
        void RequestToTargetClient(Message mesg);

        [OperationContract(IsOneWay = true)]
        void ReponseToSouceClient(Result result);

        [OperationContract(IsOneWay = true)]
        void SendUDPEndPoint(MyEndPoint endPoint,Member member,bool isFrom);

        [OperationContract(IsOneWay = true)]
        void SendTCPEndPoint(MyEndPoint endPoint, string guid);
    }
}
