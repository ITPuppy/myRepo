using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Chatter.Contract
{
    [ServiceContract]
   public  interface ICallbackSendMessage
    {
        void SendMessageCallback(Result result);
    }
}
