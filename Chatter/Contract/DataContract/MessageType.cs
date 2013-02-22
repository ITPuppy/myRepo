using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract.DataContract
{
    [DataContract]
    public enum MessageType
    {
        [EnumMember]
        TextMessage,
        [EnumMember]
        File,
        [EnumMember]
        Video,
        [EnumMember]
        Audio,
        [EnumMember]
        Command,
        [EnumMember]
        Login,
        [EnumMember]
        Logoff,
        [EnumMember]
        AddFriend,
        [EnumMember]
        AddGroup,
        [EnumMember]
        AddFriend2Group,
     
    }
}
