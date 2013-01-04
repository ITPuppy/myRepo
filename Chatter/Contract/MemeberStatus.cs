using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Contract
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
