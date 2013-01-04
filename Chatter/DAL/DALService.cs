using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatter.Contract;
using Chatter.Log;

namespace DAL
{
    class DALService
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
                Conn.Open();
                string sql = String.Format("insert into tblMember(id,nickName,password,birthday,sex,status,information) values(@id,@nickName,@password,@birthday,@sex,@status,@information)");
                cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("id", member.Id);
                cmd.Parameters.AddWithValue("password", member.Password);
                cmd.Parameters.AddWithValue("nickName", member.NickName);
                cmd.Parameters.AddWithValue("birthday", member.Birthday);
                cmd.Parameters.AddWithValue("sex", member.Sex);
                cmd.Parameters.AddWithValue("status", member.Status);
                cmd.Parameters.AddWithValue("information", member.Infomation);
                int i1 = cmd.ExecuteNonQuery();

                sql = String.Format("insert into tblFreiend(id) values(@id)");

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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                cmd.Parameters.AddWithValue("id", id);
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
                cmd.Parameters.AddWithValue("id", id);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        temp = reader["friendId"].ToString();
                       
                        friends = GetFriendNames(temp);
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
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
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
    }
}
