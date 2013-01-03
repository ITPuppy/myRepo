using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;


namespace Chatter.Contract
{
    [ServiceContract(CallbackContract=typeof(ICallbackSendMessage))]
   public interface ISendMessage
    {
       void SendMesg(Message mesg);
    }
}
