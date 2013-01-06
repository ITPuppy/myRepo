using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Contract.DataContract
{
    [DataContract]
   public  class TextMessage:Message
    {
        [DataMember]
        private string msg;

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
    }
}
