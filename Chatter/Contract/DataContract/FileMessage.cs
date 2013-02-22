using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Chatter.Contract.DataContract;

namespace Chatter.Contract.DataContract
{
    [DataContract]
    public class FileMessage:Message
    {
        [DataMember]
        public string FileName
        {
            get;
            set;
        }
        [DataMember]

        public long Size
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
        [DataMember]
        public string Path
        {
            get;
            set;
        }

    }
}
