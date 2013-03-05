using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Text;
using Chatter.Contract.DataContract;
using System.Data;
using Chatter.Log;


namespace Chatter.DAL
{
    public class DALService
    {



        [Obsolete("Please use Conn")]
        private static MySqlConnection conn;
        private static Object obj = new object();
        private static MySqlConnection Conn
        {

            get
            {
                lock (obj)
                {
                    if (conn == null)
                    {
                        conn = new MySqlConnection();
                        conn.ConnectionString = ConfigurationManager.AppSettings["connStr"];
                        conn.Open();
                    }

                    else if (conn.State == ConnectionState.Closed)
                    {
                        conn.ConnectionString = ConfigurationManager.AppSettings["connStr"];
                        conn.Open();
                    }
                    else if (conn.State == ConnectionState.Broken)
                    {
                        conn.Close();
                        conn.ConnectionString = ConfigurationManager.AppSettings["connStr"];
                        conn.Open();
                    }
                    return conn;
                }
            }

        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="member">用户</param>
        /// <returns>添加成功返回true，添加失败返回false</returns>
        static public bool AddMember(Member member)
        {

            MySqlCommand cmd = null;
            try
            {

                string sql = String.Format("insert into tblMember(id,nickName,password,birthday,sex,status,information) values(?id,?nickName,?password,?birthday,?sex,?status,?information)");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", member.Id);
                cmd.Parameters.AddWithValue("password", member.Password);
                cmd.Parameters.AddWithValue("nickName", member.NickName);
                cmd.Parameters.AddWithValue("birthday", member.Birthday);
                cmd.Parameters.AddWithValue("sex", member.Sex);
                cmd.Parameters.AddWithValue("status", member.Status);
                cmd.Parameters.AddWithValue("information", member.Infomation);
                Prepare(cmd.Parameters);
                int i1 = cmd.ExecuteNonQuery();

                sql = String.Format("insert into tblFriend(id,groupId,groupName) values(?id,?groupId,?groupName)");
                cmd.Parameters.AddWithValue("groupId", "0");
                cmd.Parameters.AddWithValue("groupName", "我的好友");
                Prepare(cmd.Parameters);
                cmd.CommandText = sql;
                int i2 = cmd.ExecuteNonQuery();


                return i1 == 1 && i2 == 1;
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("添加用户出现错误\n", e);

                return false;
            }

            finally
            {

                if (cmd != null)
                    cmd.Dispose();


            }
        }


        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        static public bool UpdateMemberStatus(string id, MemberStatus status)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set status=?status where id=?id");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("status", status.ToString());
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("更新状态出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }

        /// <summary>
        /// 更新用户资料
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="information">用户资料</param>
        /// <returns></returns>
        static public bool UpdateMemberInfomation(string id, string information)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set information=?information where id=?id");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("information", information.ToString());
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("更新资料出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        static public Member GetMember(string id)
        {
            MySqlCommand cmd = null;
            Member member = null;
            try
            {

                string sql = String.Format("select * from tblMember where id=?id");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        member = new Member();
                        member.Id = id;
                        member.Password = reader["password"].ToString();
                        member.Sex = reader["sex"].ToString();
                        member.NickName = reader["nickName"].ToString();
                        member.Birthday = Convert.ToDateTime(reader["birthday"]);
                        member.Infomation = reader["information"].ToString();
                        if (reader["status"].ToString().Length != 0)
                        {
                            MemberStatus status;
                            // Enum.TryParse<MemberStatus>(reader["status"].ToString(),out status);
                            status = (MemberStatus)Enum.Parse(typeof(MemberStatus), reader["status"].ToString());
                            member.Status = status;
                        }

                    }
                }

                return member;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("获得用户出现错误\n", e);

                return null;
            }

            finally
            {

                if (cmd != null)
                    cmd.Dispose();


            }
        }
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="id">自己id</param>
        /// <param name="friendId">好友id</param>
        /// <param name="userGroupId">分组id</param>
        /// <returns></returns>
        static public Member AddFriend(string id, string friendId, string userGroupId = "0")
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblFriend set friendId=CONCAT(friendId,?friendId) where id=?id and groupId=?groupId;");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("friendId", friendId + ";");
                cmd.Parameters.AddWithValue("groupId", userGroupId);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                if (i == 1)
                {

                    if (friendId.IndexOf(";") != -1)
                    {
                        ///说明是deleteUserGroup调用，用于转移好友
                        friendId = friendId.Substring(0, friendId.IndexOf(";"));
                    }
                    return GetMember(friendId);
                }
                else return null;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("添加好友出现错误\n", e);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }


