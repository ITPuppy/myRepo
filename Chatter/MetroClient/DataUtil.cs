using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Chatter.Log;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient
{
    public class DataUtil
    {
        public static string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static BaseRole CurrentRole;
        public static ChatterClient Client;
        public static List<UserGroup> UserGroups;
        public static Member Member;
        public static Dictionary<string, MyTabItem> FriendTabItems = new Dictionary<string, MyTabItem>();
        /// <summary>
        /// 根据分组id获取好友列表
        /// </summary>
        /// <param name="userGroupId">分组ID</param>
        /// <returns></returns>
        public static List<Member> GetMemberList(string userGroupId)
        {
            List<Member> members = new List<Member>();

            foreach (UserGroup ug in UserGroups)
            {
                if (ug.userGroupId == userGroupId)
                    if (ug.members != null && ug.members.Length > 0)
                        members.AddRange(ug.members);
            }
            return members;
        }
        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="userGroupId">分组ID</param>
        public static void DeleteUserGroup(string userGroupId)
        {
            UserGroup ug = UserGroups.Find(new Predicate<UserGroup>((tempUg) => { return tempUg.userGroupId == userGroupId; }));

            if (ug != null)
            {

                UserGroups.Remove(ug);
            }
            else
                throw new Exception("没找到分组");
        }
        /// <summary>
        /// 全局信息
        /// 将好友加到某个分组
        /// </summary>
        /// <param name="member"></param>
        /// <param name="userGroupId"></param>
        public static void AddMemberTo(Member member, string userGroupId)
        {
            UserGroup ug = UserGroups.Find(new Predicate<UserGroup>((tempUg) => { return tempUg.userGroupId == userGroupId; }));
            List<Member> members = ug.members.ToList<Member>();
            if (members == null)
                members = new List<Member>();
            members.Add(member);
            ug.members = members.ToArray<Member>();

        }
        /// <summary>
        /// 判断是否为好友ID
        /// </summary>
        /// <param name="friendId">id</param>
        /// <returns></returns>
        public static bool IsFriend(string friendId)
        {
            foreach (UserGroup ug in UserGroups)
            {
                if (ug.members.FirstOrDefault(new Func<Member, bool>((member) => { return member.id == friendId; })) != null)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获得用户所在分组ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 全局信息
        /// 从分组中删除好友
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="userGroupId"></param>
        public static void DeleteFriend(string friendId, string userGroupId)
        {
            try
            {
                UserGroup ug = UserGroups.Find(new Predicate<UserGroup>((tempUG) =>
                {
                    return tempUG.userGroupId == userGroupId;
                }));

                if (ug != null)
                {
                    List<Member> members = ug.members.ToList<Member>();

                    members.RemoveAt(members.FindIndex(new Predicate<Member>((member) => { return member.id == friendId; })));
                    ug.members = members.ToArray<Member>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除好友出错");
                MyLogger.Logger.Error("删除好友出错",ex);
            }
        }
    }
}
