using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroClient.ChatterService;

namespace Chatter.MetroClient
{
    public class DataUtil
    {

        public static List<UserGroup> UserGroups;

        public static List<Member> GetMemberList(string userGroupId)
        {
            List<Member> members = new List<Member>();

            foreach (UserGroup ug in UserGroups)
            {
                if(ug.members!=null&&ug.members.Length>0)
                    members.AddRange(ug.members);
            }
            return members;
        }

        public static void DeleteUserGroup(string userGroupId)
        {
           UserGroup ug =UserGroups.Find(new Predicate<UserGroup>((tempUg)=>{return tempUg.userGroupId==userGroupId;}));
             UserGroup defaultUG =UserGroups.Find(new Predicate<UserGroup>((tempUg)=>{return tempUg.userGroupId=="0";}));
            if(ug!=null&&defaultUG!=null)
            {

                List<Member> ugList = defaultUG.members.ToList<Member>();
                if(ug.members!=null&&ug.members.Length>0)
                    ugList.AddRange(ug.members);
                defaultUG.members = ugList.ToArray<Member>();

                UserGroups.Remove(ug);
            }
            else throw new Exception("没找到分组");
        }
    }
}
