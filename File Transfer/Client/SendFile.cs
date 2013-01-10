using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net.Sockets;

namespace Client
{
    class SendFile
    {
        int BufferSize = 1024*1024*2;
        
        public  void doSend(Socket socket)
        {

            String xmlPath=XMLTools.MakeXML();
            String filePath=XMLTools.filePath;
            FileInfo xmlFile = new FileInfo(xmlPath);
            NetworkStream nsw = null;
            BufferedStream bsw = null;
            NetworkStream nsr = null;
            BufferedStream bsr = null;
           
            try
            {
                nsw = new NetworkStream(socket);
                bsw = new BufferedStream(nsw);
                nsr = new NetworkStream(socket);
                bsr = new BufferedStream(nsr);
                
                //发送xml文件 
                Console.WriteLine("****************开始发送XML文件************");
                sendFile(xmlPath, bsw);
                Console.WriteLine("****************成功发送XML文件************");
                //发送文件个数
                
                SendInt(bsw,XMLTools.fileCount);
                //发送files
                Console.WriteLine("****************开始发送所有文件************");
                if (File.Exists(filePath))
                {
                    sendFile(filePath, bsw);
                }
                if (Directory.Exists(filePath))
                {
                    SendDirectory(filePath,bsw);
                }
                Console.WriteLine("****************成功发送所有文件************");



            }
            catch (Exception e) { }
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
            }
           
        }

        private void SendDirectory(string filePath,BufferedStream bsw)
        {
            DirectoryInfo directory = new DirectoryInfo(filePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                sendFile(filePath+@"\"+file.Name,bsw);
            }
            foreach (DirectoryInfo di in directory.GetDirectories())
            {
                SendDirectory(filePath + @"\" + di.Name, bsw);
            }
        }

        private  void sendFile(String filePath, BufferedStream bsw)
        {
            FileStream fs = null;
            try
            {
                Console.WriteLine("****************开始发送************");
                FileInfo file = new FileInfo(filePath);
                fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                SendLong(bsw, file.Length);
                byte[] array = new byte[BufferSize];
                int read = 0;
                int i = 0;
                while ((read = fs.Read(array, 0, array.Length)) > 0)
                {
                    bsw.Write(array, 0, read);
                    bsw.Flush(); i+=read;
                    Console.WriteLine(i+"  "+file.Length);
                }
                Console.WriteLine("****************成功发送************");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(fs!=null)
                {
                    fs.Close();
                    fs=null;
                }
            }
        }



        private void SendLong(BufferedStream bsw, long i)
        {
            byte[] array = null;

            array = BitConverter.GetBytes(i);
            bsw.Write(array, 0, array.Length);
            bsw.Flush();

        }
        private void SendInt(BufferedStream bsw, int i)
        {
            byte[] array = null;

            array = BitConverter.GetBytes(i);
            bsw.Write(array, 0, array.Length);
            bsw.Flush();

        }
       
    }
}
