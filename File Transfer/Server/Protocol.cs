using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace Server
{
    class Protocol:IProtocol
    {
        enum STATE { SUCESSFUL=1, FAILED=0 }; 
        public void service(Socket socket)
        {
             
            NetworkStream nsw =null;
            BufferedStream bsw = null;
            NetworkStream nsr =null;
            BufferedStream bsr = null;
            int id = 0;
            try
            {
                nsw = new NetworkStream(socket);
                bsw = new BufferedStream(nsw);
                nsr = new NetworkStream(socket);
                bsr = new BufferedStream(nsr);
                
               
                //用户id自增
                Server.n = Server.n + 1;
                 id = Server.n ;



                //添加用户
                Server.dic.Add(id, socket);


               
              


                //写ID
                 SendInt(bsw, id);
                Console.WriteLine("Client id已发送:{0}",id);



                SendUserList(bsw);


                while(true)
                {
                    int cmd = ReceiveInt(bsr);
                    if (cmd == 3)
                    {
                        Console.WriteLine(id+"退出");
                        break;
                    }
                    switch(cmd)
                    {
                        case 1:
                            {
                                Console.WriteLine("*****************用户请求在线列表***************");
                                SendUserList(bsw);
                                Console.WriteLine("*******************列表已发送*******************");
                                break;
                            }
                        case 2:
                            {
                               
                                Console.WriteLine("*****************用户请求发送文件***************");
                                int yourID = ReceiveInt(bsr);
                                Socket yourSocket = null;
                                Server.dic.TryGetValue(yourID, out yourSocket);
                                if (yourSocket == null)
                                {
                                    Console.WriteLine("*****************对方不在线，发送文件请求取消***************");
                                    SendState(bsw, STATE.FAILED);
                                }

                                else {
                                    SendState(bsw, STATE.SUCESSFUL);

                                    Console.WriteLine("*****************开始发送***************");
                                    Forward f = new Forward(socket,yourSocket);
                                    f.doForward();
                                }
                                break;
                            }
                        default: break;
                    }
                }





            }
            catch (Exception e)
            {

            }
            finally
            {
                if (nsw != null)
                {
                    nsw.Close();
                    nsw = null;

                }
                if (bsw != null)
                {
                    bsw.Close();
                    bsw = null;
                }
                if (nsr != null)
                {
                    nsr.Close();
                    nsr = null;

                }
                if (bsr != null)
                {
                    bsr.Close();
                    bsr = null;
                }
                if (socket != null)
                {
                      socket.Close();
                    socket = null;
                }
                Server.dic.Remove(id);
            }
        }

        private void SendState(BufferedStream bsw, STATE sTATE)
        {
            SendInt(bsw, sTATE == STATE.SUCESSFUL ? 1 : 0);
        }

        private void SendUserList(BufferedStream bsw)
        {
            int n = Server.dic.Count;
            //写在线人数
            SendInt(bsw, n);
            Console.WriteLine("在线人数已发送:{0}", n);



            //写在线id
            foreach (int key in Server.dic.Keys)
            {

                SendInt(bsw, key);
            }
            Console.WriteLine("在线用户id已发送");
        }

        private  void SendInt(BufferedStream bsw,  int i)
        {
            byte[] array = null;

            array = BitConverter.GetBytes(i);
            bsw.Write(array, 0, array.Length);
            bsw.Flush();
            
        }

        private int ReceiveInt(BufferedStream bsr)
        {
            byte[] array = new byte[4];

            int size = bsr.Read(array, 0, array.Length);
            int i = BitConverter.ToInt32(array, 0);
            return i;
        }
    }
}
