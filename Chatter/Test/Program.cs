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
namespace Chatter
{
    class Program
    {
        static void Main(string[] args)
        {

            InstanceContext context = new InstanceContext(typeof(IChatterCallback));
            IChatter client = DuplexChannelFactory<IChatter>.CreateChannel(context, "NetTcpBinding_IChatter");
          
            Test.ChatterService.Member member =new Test.ChatterService.Member();
            member.id="23234234324";
            member.password="asdfasdf";
            
           //client.LoginAsync(
            
          // Console.ReadLine();
          //  client.Close();
        }

        
    }
}
