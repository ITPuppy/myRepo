using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Chatter.Contract.DataContract
{
     [DataContract]
    public class Friend:BaseRole
    {
         [DataMember]
        Member member;
         
        public Member Member
        {
            get { return member; }
            set { member = value; }
        }
         [DataMember]
        string userGroupId;

        public string UserGroupId
        {
            get { return userGroupId; }
            set { userGroupId = value; }
        }
         [DataMember]
        string userGroupName;

        public string UserGroupName
        {
            get { return userGroupName; }
            set { userGroupName = value; }
        }
    }
}
