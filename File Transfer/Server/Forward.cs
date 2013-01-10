using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Forward
    {
        int BufferSize = 1024*1024*10;
        enum STATE { SUCESSFUL = 1, FAILED = 0 }; 
        private System.Net.Sockets.Socket fromSocket;
        private System.Net.Sockets.Socket toSocket;

        public Forward(Socket fromSocket, Socket toSocket)
        {
           
            this.fromSocket = fromSocket;
            this.toSocket = toSocket;
        }

       public  void doForward()
        {
            NetworkStream fromNSW = null;
            BufferedStream fromBSW = null;
            NetworkStream fromNSR = null;
            BufferedStream fromBSR = null;
            NetworkStream toNSW = null;
            BufferedStream toBSW = null;
            NetworkStream toNSR = null;
            BufferedStream toBSR = null;

            try
            {
                 fromNSW = new NetworkStream(fromSocket);
                 fromBSW = new BufferedStream(fromNSW);
                 fromNSR = new NetworkStream(fromSocket);
                 fromBSR = new BufferedStream(fromNSR);
                 toNSW = new NetworkStream(toSocket);
                 toBSW = new BufferedStream(toNSW);
                 toNSR = new NetworkStream(toSocket);
                 toBSR = new BufferedStream(toNSR);
                
                 

                //转发XML文件
                 ForwardFile(fromBSR, toBSW);

                 ForwardAllFiles(fromBSR,toBSR);

            }
            catch (Exception e)
            {


            }
            finally 
            {

                if (fromBSR != null)
                {
                    fromBSR.Close();
                    fromBSR = null;
                }
                 if (fromBSW != null)
                {
                    fromBSW.Close();
                    fromBSW = null;
                }
                 if (fromNSR != null)
                {
                    fromNSR.Close();
                    fromNSR = null;
                }
                 if (fromNSW != null)
                 {
                     fromNSW.Close();
                     fromNSW = null;
                 }


                      if (toBSR != null)
                {
                    toBSR.Close();
                    toBSR = null;
                }
                      if (toBSW != null)
                {
                    toBSW.Close();
                    toBSW = null;
                }
                      if (toNSR != null)
                {
                    toNSR.Close();
                    toNSR = null;
                }
                      if (toNSW != null)
                {
                    toNSW.Close();
                    toNSW = null;
                }

            }

        }

       private void ForwardAllFiles(BufferedStream fromBSR, BufferedStream toBSR)
       {
           int count=ReceiveInt(fromBSR);
           while (count--!=0)
           {
               ForwardFile(fromBSR, toBSR);
           }
       }

       

         private  void ForwardFile(BufferedStream fromBSR, BufferedStream toBSW)
         {
             byte[] array = new byte[BufferSize];
             int read=0 ,temp=0;
             
             long size = ReceiveLong(fromBSR);
             SendLong(toBSW, size);


             Console.WriteLine("********开始转发文件********");
             Console.WriteLine("文件大小为{0}", size);

            
             
             for (read=temp;read<size;read+=temp )
             {
                 temp = fromBSR.Read(array, 0, (int)((size - read) - array.Length > 0 ? array.Length : (size - read)));
                 toBSW.Write(array, 0, temp);
                  toBSW.Flush();
                
             }
             Console.WriteLine("********成功转发文件********");

         }


         private long ReceiveLong(BufferedStream bsr)
         {
             byte[] array = new byte[8];

             int size = bsr.Read(array, 0, array.Length);
             long i = BitConverter.ToInt64(array, 0);
             return i;
         }

         private void SendInt(BufferedStream bsw, int i)
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
         private void SendLong(BufferedStream bsw, long i)
         {
             byte[] array = null;

             array = BitConverter.GetBytes(i);
             bsw.Write(array, 0, array.Length);
             bsw.Flush();

         }
        
    }
}
