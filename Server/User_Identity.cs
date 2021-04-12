using System;
using System.Collections.Generic;
using System.Text;

namespace Login_Server
{
    public class User_Identity
    {
        private string json;
        private byte fn;
        public byte type() { return Fn; }
        //public int Json_Leng_Nofn() { return (json.Length - 8); }

        public string Json { get => json; set => json = value; }
        public byte Fn { get => fn; set => fn = value; }

        public byte getCursor()
        {
            return Byte.Parse(json.Substring(6, json.Length - 6));
        }
        public int getVerti()
        {
            return Int32.Parse(json.Substring(0, 6));
        }
    }
}
