using Chatter.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.RegisterService;
using System.Security.Cryptography;
using Test.ChatterService;
using System.ServiceModel;
using Test.RegisterService;
using Test;

namespace Chatter
{
    class Program
    {
        static ChatterClient chatterClient = null;
        static RegisterClient registerClient = null;
        static void Main(string[] args)
        {


           // Register();

            Login();


            Console.ReadLine();
            if (registerClient != null)
                registerClient.Close();
            if (chatterClient != null)
                chatterClient.Close();


        }

        private static void Login()
        {
            InstanceContext context = new InstanceContext(new ChatterCallbackService());

            chatterClient = new ChatterClient(context);

            chatterClient.LoginCompleted += client_LoginCompleted;
            Test.ChatterService.Member member = new Test.ChatterService.Member();
            member.id = "870067227";
            member.password = "vbnvbn";

            chatterClient.LoginAsync(member);
           //chatterClient.AddFriendCompleted += chatterClient_AddFriendCompleted;
            //chatterClient.AddFriendAsync("776041669", "870067227");



        }

        static void chatterClient_AddFriendCompleted(object sender, AddFriendCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
        }

        private static void Register()
        {
            registerClient = new RegisterClient();
            Test.RegisterService.Member member = new Test.RegisterService.Member();
            member.nickName = "walle";
            member.password = "vbnvbn";
            member.birthday = DateTime.Now;

            registerClient.RegisterCompleted += client_RegisterCompleted;
            registerClient.RegisterAsync(member);
        }

        static void client_RegisterCompleted(object sender, Test.RegisterService.RegisterCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
        }

        static void client_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            Console.WriteLine(e.Result);
        }


    }
}
