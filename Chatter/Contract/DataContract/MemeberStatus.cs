using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Chatter.Contract.DataContract
{
    [DataContract]
    public enum MemberStatus
    {
        [EnumMember]
        Online,
        [EnumMember]
        Offline,
        [EnumMember]
        Levave,
        [EnumMember]
        Busy
    }
}