        /// <summary>
        /// 判断是否为自己好友
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="friendId">好友ID</param>
        /// <returns></returns>
        static public bool IsFriend(string id, string friendId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select id from tblFriend where id=?id and friendId like ?friendId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("friendId", "%" + friendId + "%");
                Prepare(cmd.Parameters);

                return cmd.ExecuteScalar() != null;
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("查询是否为好友的时候出现错误", ex);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 判断用户是否合法
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        static public bool IsMember(string id, string pwd)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select id from tblMember where id=?id and password=?password");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("password", pwd);
                Prepare(cmd.Parameters);
                return cmd.ExecuteScalar() != null;
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("查询是否为合法用户时出现错误", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        static public bool IsExistMember(string id)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select id from tblMember where id=?id");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);


                return null != cmd.ExecuteScalar(); ;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("查询用户存在出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="member">用户</param>
        /// <returns></returns>
        static public bool UpdateMemeber(Member member)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set password=?password,sex=?sex,nickName=?nickName,status=?status,information=?information birthday=?birthday where id=?id");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", member.Id);
                cmd.Parameters.AddWithValue("password", member.Password);
                cmd.Parameters.AddWithValue("nickName", member.NickName);
                cmd.Parameters.AddWithValue("birthday", member.Birthday);
                cmd.Parameters.AddWithValue("sex", member.Sex);
                cmd.Parameters.AddWithValue("status", member.Status);
                cmd.Parameters.AddWithValue("information", member.Infomation);

                Prepare(cmd.Parameters);

                return 1 == cmd.ExecuteNonQuery(); ;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("更新用户信息出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="userGroupId">分组id</param>
        /// <param name="friendId">好友id</param>
        /// <returns></returns>
        static public bool DeleteFriend(string id, string userGroupId, string friendId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select friendId from tblFriend  where id=?id and groupId=?groupId");
                string temp = String.Empty;
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("groupId", userGroupId);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            temp = reader["friendId"].ToString();
                            MyLogger.Logger.Debug("删除前" + temp);


                            temp = temp.Replace(friendId + ";", "");
                            MyLogger.Logger.Debug("删除后" + temp);


                        }
                        else
                        {
                            MyLogger.Logger.Error("删除好友时候出现错误\n");
                            return false;
                        }
                    }
                    else
                    {
                        MyLogger.Logger.Error("删除好友时候没有找到用户id\n");
                        return false;
                    }
                }
                cmd.Dispose();


                sql = String.Format("update tblFriend set friendId=?friendId where id=?id  and groupId=?groupId ");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("friendId", temp);
                cmd.Parameters.AddWithValue("groupId", userGroupId);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("删除好友出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }



        /// <summary>
        /// 获取好友id的List
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        static public List<UserGroup> GetFriendList(string id)
        {
            MySqlCommand cmd = null;
            List<UserGroup> userGroups = new List<UserGroup>();
            List<String> friends = null;
            try
            {
                string sql = String.Format("select friendId,groupId,groupName from tblFriend  where id=?id ");
                string temp = String.Empty;
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);
                List<Tuple> userGroupList = new List<Tuple>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        string groupId = reader["groupId"].ToString();
                        string groupName = reader["groupName"].ToString();

                        temp = reader["friendId"].ToString();
                        friends = GetNames(temp);

                        userGroupList.Add(new Tuple(groupId, groupName, friends));


                    }


                }

                foreach (Tuple tuple in userGroupList)
                {
                    List<Member> members = new List<Member>();
                    foreach (string tempid in tuple.Friends)
                    {
                        Member member = GetMember(tempid);
                        if (member != null)
                            members.Add(member);
                    }
                    UserGroup userGroup = new UserGroup();
                    userGroup.UserGroupId = tuple.GroupId;
                    userGroup.UserGroupName = tuple.GroupName;
                    userGroup.Members = members;
                    userGroups.Add(userGroup);

                }

                return userGroups;


            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("获取好友列表出现错误\n", e);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }


        /// <summary>
        /// 将id；id；分解成字符串的List
        /// </summary>
        /// <param name="temp">id长串,id可以是group中的用户id也可以是好友的用户id</param>
        /// <returns></returns>
        private static List<string> GetNames(string temp)
        {
            List<string> names = new List<string>();

            while (temp.Length != 0)
            {
                int index = temp.IndexOf(';');
                names.Add(temp.Substring(0, index));
                temp = temp.Substring(index + 1);
            }
            return names;
        }


