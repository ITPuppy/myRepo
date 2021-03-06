﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;


namespace Chatter.Contract.DataContract
{
    [DataContract]
    [KnownType(typeof(TextMessage))]
    [KnownType(typeof(FileMessage))]
    [KnownType(typeof(CommandMessage))]
    [KnownType(typeof(AudioMessage))]
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
        private BaseRole from;
        [DataMember]
        private BaseRole to;

        public  BaseRole To
        {
            get { return to; }
            set { to = value; }
        }

        public BaseRole From
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
