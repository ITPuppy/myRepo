using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Chatter.Contract.DataContract
{
    [DataContract]
   public class MyEndPoint
    {
        [DataMember]
        public string Address
        {
            get;
            set;
        }
        [DataMember]
        public int Port
        {
            get;
            set;
        }
    }
}
