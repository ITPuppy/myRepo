using Chatter.Contract.DataContract;
using Chatter.Contract.ServiceContract;
using Chatter.DAL;
using Chatter.Log;
using System;
using System.ServiceModel;

namespace Chatter.Service
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    public class RegisterService:IRegister
    {
        
        public Member Register(Member member)
        {
            string id = NewId(7);
            member.Id = id;
            if (DALService.AddMember(member))
            {
                return member;
            }
            else
            {
                Logger.Warn("用户添加失败");
                return null;
            }
        }
        private string NewId(int length)
        {
            string id=NewRandom(length);
            while (DALService.IsExistMember(id))
            {
                id = NewRandom(length);
            }
            Logger.Info("生成用户id："+id);
            return id;
            
        }

        private string NewRandom(int length)
        {
            Random random = new Random();
            int id = random.Next((int)Math.Pow(10, length-1), (int)Math.Pow(10,length));
            return id.ToString();
        }
    }
}
