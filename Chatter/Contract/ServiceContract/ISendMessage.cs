using System.ServiceModel;
using Chatter.Contract.DataContract;


namespace Chatter.Contract.ServiceContract
{
    [ServiceContract(CallbackContract=typeof(ICallbackSendMessage))]
   public interface ISendMessage
    {
       void SendMesg(Message mesg);
    }
}
