using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.Callback
{
    class ChatterCallback:IChatterCallback
    {
        public void OnLogin(string id)
        {
            MessageBox.Show(id+"已经登录");
        }

        public void OnSendMessageCallback(Result result)
        {
            
        }

        public void OnLogoff(string id)
        {
            MessageBox.Show(id + "已经退出");
        }


        public IAsyncResult BeginOnLogin(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogin(IAsyncResult result)
        {
           
        }

       

       

        public IAsyncResult BeginOnLogoff(string id, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndOnLogoff(IAsyncResult result)
        {
            
        }


        public void OnSendMessage(Result result)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginOnSendMessage(Result result, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndOnSendMessage(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
