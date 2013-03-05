using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract.DataContract
{
    /// <summary>
    /// 结果
    /// </summary>
    [DataContract]
    public class Result
    {


        [DataMember]
        public MessageType Type
        {
            get;
            set;
        }

        [DataMember]
        public Group Group
        {
            get;
            set;
        }



        [DataMember]
        public MessageStatus Status
        {
            get;
            set;
        }

        [DataMember]
        public Member Member
        {
            get;
            set;
        }
        [DataMember]
        public UserGroup UserGroup
        {
            get;
            set;
        }
        [DataMember]
        public String Mesg
        {
            get;
            set;
        }
        [DataMember]
        public MyEndPoint EndPoint
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
