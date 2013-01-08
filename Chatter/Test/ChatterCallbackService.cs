using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ChatterService;

namespace Test
{
    class ChatterCallbackService:IChatterCallback
    {
        public void OnLogin(string id)
        {
            Console.WriteLine(id+"已经登录");
        }

        public IAsyncResult BeginOnLogin(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogin(IAsyncResult result)
        {
            
        }

        public void SendMessageCallback(Result result)
        {
            
        }

        public IAsyncResult BeginSendMessageCallback(Result result, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndSendMessageCallback(IAsyncResult result)
        {
           
        }

        public void OnLogoff(string id)
        {
            Console.WriteLine(id + "已经下线");
        }

        public IAsyncResult BeginOnLogoff(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogoff(IAsyncResult result)
        {
            
        }
    }
}
