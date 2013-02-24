using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;


namespace Chatter.Contract.DataContract
{
    [DataContract]
   public  class CommandMessage:Message
    {

        [DataMember]
        public MyCommandType CommandType
        {
            get;
            set;
        }
        [DataMember]
        public string Guid
        {
            get;
            set;
        }

    }
}
