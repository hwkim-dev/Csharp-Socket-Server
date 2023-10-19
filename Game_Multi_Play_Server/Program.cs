using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game_Multi_Play_Server
{
    class Program
    {
        public class AsyncStateData
        {
            public byte[] Buffer;
            public Socket Socket;
            public EndPoint client_EP;
            //public IPAddress Ip;
            public EndPoint Me;
        }
        enum function : byte
        {
            JOIN_ROOM_CMD = 0,
            MAKE_ROOM_CMD = 1,
            ONMATCH = 2,
            FIND_ROOM = 3,
            ROOM_NOT_FOUND = 4,
            FOUND_USER_IN_MYROOM = 5,
            ROOM_PING = 6,
            USE_SKILL = 7,
            MAKE_TOWER = 8,
            ENEMY_PING = 9,
            NOTHING_TO_UPDATE = 10,
            NEED_UPDATE = 11,
            ENEMY_Skill_PING = 12,
        }
        enum tower_Make_Function : byte
        {
            ROOM_NUMBER = 1,
            TOWER_NAME = 2,
            TOWER_PLACE = 3,
            PLAYER_ID = 4,
        }
        enum enemy_Use_Skill : byte
        {
            ROOM_NUMBER = 1,
            PLAYER_ID = 2,
        }
        enum enemy_Ping : byte
        {
            FN = 0,
            ROOM_NUMBER = 1,
            PLAYER_ID = 2,
        }
        static void Main(string[] args)
        {
            try
            {
                Play_Table.boot();
                //Player_Table.boot();
                //Form.boot();

                //using ()
                {

                    Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11201);

                    srvSocket.Bind(endPoint);

                    EndPoint endPoint_Client = new IPEndPoint(IPAddress.None, 0);

                    Console.WriteLine("\nServer is running");
                    Console.WriteLine("Time : " + DateTime.Now);


                    while (true)
                    {
                        AsyncStateData data = new AsyncStateData();
                        data.Buffer = new byte[16];
                        data.Socket = srvSocket;
                        data.client_EP = new IPEndPoint(IPAddress.None, 0);

                        data.Socket.ReceiveFrom(data.Buffer, ref data.client_EP);

                        ThreadPool.QueueUserWorkItem(accept, data);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //public class match_Return
        //{
        //    public EndPoint ip;
        //    public byte roomNum;
        //}

        private static void accept(Object data)
        {
            try
            {
                //받은 데이터
                AsyncStateData rcvData = data as AsyncStateData;


                //아이피 얻어오기
                string client_EP = rcvData.client_EP.ToString();
                string ip = "";
                foreach (char item in client_EP)
                {
                    if (item == ':')
                    {
                        break;
                    }
                    ip += item;
                }
                rcvData.Me = new IPEndPoint(IPAddress.Parse(ip), 11201);

                //Console.WriteLine(rcvData.Buffer[0]);
                switch (rcvData.Buffer[0])
                {
                    case (byte)function.ENEMY_Skill_PING:
                        if (rcvData.Buffer[(byte)enemy_Ping.PLAYER_ID] == 2)
                        {
                            byte[] skill_Ping_Return = Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_3_Skill;
                            rcvData.Socket.SendTo(skill_Ping_Return, rcvData.client_EP);
                            //if(Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_3_change == true)
                            //{

                            //    //Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_3_change = false;
                            //}
                            //else
                            //{
                            //    byte[] skill_Ping_Return = new byte[1];
                            //    skill_Ping_Return[0] = (byte)function.NOTHING_TO_UPDATE;
                            //    rcvData.Socket.SendTo(skill_Ping_Return, rcvData.client_EP);
                            //}

                        }
                        else if (rcvData.Buffer[(byte)enemy_Ping.PLAYER_ID] == 3)
                        {
                            byte[] skill_Ping_Return = Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_2_Skill;
                            rcvData.Socket.SendTo(skill_Ping_Return, rcvData.client_EP);
                            //if(Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_2_change == true)
                            //{

                            //    Play_Table.buffer[rcvData.Buffer[(byte)enemy_Ping.ROOM_NUMBER]].Id_2_change = false;
                            //}
                            //else
                            //{
                            //    byte[] skill_Ping_Return = new byte[1];
                            //    skill_Ping_Return[0] = (byte)function.NOTHING_TO_UPDATE;
                            //    rcvData.Socket.SendTo(skill_Ping_Return, rcvData.client_EP);
                            //}
                        }
                        break;
                    case (byte)function.ENEMY_PING:
                        if (rcvData.Buffer[2] == 2)
                        {
                            //2면3껄 가져옴
                            //3이면 2껄 가져옴

                            byte[] ping_Return = Play_Table.buffer[rcvData.Buffer[1]].bf_Id_3;

                            rcvData.Socket.SendTo(ping_Return, rcvData.client_EP);

                            //if(Play_Table.buffer[rcvData.Buffer[1]].id3_check == false)
                            //{
                            //    Play_Table.buffer[rcvData.Buffer[1]].id3_check = true;

                            //}
                        }
                        else if (rcvData.Buffer[2] == 3)
                        {
                            byte[] ping_Return = Play_Table.buffer[rcvData.Buffer[1]].bf_Id_2;
                            rcvData.Socket.SendTo(ping_Return, rcvData.client_EP);
                            //if (Play_Table.buffer[rcvData.Buffer[1]].id2_check == false)
                            //{
                            //    Play_Table.buffer[rcvData.Buffer[1]].id2_check = true;

                            //}
                        }
                        break;
                    case (byte)function.USE_SKILL:
                        if (rcvData.Buffer[(byte)enemy_Use_Skill.PLAYER_ID] == 2)
                        {
                            Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_Skill = rcvData.Buffer;
                            //Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_change = true;
                            for (int i = 0; i < Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_Skill.Length; i++)
                            {
                                Console.Write(i + " = " + Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_Skill[i] + " /");
                            }
                            Console.WriteLine();
                        }
                        else if (rcvData.Buffer[(byte)enemy_Use_Skill.PLAYER_ID] == 3)
                        {
                            Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_3_Skill = rcvData.Buffer;
                            //Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_3_change = true;
                            for (int i = 0; i < Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_Skill.Length; i++)
                            {
                                Console.Write(i + " = " + Play_Table.buffer[rcvData.Buffer[(byte)enemy_Use_Skill.ROOM_NUMBER]].Id_2_Skill[i] + " /");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case (byte)function.MAKE_TOWER:
                        if (rcvData.Buffer[(byte)tower_Make_Function.PLAYER_ID] == 2)
                        {
                            // Play_Table.buffer[rcvData.Buffer[1]].id3_check = false;

                            // Play_Table.buffer[rcvData.Buffer[1]].bf_Id_2[0] = (byte)function.NEED_UPDATE;

                            //타워 장소에 타워 종류 
                            Play_Table.buffer[rcvData.Buffer[(byte)tower_Make_Function.ROOM_NUMBER]].bf_Id_2[rcvData.Buffer[(byte)tower_Make_Function.TOWER_PLACE]] = rcvData.Buffer[(byte)tower_Make_Function.TOWER_NAME];
                        }
                        else if (rcvData.Buffer[(byte)tower_Make_Function.PLAYER_ID] == 3)
                        {
                            // Play_Table.buffer[rcvData.Buffer[1]].id2_check = false;
                            //Play_Table.buffer[rcvData.Buffer[1]].bf_Id_2[0] = (byte)function.NEED_UPDATE;
                            Play_Table.buffer[rcvData.Buffer[(byte)tower_Make_Function.ROOM_NUMBER]].bf_Id_3[rcvData.Buffer[(byte)tower_Make_Function.TOWER_PLACE]] = rcvData.Buffer[(byte)tower_Make_Function.TOWER_NAME];
                        }
                        break;
                    case (byte)function.ROOM_PING:
                        if (Play_Table.update_My_Room(rcvData.Buffer[1]) == true)
                        {
                            byte[] fond_Enemy = { (byte)function.FOUND_USER_IN_MYROOM, 0 };


                            rcvData.Socket.SendTo(fond_Enemy, rcvData.client_EP);
                        }
                        else
                        {
                            byte[] not_Found = { (byte)function.ROOM_NOT_FOUND, 0 };


                            rcvData.Socket.SendTo(not_Found, rcvData.client_EP);
                        }
                        break;
                    case (byte)function.JOIN_ROOM_CMD:
                        if (Play_Table.find_Room(rcvData.Buffer[1]) == true)
                        {
                            //3는 플레이어아이디
                            byte[] room_Found = { (byte)function.FOUND_USER_IN_MYROOM, rcvData.Buffer[1], 3 };
                            rcvData.Socket.SendTo(room_Found, rcvData.client_EP);
                        }
                        else
                        {
                            byte[] room_NotFound = { (byte)function.ROOM_NOT_FOUND, rcvData.Buffer[1] };
                            rcvData.Socket.SendTo(room_NotFound, rcvData.client_EP);
                        }
                        break;
                    case (byte)function.MAKE_ROOM_CMD:
                        //3는 플레이어아이디
                        byte[] response = { 0, Play_Table.new_Room(), 2 };
                        rcvData.Socket.SendTo(response, rcvData.client_EP);
                        break;
                    default:
                        Console.WriteLine(rcvData.Buffer[0]);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!Connection Error Occur!!!\n" +
                    "<Connection Error>\n" +
                    e.Message + "\n" +
                    "<Connection Error>");
            }
            finally
            {

            }
        }


    }
}
