using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class Client
    {
        public static object o = new object();
        enum STATE { SUCESSFUL = 1, FAILED = 0 }; 
        public void Start()
        {
             Socket socket=null;
            NetworkStream nsw = null;
            BufferedStream bsw = null;
            NetworkStream nsr = null;
            BufferedStream bsr = null;
            try
            { 
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect("10.2.5.177", 9877);
                
                nsw = new NetworkStream(socket);
                bsw = new BufferedStream(nsw);
                nsr = new NetworkStream(socket);
                bsr = new BufferedStream(nsr);
                int id = 0;
               



                //读ID
                id = ReceiveInt(bsr);
                Console.WriteLine("用户id已接收:{0}", id);



                 ReceiveUserList(bsr);
               
               
                while (true)
                {
                    
                    
                    Console.WriteLine("1:查看在线用户\n2:发送文件\n3:接收文件\n4:退出\n");
                    String sCmd = Console.ReadLine();
                    
                 
                        
                        int cmd = Convert.ToInt32(sCmd);
                        if (cmd == 4)
                        {
                            SendInt(bsw, cmd);
                            break;
                        }
                        switch (cmd)
                        {
                            case 1:
                                {
                                    SendInt(bsw, cmd);
                                    ReceiveUserList(bsr);
                                    break;
                                }
                            case 2:
                                {
                                    SendInt(bsw, cmd);
                                    Console.WriteLine("输入文件接收者ID");
                                    String sHisID = Console.ReadLine();
                                    int hisID = Convert.ToInt32(sHisID);
                                    SendInt(bsw, hisID);
                                    STATE state = ReceiveState(bsr);
                                    if (state == STATE.FAILED)
                                    {
                                        Console.WriteLine("用户不在线");
                                    }
                                    else
                                    {
                                        SendFile sendFile = new SendFile();
                                        sendFile.doSend(socket);
                                    }
                                    break;
                                }
                            case 3:
                                {
                                    Console.WriteLine("服务器发送文件请求");
                                    ReceiveFile receiveFile = new ReceiveFile();
                                    receiveFile.doReceive(socket);
                                    break;
                                }
                            default: Console.WriteLine("没有此命令"); break;
                        
                    }
                }
                




             


            }
            catch (Exception e)
            {

            }
            finally
            {
                if (bsw != null)
                {
                    bsw.Close();
                    bsw = null;
                }
                if (nsw != null)
                {
                    nsw.Close();
                    nsw = null;

                }


                if (bsr != null)
                {
                    bsr.Close();
                    bsr = null;
                }
                if (nsr != null)
                {
                    nsr.Close();
                    nsr = null;

                }
                if (socket != null)
                {
                     socket.Close();
                    socket = null;
                }
                
            }

        }

        private STATE ReceiveState(BufferedStream bsr)
        {
            return ReceiveInt(bsr)==1?STATE.SUCESSFUL:STATE.FAILED;
        }

        private void ReceiveUserList(BufferedStream bsr)
        {
            int n = 0;
            //读在线人数
            n = ReceiveInt(bsr);
            Console.WriteLine("在线人数已接收:{0}", n);



            for (int i = 0; i < n; i++)
            {

                int key = ReceiveInt(bsr);
                Console.WriteLine(key);

            }
           
        }

        private void SendInt(BufferedStream bsw, int i)
        {
            byte[] array = null;

            array = BitConverter.GetBytes(i);
            bsw.Write(array, 0, array.Length);
            bsw.Flush();

        }
        private  int ReceiveInt(BufferedStream bsr)
        {
            byte[] array = new byte[4];
            
            int size = bsr.Read(array, 0, array.Length);
            int i = BitConverter.ToInt32(array, 0);
            return i;
        }
        }
    }
