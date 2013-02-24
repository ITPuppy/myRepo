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
        string groupId;
        [DataMember]
        string ownerId;
        [DataMember]
        List<string> groupMember;

        public List<string> GroupMember
        {
            get { return groupMember; }
            set { groupMember = value; }
        }
        public string OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }
        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        [DataMember]
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