        /// <summary>
        /// 添加群组
        /// </summary>
        /// <param name="group">群组</param>
        /// <returns></returns>
        public static string AddGroup(Group group)
        {
            MySqlCommand cmd = null;
            try
            {
                group.GroupId = NewGroupId(6);

                string sql = String.Format("insert into tblGroup(groupId,name,ownerId,groupMember) values(?groupId,?name,?ownerId,?groupMember)");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", group.GroupId);
                cmd.Parameters.AddWithValue("name", group.Name);
                cmd.Parameters.AddWithValue("ownerId", group.OwnerId);
                Prepare(cmd.Parameters);
                StringBuilder sb = new StringBuilder();
                if (group.GroupMember != null)
                    foreach (Member tempMember in group.GroupMember)
                    {
                        sb.Append(tempMember.Id + ";");
                    }
                cmd.Parameters.AddWithValue("groupMember", sb.ToString());

                int i1 = cmd.ExecuteNonQuery();




                if (i1 == 1)
                    return group.GroupId;
                else return null;
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("添加群组出现错误\n", e);

                return null;
            }

            finally
            {

                if (cmd != null)
                    cmd.Dispose();


            }
        }
        /// <summary>
        /// 群组是否存在
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <returns></returns>
        public static bool IsExistGroup(string groupId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select groupId from tblGroup where groupId=?groupId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", groupId);
                Prepare(cmd.Parameters);
                return null != cmd.ExecuteScalar(); ;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error(String.Format("查询群组存在出现错误\n", e));
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="gourpId"></param>
        /// <returns></returns>
        public static bool DeleteGroup(string groupId)
        {

            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("delete  from tblGroup  where groupId=?groupId");
                string temp = String.Empty;
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", groupId);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();
                return i == 1;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("删除群组出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 添加用户到组
        /// </summary>
        /// <param name="memberId">好友id</param>
        /// <param name="groupId">组id</param>
        /// <returns></returns>
        public static bool AddMember2Group(string memberId, string groupId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblGroup set groupMemeber=CONCAT（groupMember，?groupMember） where groupId=?groupId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", groupId);
                cmd.Parameters.AddWithValue("groupMember", memberId + ";");
                Prepare(cmd.Parameters);
                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("添加用户到组时出现错误：", e);
                return false;
            }
            finally
            {
                cmd.Dispose();

            }
        }
        /// <summary>
        /// 从群组中删除用户
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static bool DeleteMemberFromGroup(string groupId, string memberId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select groupMember from tblGroup  where groupId=?groupId");
                string temp = String.Empty;
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", groupId);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            temp = reader["groupMember"].ToString();
                            MyLogger.Logger.Debug("删除前" + temp);
                            temp = temp.Replace(groupId + ";", "");
                            MyLogger.Logger.Debug("删除后" + temp);
                        }
                        else
                        {
                            MyLogger.Logger.Error("从群组中删除用户时候出现错误\n");
                            return false;
                        }
                    }
                    else
                    {
                        MyLogger.Logger.Error("从群组中删除用户时候没有找到用户id\n");
                        return false;
                    }
                }
                cmd.Dispose();


                sql = String.Format("update tblGroup set groupMember=?groupMember where groupId=?groupId");
                cmd = new MySqlCommand(sql, Conn);

                cmd.Parameters.AddWithValue("groupId", temp);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("从群组中删除用户出现错误\n", e);
                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 获取组列表
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>

        public static List<Group> GetGroup(string id)
        {
            MySqlCommand cmd = null;
            List<Group> groups = new List<Group>();
            Dictionary<string, List<string>> tempIds = new Dictionary<string, List<string>>();
            try
            {
                string sql = String.Format("select * from tblGroup where ownerId=?ownerId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("ownerId", id);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Group g = new Group();
                        g.GroupId = reader["groupId"].ToString();
                        g.Name = reader["name"].ToString();
                        g.OwnerId = reader["ownerId"].ToString();
                        tempIds.Add(g.GroupId, GetNames(reader["groupMember"].ToString()));
                        groups.Add(g);
                    }
                }
                cmd.Parameters.Clear();
                cmd.CommandText = String.Format("select * from tblGroup where groupMember like ?groupMember");
                cmd.Parameters.AddWithValue("groupMember", "%"+id+"%");
                cmd.Parameters.AddWithValue("ownerId", id);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Group g = new Group();
                        g.GroupId = reader["groupId"].ToString();
                        g.Name = reader["name"].ToString();
                        g.OwnerId = reader["ownerId"].ToString();
                        tempIds.Add(g.GroupId, GetNames(reader["groupMember"].ToString()));
                        groups.Add(g);
                    }
                }

