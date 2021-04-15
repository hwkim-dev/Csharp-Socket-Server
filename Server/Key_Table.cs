using System;
using System.Collections.Generic;
using System.Text;


namespace Login_Server
{
    /*
    static class Key_Table
    {
        
        private static readonly byte KEYLENGTH = 10;
        private static readonly byte LOCATION = 3;
        private static readonly byte EXPIRETIME = 5;
        private static Dictionary<string, byte> user;
        private static Random rand;
        private static char[,,] keys;
        private static StringBuilder[] Latest_Key;
        public static void boot()
        {
            Latest_Key = new StringBuilder[LOCATION];
            Latest_Key[0] = new StringBuilder();
            Latest_Key[1] = new StringBuilder();
            Latest_Key[2] = new StringBuilder();
            user = new Dictionary<string, byte>();
            rand = new Random();
            keys = new char[EXPIRETIME, LOCATION, KEYLENGTH];
        }
        public static void add_Vaild_User(string id, byte location)
        {
            user.Add(id, location);
        }
        //키의 가장 첫번째 값 = 키 배열의 위치
        public static bool is_Vaild_Key(string _id, string input_Key)
        {
            try
            {
                byte location;
                byte num = 0;
                if (user.ContainsKey(_id))
                {
                    return false;
                }
                else
                {
                    location = user.GetValueOrDefault(_id);
                }
                foreach (char item in input_Key)
                {
                    if(item != keys[(int)input_Key[0], location, ++num])
                    {
                        return false;
                    }
                }
                user.Remove(_id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {

            }
        }

        //1분이 지날때마다 호출되는 함수

        private static byte cursor = 0;
        public static void key_Expires()
        {
            try
            {
                for (byte lc = 0; lc < LOCATION; lc++)
                {
                    for (byte i = 0; i < KEYLENGTH; i++)
                    {
                        keys[cursor, lc, i] = (char)rand.Next(0, 255);
                        Latest_Key[lc].Append(keys[cursor, lc, i]);
                    }
                }
            }
            catch(Exception)
            {

            }
            finally
            {
                cursor = (byte)((++cursor) % EXPIRETIME);
            }
        }

        public static string get_Latest_Key(byte location)
        {
            return Latest_Key[location].ToString();
        }
    }*/
}
