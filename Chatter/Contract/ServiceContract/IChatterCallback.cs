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
        void OnSendMessage(Result result);

        [OperationContract(IsOneWay = true)]
        void OnLogoff(string id);
    }
}
