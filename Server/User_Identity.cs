using System;
using System.Collections.Generic;
using System.Text;

namespace Login_Server
{
    static class User_Identity
    {
        private static string json;
        public static string Json { get => json; set => json = value; }
        public static byte type() { return (byte)Json[7]; }
    }
}
