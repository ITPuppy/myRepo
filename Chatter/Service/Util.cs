using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Chatter.DAL;
using Chatter.Log;

namespace Chatter.Service
{
   public class Util
    {
        public static string NewMemberId(int length)
        {
            string id = NewRandom(length);
            while (DALService.IsExistMember(id))
            {
                id = NewRandom(length);
            }
           MyLogger.Logger.Info("生成用户id：" + id);
            return id;

        }

        public static string NewRandom(int length)
        {
            Random random = new Random();
            int id = random.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length));
            return id.ToString();
        }
    }
}
