﻿using System;
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
        [DataMember]
        private UserGroup userGroup;

        public UserGroup UserGroup
        {
            get { return userGroup; }
            set { userGroup = value; }
        }
        [DataMember]
        private String mesg;

        public String Mesg
        {
            get { return mesg; }
            set { mesg = value; }
        }

    }
}