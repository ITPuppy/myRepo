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

        public void SendMessageCallback(Result result)
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

        public IAsyncResult BeginSendMessageCallback(Result result, AsyncCallback callback, object asyncState)
        {
            return null;
        }

        public void EndSendMessageCallback(IAsyncResult result)
        {
            throw new NotImplementedException();
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
