using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Chatter.Contract.DataContract
{
    [DataContract]
    public class Group:BaseRole
    {
      
   
        [DataMember]
        public List<Member> GroupMember
        {
            get;
            set;
        }
         [DataMember]
        public string OwnerId
        {
            get;
            set;
        }
          [DataMember]
        public string GroupId
        {
            get;
            set;
        }
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }
}
