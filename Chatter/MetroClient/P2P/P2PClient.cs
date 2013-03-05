using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.P2P
{
    public class P2PClient
    {

        private static Dictionary<string,P2PClient> clients=new Dictionary<string,P2PClient>();
        private string groupId;
       private IP2PChatService channel;
        private P2PClient(string  groupId )
        {
            this.groupId = groupId;
            StartP2PClient();
        }

        public static P2PClient GetP2PClient(string groupId)
        {
           if(clients.ContainsKey(groupId))
           {
               return clients[groupId];
           }
           else 
           {
               return new P2PClient(groupId);
             
           }
        }

        private  void StartP2PClient( )
        {
            InstanceContext context = new InstanceContext(new P2PChatService());
            DuplexChannelFactory<IP2PChatService> factory = new DuplexChannelFactory<IP2PChatService>(context,"p2p", new EndpointAddress("net.p2p://" + groupId));

            channel = factory.CreateChannel();
            clients.Add(groupId,this);
        }

        public void SendP2PMessage(Message mesg)
        {
            channel.SendP2PMessage(DataUtil.Member,groupId,mesg);
        }

        public void AddGroupMember(string memberId)
        {
            channel.AddMember(memberId, groupId);
        }

        public void DeleteGroupMember(string memberId)
        {
            channel.DeleteMember(memberId, this.groupId);
        }
    }
}
