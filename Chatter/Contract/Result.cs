﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract
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
        private Status status;

        public Status Status
        {
            get { return status; }
            set { status = value; }
        }


    }
}
