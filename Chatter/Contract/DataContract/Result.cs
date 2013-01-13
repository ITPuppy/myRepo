using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract.DataContract
{
    [DataContract]
    public class Result
    {
        [DataMember]
        private Guid guid;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        [DataMember]
        private MessageStatus status;

        public MessageStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        private Member member;

        public Member Member
        {
            get { return member; }
            set { member = value; }
        }
    }
}
