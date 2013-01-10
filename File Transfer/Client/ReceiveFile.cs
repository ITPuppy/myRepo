using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Xml;

namespace Client
{
    class ReceiveFile
    {
        static int BufferSize = 1024*1024*2;
        static String xmlPath = @"d:\files.xml";
        public void doReceive(Socket socket)
        {
            Console.WriteLine("输入存储路径");
           string filesPath= @Console.ReadLine();
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

                Console.WriteLine("************开始接收XML文件**************");
                receiveFile(xmlPath, bsr);
                Console.WriteLine("************成功接收XML文件**************");

                readXML(filesPath, bsr);

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



        void readXML(String filesPath, BufferedStream bsr)
        {
            XmlDocument xmld = new XmlDocument();
            xmld.Load(xmlPath);
            XmlNode root = xmld.DocumentElement;
            ReadAllFiles(filesPath, bsr, root);
        }

        private static void ReadAllFiles(String filesPath, BufferedStream bsr, XmlNode root)
        {
            XmlNodeList xnlFile = root.ChildNodes;
            foreach (XmlElement xe in xnlFile)
            {
                if (xe.Name.Equals("FILE"))
                {
                    String name = xe.GetAttribute("NAME");
                    int size = Convert.ToInt32(xe.GetAttribute("SIZE"));
                    DateTime create_time = Convert.ToDateTime(xe.GetAttribute("CREATE_TIME"));
                    DateTime modify_time = Convert.ToDateTime(xe.GetAttribute("MODIFY_TIME"));
                    string attributes = xe.GetAttribute("ATTRIBUTES");

                    receiveFile(filesPath + @"\" + name, bsr);
                }
                else if (xe.Name.Equals("DIRECTORY"))
                {

                    String name = xe.GetAttribute("NAME");

                    DateTime create_time = Convert.ToDateTime(xe.GetAttribute("CREATE_TIME"));
                    DateTime modify_time = Convert.ToDateTime(xe.GetAttribute("MODIFY_TIME"));
                    string attributes = xe.GetAttribute("ATTRIBUTES");

                    DirectoryInfo di = Directory.CreateDirectory(filesPath + @"\" + name);
                    di.CreationTime = create_time;
                    di.LastWriteTime = modify_time;
                    //文件属性？？？？


                    ReadAllFiles(filesPath + @"\" + di.Name, bsr, xe);


                }

            }

        }

        public static void receiveFile(String filePath, BufferedStream bsr)
        {
            FileStream fs = null;
            try
            {
                long size = ReceiveLong(bsr);
                fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                Console.WriteLine("文件大小为{0}", size);
                int read, temp = 0;
                byte[] array = new byte[BufferSize];

                for (read = temp; read < size; read += temp)
                {
                    temp = bsr.Read(array, 0, (int)((size - read) - array.Length > 0 ? array.Length : (size - read)));
                    fs.Write(array, 0, temp);
                    fs.Flush();

                }
            }
            catch (Exception e)
            { }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
        }


        static private long ReceiveLong(BufferedStream bsr)
        {
            byte[] array = new byte[8];

            int size = bsr.Read(array, 0, array.Length);
            long i = BitConverter.ToInt64(array, 0);
            return i;
        }
    }
}
