using Chatter.Contract.DataContract;
using Chatter.Contract.ServiceContract;
using Chatter.DAL;
using Chatter.Log;
using System;
using System.ServiceModel;

namespace Chatter.Service
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall)]
    public class RegisterService:IRegister
    {
        /// <summary>
        /// 注册号码
        /// </summary>
        /// <param name="member">注册用户信息</param>
        /// <returns>包含用户号码的用户信息</returns>
        public Member Register(Member member)
        {
            string id = Util.NewMemberId(7);
            member.Id = id;
            if (DALService.AddMember(member))
            {
                return member;
            }
            else
            {
               MyLogger.Logger.Warn("用户添加失败");
                return null;
            }
        }
       
    }
}
