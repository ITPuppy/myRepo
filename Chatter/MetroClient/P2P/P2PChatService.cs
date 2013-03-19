using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;
using Chatter.Log;

namespace Chatter.MetroClient.P2P
{
    class P2PChatService : IP2PChatService
    {


        public void SendP2PMessage(Member member, string to, Message mesg)
        {
            try
            {

                DataUtil.MessageTabControl.SelectedItem = DataUtil.GroupMessageTabItems[to];
                DataUtil.GroupMessageTabItems[to].ReceiveMessage(mesg);
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error(String.Format("从{0}接收群消息时候出错", member.nickName), ex);
            }
        }

        public void AddMember(Member member, string groupId)
        {
            try
            {
                var tabItem = DataUtil.GroupMemberTabItems[groupId];
                tabItem.myGrid.AddButton(MyType.UserInGroup, member);
               
            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("界面上添加群组成员时候出错", ex);
            }
        }

        public void DeleteMember(string memberId, string groupId)
        {
            try
            {


                var tabItem = DataUtil.GroupMemberTabItems[groupId];
                var button = tabItem.myGrid.GetButton(MyType.UserInGroup, memberId);
                tabItem.myGrid.RemoveButton(MyType.UserInGroup, button);

                if ((button.baseRole as Member).id == DataUtil.Member.id)
                {
                    DeleteGroup(groupId);
                   
                }

            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("界面上删除群组成员时候出错", ex);
            }
        }



        public void Join()
        {

        }



        public void Dispose()
        {

        }








        public void DeleteGroup(string groupId)
        {
            try
            {
                var groupTabItems = DataUtil.TabControl.groupTabItem as MyTabItem;
                var btn = groupTabItems.myGrid.GetButton(MyType.Group, groupId);
                groupTabItems.myGrid.RemoveButton(MyType.Group, btn);

                P2PClient.RemoveClient(groupId);

            }
            catch (Exception ex)
            {
                MyLogger.Logger.Error("界面上删除群组时候出错", ex);
            }
        }
    }
}
