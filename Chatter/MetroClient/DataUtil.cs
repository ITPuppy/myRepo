using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient
{
    public class DataUtil
    {
        public static ChatterClient Client;
        public static List<UserGroup> UserGroups;
        public static Member Member;
        public static Dictionary<string, MyTabItem> FriendTabItems = new Dictionary<string, MyTabItem>();
        public static List<Member> GetMemberList(string userGroupId)
        {
            List<Member> members = new List<Member>();

            foreach (UserGroup ug in UserGroups)
            {
                if(ug.userGroupId==userGroupId)
                if(ug.members!=null&&ug.members.Length>0)
                    members.AddRange(ug.members);
            }
            return members;
        }

        public static void DeleteUserGroup(string userGroupId)
        {
           UserGroup ug =UserGroups.Find(new Predicate<UserGroup>((tempUg)=>{return tempUg.userGroupId==userGroupId;}));
            
            if(ug!=null)
            {

                UserGroups.Remove(ug);
            }
            else throw new Exception("没找到分组");
        }

        internal static void AddMemberTo(Member member, string userGroupId)
        {
            UserGroup ug = UserGroups.Find(new Predicate<UserGroup>((tempUg) => { return tempUg.userGroupId == userGroupId; }));
            List<Member> members= ug.members.ToList<Member>();
            if (members == null)
                members = new List<Member>();
            members.Add(member);
            ug.members = members.ToArray<Member>();

        }

        public  static bool IsFriend(string friendId)
        {
            foreach (UserGroup ug in UserGroups)
            {
                if(ug.members.FirstOrDefault(new Func<Member,bool>((member)=>{return member.id==friendId;}))!=null)
                    return true;
            }
            return false;
        }

        public static string GetUserGroupIdByMember(string id)
        {
            foreach (UserGroup ug in UserGroups)
            {
                foreach (Member member in ug.members)
                {
                    if (member.id == id)
                        return ug.userGroupId;
                }
            }
            return null;
        }
    }
}
