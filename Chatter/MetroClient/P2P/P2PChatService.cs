using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.P2P
{
    class P2PChatService:IP2PChatService
    {
        

        public void SendP2PMessage(Member member, string to, Message mesg)
        {
            DataUtil.MessageTabControl.SelectedItem=DataUtil.GroupMessageTabItems[to];
            DataUtil.GroupMessageTabItems[to].ReceiveMessage(mesg);
        }

        public void AddMember(Member member, string groupId)
        {

            var tabItem = DataUtil.GroupMemberTabItems[groupId];
            tabItem.myGrid.AddButton(MyType.UserInGroup, member);
            DataUtil.AddMember2Group(member, groupId);
        }

        public void DeleteMember(string memberId, string groupId)
        {
        }


        public void Join()
        {

        }
    }
}
