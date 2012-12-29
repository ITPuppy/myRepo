using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Contract
{
    [DataContract]
    class MessageContract
    {
        [DataMember]
        private string head;
        [DataMember]
        private string body;
        [DataMember]
        private string tail;

        public string Tail
        {
            get { return tail; }
            set { tail = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Head
        {
            get { return head; }
            set { head = value; }
        }

    }
}
