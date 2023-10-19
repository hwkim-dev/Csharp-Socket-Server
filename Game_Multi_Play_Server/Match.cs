//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using static Game_Multi_Play_Server.Program;

//namespace Game_Multi_Play_Server
//{
//    static class Match
//    {
//        //ArrayList<EndPoint> mt = new System.Collections.ArrayList<EndPoint>
//        //static Dictionary<int, EndPoint> match = new Dictionary<int, EndPoint>();

//        static EndPoint[] id;
//        static EndPoint[] player_2;

//        static Socket[] sk;
//        static Socket[] player_2_socket;



//        public static void boot()
//        {
//            id = new EndPoint[255];
//            player_2 = new EndPoint[255];
//            sk = new Socket[255];
//            player_2_socket = new Socket[255];
//        }

//        private static byte cursor = 0;

//        private static Object obj = new Object();
//        public static byte make_Room(Socket skt, EndPoint ep)
//        {
//            lock (obj)
//            {
//                id[cursor] = ep;
//                sk[cursor] = skt;


//                ++cursor;
//                return (byte)(cursor - 1);
//            }
//        }
//        public static void test()
//        {
//            lock (obj)
//            {
//                byte[] x = new byte[2];
//                x[0] = 5;
//                sk[cursor - 1].SendTo(x,id[cursor - 1]);

//            }
//        }
//        public static bool find_Room(byte key, Socket skt, EndPoint ep, byte return_Val, User_Identity ui)
//        {
//            if(id[key] != null)
//            {
//                player_2[key] = ep;
//                player_2_socket[key] = skt;

//                ui.ep = id[key];
//                ui.socket = sk[key];

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static bool send_To(byte key, EndPoint ep, User_Identity ui)
//        {
//            string ep_ip = "";
//            foreach (char item in ep.ToString())
//            {
//                if (item == ':')
//                {
//                    break;
//                }
//                ep_ip += item;
//            }
//            string ip = "";
//            foreach (char item in ip[key].ToString())
//            {
//                if (item == ':')
//                {
//                    break;
//                }
//                ip += item;
//            }

//            if (ip.Equals(ep_ip))
//            {
//                ui.ep = id[key];
//                ui.socket = sk[key];
//            }
//            else
//            {
//                Console.WriteLine(ep.ToString());
//                ui.ep = player_2[key];
//                ui.socket = player_2_socket[key];
//            }

//            return true;
//        }
//        /*

//        public static match_Return new_In(EndPoint ep)
//        {
//            if (match.Count < 1)
//            {
//                match.AddLast(ep);

//                return null;
//            }
//            else
//            {
//                match_Return mr = new match_Return();
//                mr.ip = match.First.Value;
//                mr.roomNum = Player_Table.make_Room(ep, match.First.Value);
//                match.RemoveFirst();

//                return mr;
//            }
//        }*/
//    }
//}
