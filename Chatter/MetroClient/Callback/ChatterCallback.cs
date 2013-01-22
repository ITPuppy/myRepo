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


        public void RequestToTargetClient(Message mesg)
        {
            MessageBox.Show("对方请求添加好友是否同意");
            DataUtil.Client.ResponseToAddFriendCompleted += Client_ResponseToAddFriendCompleted;
            DataUtil.Client.ResponseToAddFriendAsync(new Result() {  member=mesg.from as Member,userGroup=mesg.to as UserGroup, status=MessageStatus.Accept});
        }

        void Client_ResponseToAddFriendCompleted(object sender, ResponseToAddFriendCompletedEventArgs e)
        {
            MessageBox.Show(e.Result.mesg);
        }

        public IAsyncResult BeginRequestToTargetClient(Message mesg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndRequestToTargetClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void ReponseToSouceClient(Result result)
        {
            MessageBox.Show(result.mesg);
        }

        public IAsyncResult BeginReponseToSouceClient(Result result, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndReponseToSouceClient(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
