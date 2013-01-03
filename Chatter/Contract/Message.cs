using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace Chatter.Contract
{
    [DataContract]
    [KnownType(typeof(TextMessage))]
    [KnownType(typeof(FileMessage))]
    public class Message
    {
        [DataMember]
        private MessageType type;

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }
        [DataMember]
        private Member from;
        [DataMember]
        private Member to;

        public  Member To
        {
            get { return to; }
            set { to = value; }
        }

        public Member From
        {
            get { return from; }
            set { from = value; }
        }
        [DataMember]
        private DateTime sendTime;

        public DateTime SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }
    }


}
