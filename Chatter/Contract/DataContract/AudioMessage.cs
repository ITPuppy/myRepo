using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract.DataContract
{
    [DataContract]
   public class AudioMessage:Message
    {
        [DataMember]
        public MyEndPoint ServerEndPoint { get; set; }
    }
}
