using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Multi_Play_Server
{
    public static class Play_Table
    {
        public class Buffer
        {
            public byte[] bf_Id_2 = new byte[13];


            public byte[] Id_2_Skill = new byte[13];
            //public bool Id_2_change = false;

            //public bool id2_check = false;
            public byte[] bf_Id_3 = new byte[13];

            public byte[] Id_3_Skill = new byte[13];
            //public bool Id_3_change = false;
            //public bool id3_check = false;
            public Buffer()
            {
                for (int i = 0; i < Id_2_Skill.Length; i++)
                {
                    Id_2_Skill[i] = 0;
                    Id_3_Skill[i] = 0;
                }
                for (int i = 0; i < bf_Id_2.Length; i++)
                {
                    bf_Id_2[i] = 0;
                    bf_Id_3[i] = 0;
                }
            }
        }
        private static readonly byte no_Room = 0;
        private static readonly byte room_On_Que = 1;
        private static readonly byte match_Succed = 2;


        public static Buffer[] buffer;

        public static byte[] room;

        public static void boot()
        {
            buffer = new Buffer[255];
            room = new byte[255];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = new Buffer();
            }
        }

        public static bool find_Room(byte key)
        {
            if (room[key] == room_On_Que)
            {
                room[key] = match_Succed;
                return true;
            }
            else
            {
                return false;
            }
        }

        //오버플로우 경우를 생각하자
        private static byte cursor = 0;
        public static byte new_Room()
        {
            room[cursor] = room_On_Que;
            ++cursor;
            return (byte)(cursor - 1);
        }

        public static bool update_My_Room(byte key)
        {
            if (room[key] == match_Succed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
