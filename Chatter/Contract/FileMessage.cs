using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chatter.Contract
{
    [DataContract]
    public class FileMessage:Message
    {
        [DataMember]
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        [DataMember]
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        
    }
}
