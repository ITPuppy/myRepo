using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Contract.DataContract
{
    [DataContract]
   public class UserGroup:BaseRole
    {
        [DataMember]
        string userGroupId;
        [DataMember]
        string userGroupName;

        [DataMember]
        List<Member> members;

        public List<Member> Members
        {
            get { return members; }
            set { members = value; }
        }
        public string UserGroupName
        {
            get { return userGroupName; }
            set { userGroupName = value; }
        }

        public string UserGroupId
        {
            get { return userGroupId; }
            set { userGroupId = value; }
        }

 
        public override string ToString()
        {
            return userGroupId+":"+userGroupName;
        }
    }
}
