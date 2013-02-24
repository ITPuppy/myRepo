using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chatter.DAL
{
    class Tuple
    {
       

        public Tuple(string groupId, string groupName, List<string> friends)
        {
           
            this.GroupId = groupId;
            this.GroupName = groupName;
            this.Friends = friends;
        }
        public string GroupId
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
        public List<string> Friends
        {
            get;
            set;
        }
    }
}
