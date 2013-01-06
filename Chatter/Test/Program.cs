using Chatter.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.RegisterService;

namespace Chatter
{
    class Program
    {
        static void Main(string[] args)
        {

            Test.RegisterService.RegisterClient client = new Test.RegisterService.RegisterClient();
            Member member = new Member();
            member.nickName = "帅的拖网速";
            member.password = "12345";
            member.sex = "男";
         
            member.birthday = DateTime.Now;

            client.RegisterCompleted += client_RegisterCompleted;
            client.RegisterAsync(member);

            Console.ReadLine();
            client.Close();
        }

        static void client_RegisterCompleted(object sender, RegisterCompletedEventArgs e)
        {
               Console.WriteLine(e.Result.id);
        }
    }
}
