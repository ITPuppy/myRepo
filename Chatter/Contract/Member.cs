using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Contract
{
    [DataContract]
    public class Member
    {
        [DataMember]
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        private string sex;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        [DataMember]
        private DateTime birthday;

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        [DataMember]
        private string nickName;

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        [DataMember]
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        [DataMember]
        private MemberStatus status;

        public MemberStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        [DataMember]
        private string infomation;

        public string Infomation
        {
            get { return infomation; }
            set { infomation = value; }
        }
    }
}
