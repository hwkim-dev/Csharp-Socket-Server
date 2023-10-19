//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;

//namespace Game_Multi_Play_Server
//{
//    static class Player_Table
//    {
//        class Players
//        {
//            //첫번째 플레이어 IP
//            public EndPoint[] ip_add;
//            //두번째 플레이어 IP
//            //public EndPoint ip_B;

//            public bool isEmpty;
//            public Players()
//            {
//                ip_add = new EndPoint[2];
//                isEmpty = true;
//            }
//        }

//        private static byte table_Length = 255;
//        private static Players[] gm_Play = new Players[table_Length];
//        private static byte room_Cursor = 0;
//        public static void boot()
//        {
//            room_Cursor = 0;
//            for (int i = 0; i < table_Length; i++)
//            {
//                gm_Play[i] = new Players();
//            }
//        }

//        public static byte make_Room(EndPoint ip_A, EndPoint ip_B)
//        {
//            if (room_Cursor >= table_Length)
//            {
//                room_Cursor = 0;
//            }
//            while (gm_Play[room_Cursor].isEmpty == false)
//            {
//                ++room_Cursor;
//            }
//            gm_Play[room_Cursor].ip_add[0] = ip_A;
//            gm_Play[room_Cursor].ip_add[1] = ip_B;

//            gm_Play[room_Cursor].isEmpty = false;
//            ++room_Cursor;
//            return (byte)(room_Cursor - 1);
//        }

//        public static EndPoint send(byte cursor, byte player_Num)
//        {
//            player_Num += 1;
//            if (player_Num >= 2)
//            {
//                player_Num = 0;
//            }
//            return gm_Play[cursor].ip_add[player_Num];
//        }
//    }
//}
