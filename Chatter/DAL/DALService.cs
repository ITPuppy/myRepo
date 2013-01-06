using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Chatter.Log;
using Chatter.Contract.DataContract;

namespace Chatter.DAL
{
    public class DALService
    {
        [Obsolete("Please use Conn")]
        private static SqlConnection conn;
        private static Object obj = new object();
        private static SqlConnection Conn
        {
            get
            {
                lock (obj)
                {
                    if (conn == null)
                    {
                        conn = new SqlConnection();
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
            SqlCommand cmd = null;
            try
            {
              
                string sql = String.Format("insert into tblMember(id,nickName,password,birthday,sex,status,information) values(@id,@nickName,@password,@birthday,@sex,@status,@information)");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", member.Id);
                cmd.Parameters.AddWithValue("password", member.Password);
                cmd.Parameters.AddWithValue("nickName", member.NickName);
                cmd.Parameters.AddWithValue("birthday", member.Birthday);
                cmd.Parameters.AddWithValue("sex", member.Sex);
                cmd.Parameters.AddWithValue("status", member.Status);
                cmd.Parameters.AddWithValue("information", member.Infomation);
                Prepare(cmd.Parameters);
                int i1 = cmd.ExecuteNonQuery();

                sql = String.Format("insert into tblFriend(id) values(@id)");
                cmd.CommandText = sql;
                int i2=cmd.ExecuteNonQuery();


                return i1 == 1&&i2==1;
            }
            catch (Exception e)
            {
                Logger.Error("添加用户出现错误\n" + e.Message);

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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set status=@status where id=@id");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("status", status.ToString());
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("更新状态出现错误\n" + e.Message);
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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set information=@information where id=@id");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("information", information.ToString());
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("更新资料出现错误\n" + e.Message);
                return false;
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
        /// <returns></returns>
        static public bool AddFriend(string id, string friendId)
        {
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblFriend set friendId=friendId+@friendId where id=@id");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("friendId", friendId+";");
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("添加好友出现错误\n" + e.Message);
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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("select id from tblMember where id=@id");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);


                return null!=cmd.ExecuteScalar(); ;

            }
            catch (Exception e)
            {
                Logger.Error("查询用户存在出现错误\n" + e.Message);
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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblMember set password=@password,sex=@sex,nickName=@nickName,status=@status,information=@information birthday=@birthday where id=@id");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", member.Id);
                cmd.Parameters.AddWithValue("password", member.Password);
                cmd.Parameters.AddWithValue("nickName", member.NickName);
                cmd.Parameters.AddWithValue("birthday", member.Birthday);
                cmd.Parameters.AddWithValue("sex", member.Sex);
                cmd.Parameters.AddWithValue("status", member.Status);
                cmd.Parameters.AddWithValue("information", member.Infomation);

                Prepare(cmd.Parameters);

                return 1== cmd.ExecuteNonQuery(); ;

            }
            catch (Exception e)
            {
                Logger.Error("更新用户信息出现错误\n" + e.Message);
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
        /// <param name="friendId">好友id</param>
        /// <returns></returns>
        static public bool DeleteFriend(string id, string friendId)
        {
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("select friendId from tblFriend  where id=@id");
                string temp = String.Empty;
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            temp=reader["friendId"].ToString();
                            Logger.Debug("删除前"+temp);
                            temp=temp.Replace(friendId+";","");
                            Logger.Debug("删除后"+temp);
                        }
                        else
                        {
                            Logger.Error("删除好友时候出现错误\n");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.Error("删除好友时候没有找到用户id\n");
                        return false;
                    }
                }
                cmd.Dispose();


                sql = String.Format("update tblFriend set friendId=@friendId where id=@id");
                cmd = new SqlCommand(sql, Conn);
                
                cmd.Parameters.AddWithValue("friendId", temp );
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("删除好友出现错误\n" + e.Message);
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
        static public List<string> GetFriendList(string id)
        {
            SqlCommand cmd = null;
            List<String> friends =null;
            try
            {
                string sql = String.Format("select friendId from tblFriend  where id=@id");
                string temp = String.Empty;
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", id);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        temp = reader["friendId"].ToString();
                       
                        friends = GetNames(temp);
                    }
                    else
                    {
                        Logger.Debug("获取好友列表失败");
                        return null;
                    }
                    
                }

                return friends;
                

            }
            catch (Exception e)
            {
                Logger.Error("删除好友出现错误\n" + e.Message);
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

            while (temp.Length!=0)
            {
                int index=temp.IndexOf(';');
                names.Add(temp.Substring(0, index));
                temp = temp.Substring(index+1);
            }
            return names;
        }


        /// <summary>
        /// 添加群组
        /// </summary>
        /// <param name="group">群组</param>
        /// <returns></returns>
        public static bool AddGroup(Group group)
        {
            SqlCommand cmd = null;
            try
            {
              
                string sql = String.Format("insert into tblGroup(groupId,name,ownerId,groupMember) values(@groupId,@name,@ownerId,@groupMember)");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId",group.GroupId);
                cmd.Parameters.AddWithValue("name", group.Name);
                cmd.Parameters.AddWithValue("ownerId", group.OwnerId);
                Prepare(cmd.Parameters);
                StringBuilder sb=new StringBuilder();
                foreach (string temp in group.GroupMember)
                {
                    sb.Append(temp+";");
                }
                cmd.Parameters.AddWithValue("groupMember",sb.ToString());

                int i1 = cmd.ExecuteNonQuery();


               


                return i1 == 1 ;
            }
            catch (Exception e)
            {
                Logger.Error("添加群组出现错误\n" + e.Message);

                return false;
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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("select groupId from tblGroup where groupId=@groupId");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId", groupId);
                Prepare(cmd.Parameters);
                return null != cmd.ExecuteScalar(); ;

            }
            catch (Exception e)
            {
                Logger.Error("查询群组存在出现错误\n" + e.Message);
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

            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("delete  from tblGroup  where groupId=@groupId");
                string temp = String.Empty;
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId",groupId);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();
                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("删除群组出现错误\n" + e.Message);
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
        /// <param name="groupId">组id</param>
        /// <param name="memberId">用户id</param>
        /// <returns></returns>
        public static bool AddMember2Group(string groupId,string memberId)
        {
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("update tblGroup set groupMemeber=groupMember+@groupMember where groupId=@groupId");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId",groupId);
                cmd.Parameters.AddWithValue("groupMember",memberId+";");
                Prepare(cmd.Parameters);
                return cmd.ExecuteNonQuery()==1;
            }
            catch (Exception e)
            {
                Logger.Error("添加用户到组时出现错误："+e.Message);
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
            SqlCommand cmd = null;
            try
            {
                string sql = String.Format("select groupMember from tblGroup  where groupId=@groupId");
                string temp = String.Empty;
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("groupId",groupId);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            temp = reader["groupMember"].ToString();
                            Logger.Debug("删除前" + temp);
                            temp = temp.Replace(groupId + ";", "");
                            Logger.Debug("删除后" + temp);
                        }
                        else
                        {
                            Logger.Error("从群组中删除用户时候出现错误\n");
                            return false;
                        }
                    }
                    else
                    {
                        Logger.Error("从群组中删除用户时候没有找到用户id\n");
                        return false;
                    }
                }
                cmd.Dispose();


                sql = String.Format("update tblGroup set groupMember=@groupMember where groupId=@groupId");
                cmd = new SqlCommand(sql, Conn);

                cmd.Parameters.AddWithValue("groupId", temp);
                Prepare(cmd.Parameters);
                int i = cmd.ExecuteNonQuery();

                return i == 1;

            }
            catch (Exception e)
            {
                Logger.Error("从群组中删除用户出现错误\n" + e.Message);
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
            SqlCommand cmd = null;
            List<Group> groups = new List<Group>();
            try
            {
                string sql = String.Format("select * from tblGroup where ownerId=@ownerId");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("ownerId",id);
                Prepare(cmd.Parameters);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Group g = new Group();
                        g.GroupId = reader["groupId"].ToString();
                        g.Name = reader["name"].ToString();
                        g.OwnerId = reader["ownerId"].ToString();
                        g.GroupMember = GetNames(reader["groupMember"].ToString());
                        groups.Add(g);
                    }
                }
                cmd.CommandText = String.Format("select * from tblGroup where groupMember like %@groupMember%");
                cmd.Parameters.AddWithValue("groupMember",id);
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
                        g.GroupMember = GetNames(reader["groupMember"].ToString());
                        groups.Add(g);
                    }
                }

                return groups;

            }
            catch (Exception e)
            {
                Logger.Error("获取群组时候出现错误"+e.Message);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
              
 
            }
        }



        private static void Prepare(SqlParameterCollection parameters)
        {
            foreach (SqlParameter paramenter in parameters)
            {
                if (paramenter.Value == null)
                    paramenter.Value = DBNull.Value;
            }
        }
    }
}
