using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Client
{
    class XMLTools
    {
        public static string filePath;
        static string xmlPath = @"c:\files.xml";
        public static int fileCount=0;
        static XmlDocument xmlDoc ;
        static public  String MakeXML()
        {
            Console.WriteLine("输入源文件夹");
            filePath=@Console.ReadLine();

            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>

            xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmldecl);

            //加入一个根元素
            XmlElement root = xmlDoc.CreateElement("", "FILES", "");
            xmlDoc.AppendChild(root);



            if (File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                XmlElement xel = xmlDoc.CreateElement("FILE");//创建一个<file>节点
                xel.SetAttribute("NAME", file.Name);//设置该节点genre属性
                xel.SetAttribute("SIZE", file.Length.ToString());//设置该节点genre属性
                xel.SetAttribute("CREATE_TIME", file.CreationTime.ToString());
                xel.SetAttribute("MODIFY_TIME", file.LastWriteTime.ToString());
                xel.SetAttribute("ATTRIBUTES", file.Attributes.ToString());
                root.AppendChild(xel);
                fileCount = 1;
            }

            else if (Directory.Exists(filePath))
            {
                int count=0;
                fileCount=addToXml(filePath, root,count);
            }
            xmlDoc.Save(xmlPath);
            
            return xmlPath;
        }
        static private int addToXml(string filePath, XmlElement root,int count)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);


            foreach (FileInfo file in di.GetFiles())
            {
                XmlElement xel = xmlDoc.CreateElement("FILE");//创建一个<file>节点
                xel.SetAttribute("NAME", file.Name);//设置该节点genre属性
                xel.SetAttribute("SIZE", file.Length.ToString());//设置该节点genre属性
                xel.SetAttribute("CREATE_TIME", file.CreationTime.ToString());
                xel.SetAttribute("MODIFY_TIME", file.LastWriteTime.ToString());
                xel.SetAttribute("ATTRIBUTES", file.Attributes.ToString());
                root.AppendChild(xel);
                count=count+1;
            }
            foreach (DirectoryInfo directory in di.GetDirectories())
            {
                XmlElement xel = xmlDoc.CreateElement("DIRECTORY");//创建一个<directory>节点
                xel.SetAttribute("NAME", directory.Name);//设置该节点ISBN属性
                xel.SetAttribute("CREATE_TIME", directory.CreationTime.ToString());
                xel.SetAttribute("MODIFY_TIME", directory.LastWriteTime.ToString());
                xel.SetAttribute("ATTRIBUTES", directory.Attributes.ToString());
                root.AppendChild(xel);
              count= addToXml(filePath + @"\" + directory.Name, xel,count);
            }
            return count;
        }

        
        
    }
}
