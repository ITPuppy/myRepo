using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract
{
    [DataContract]
    public enum MessageStatus
    {
        [EnumMember]
        OK = 0,
        [EnumMember]
        Failed = 1,
        [EnumMember]
        Accept = 2,
        [EnumMember]
        Refuse = 3
    }
}