                foreach (Group g in groups)
                {

                    List<Member> members = new List<Member>();

                    foreach (string tempId in tempIds[g.GroupId])
                    {
                        members.Add(GetMember(tempId));
                    }
                    g.GroupMember = members;

                }

                return groups;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("获取群组时候出现错误", e);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();


            }
        }


        /// <summary>
        /// 获得所有群组的ID
        /// </summary>
        /// <returns></returns>

        public static List<string> GetAllGroupID()
        {
            MySqlCommand cmd = null;
            List<string> ids = new List<string>();
        
            try
            {
                string sql = String.Format("select groupId from tblGroup");
                cmd = new MySqlCommand(sql, Conn);
               
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        ids.Add(reader["groupId"].ToString());
                       
                     
                    }
                }
               
               
                
                return ids;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("获取群组时候出现错误", e);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();


            }
        }


        private static void Prepare(MySqlParameterCollection parameters)
        {
            foreach (MySqlParameter paramenter in parameters)
            {
                if (paramenter.Value == null)
                    paramenter.Value = DBNull.Value;
            }
        }

        /// <summary>
        ///判断分组是否存在
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="userGroupId">分组id</param>
        /// <returns></returns>
        private static bool IsExistUserGroup(string id, string userGroupId)
        {
            MySqlCommand cmd = null;
            try
            {
                string sql = String.Format("select id from tblFriend where id=?id and groupId=?userGroupId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("userGroupId", userGroupId);
                Prepare(cmd.Parameters);
                return null != cmd.ExecuteScalar(); ;

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("查询分组存在出现错误\n", e);
                return true;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }
        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="userGroupName">分组名</param>
        /// <returns></returns>
        public static string AddUserGroup(string id, string userGroupName)
        {
            MySqlCommand cmd = null;
            try
            {

                string sql = String.Format("insert into tblFriend(id,groupId,groupName) values(?id,?groupId,?groupName)");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                string groupId = NewUserGroupId(2, id);
                cmd.Parameters.AddWithValue("groupId", groupId);
                cmd.Parameters.AddWithValue("groupName", userGroupName);
                Prepare(cmd.Parameters);


                int i1 = cmd.ExecuteNonQuery();





                return groupId;
            }
            catch (Exception e)
            {
                MyLogger.Logger.Error("添加分组出现错误\n", e);

                return null;
            }

            finally
            {

                if (cmd != null)
                    cmd.Dispose();


            }
        }

        /// <summary>
        /// 将好友移动至其他分组
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="friendId">好友id</param>
        /// <param name="fromId">之前分组id</param>
        /// <param name="toId">移动后分组id</param>
        /// <returns></returns>
        public static bool MoveFriendToUserGroup(string id, string friendId, string fromId, string toId)
        {
            if (!DeleteFriend(id, fromId, friendId))
            {
                MyLogger.Logger.Error("移动好友时候，删除好友出错");
                return false;
            }
            return AddFriend(id, friendId, toId) == null ? false : true;

        }

        /// <summary>
        /// 删除用户分组
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="userGroupId">分组id</param>
        /// <returns></returns>
        public static bool DeleteUserGroup(string id, string userGroupId)
        {
            if (userGroupId == "0")
                return false;

            MySqlCommand cmd = null;

            try
            {
                string sql = String.Format("select friendId from tblFriend  where id=?id and groupId=?groupId");
                string temp = String.Empty;
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("groupId", userGroupId);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        temp = reader["friendId"].ToString();

                    }


                }

                if (temp != null && temp.Length != 0)
                {
                    temp = temp.Remove(temp.LastIndexOf(";"));
                    if (AddFriend(id, temp, "0") == null)
                        throw new Exception("转移好友失败");

                }
                sql = String.Format("delete from tblFriend  where id=?id and groupId=?groupId");
                cmd = new MySqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("groupId", userGroupId);
                Prepare(cmd.Parameters);
                return 1 == cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MyLogger.Logger.Error(String.Format("删除分组出现错误\n", e));

                return false;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

            }
        }

        /// <summary>
        /// 获得分组id
        /// </summary>
        /// <param name="length">分组id长度</param>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        private static string NewUserGroupId(int length, string id)
        {
            string userGroupid = NewRandom(length);
            while (IsExistUserGroup(id, userGroupid))
            {
                userGroupid = NewRandom(length);
            }

            return userGroupid;

        }




        private static string NewGroupId(int length)
        {
            string groupId = NewRandom(length);
            while (IsExistGroup(groupId))
            {
                groupId = NewRandom(length);
            }
            return groupId;
        }

        private static string NewRandom(int length)
        {
            Random random = new Random();
            int id = random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length));
            return id.ToString();
        }

        
    }
}
