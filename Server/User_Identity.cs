using System;
using System.Collections.Generic;
using System.Text;

namespace Login_Server
{
    public class User_Identity
    {
        private string json;
        private byte fn;
        private string ip_Addr;
        public byte type() { return Fn; }
        //public int Json_Leng_Nofn() { return (json.Length - 8); }

        public string Json { get => json; set => json = value; }
        public byte Fn { get => fn; set => fn = value; }

        //뒤의 5자리는 ip주소가 아님
        public string get_Ip_Addr()
        {
            string ip = "";
            foreach (char item in ip_Addr)
            {
                if (item == ':')
                {
                    break;
                }
                ip += item;
            }
            return ip;
        }
        public string set_Ip_Addr{ set => ip_Addr = value; }
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
