using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using MetroClient.ChatterService;
using Chatter.Log;
using System.Configuration;
using Chatter.MetroClient.UI;

namespace Chatter.MetroClient.P2P
{
    public class P2PClient:IDisposable
    {

        private static Dictionary<string,P2PClient> clients=new Dictionary<string,P2PClient>();
        private string groupId;
       private IP2PChatService channel;
        private P2PClient(string  groupId )
        {
            this.groupId = groupId;
            StartP2PClient();
        }

        public static void RemoveClient(string groupId)
        {
            if (clients.ContainsKey(groupId))
            {
                clients[groupId].Dispose();
                clients.Remove(groupId);
            }
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
            string s = ConfigurationManager.AppSettings["IsSupportGroup"];
            if (s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            InstanceContext context = new InstanceContext(new P2PChatService());
            DuplexChannelFactory<IP2PChatService> factory = new DuplexChannelFactory<IP2PChatService>(context,"p2p", new EndpointAddress("net.p2p://" + groupId));


            channel = factory.CreateChannel();
            clients.Add(groupId, this);
           

           
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    channel.Join();
                }
                catch (Exception ex)
                {
                    MyLogger.Logger.Error("进入群组聊天室时出错",ex);
                }
            })).Start();
           
        }


        private void StopP2PClient()
        {
            channel.Dispose();
        }

        public void SendP2PMessage(Message mesg)

        {
            string s = ConfigurationManager.AppSettings["IsSupportGroup"];
            if (s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            channel.SendP2PMessage(DataUtil.Member,groupId,mesg);
        }

        public void AddGroupMember(Member member)
        {
            string s = ConfigurationManager.AppSettings["IsSupportGroup"];
            if (s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            channel.AddMember(member, groupId);
        }

        public void DeleteGroupMember(string memberId)
        {
            string s = ConfigurationManager.AppSettings["IsSupportGroup"];
            if (s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            channel.DeleteMember(memberId, this.groupId);
        }

        internal void DeleteGroup(string groupId)
        {
            string s = ConfigurationManager.AppSettings["IsSupportGroup"];
            if (s.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            channel.DeleteGroup(groupId);
        }

        public void Dispose()
        {
            channel.Dispose();   
        }
    }
}
