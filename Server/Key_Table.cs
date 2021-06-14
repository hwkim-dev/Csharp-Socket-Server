

using System;
using System.Collections.Generic;
using System.Text;


namespace Login_Server
{
    static class Key_Table
    {
        class id_Table
        {
            //ip주소
            public char[] ip;
            //6자리 이메일 인증 숫자
            public int vertinum;
            //이 테이블이 생성된 시간을 저장
            //이메일 인증할때 3분의 제한시간을 둠
            //또한 이메일 인증이 성공하고
            //인증이 되었다라는걸 유지하는 10분의 추가 시간을 준다(2분의 추가시간도 
            public byte verti_Time;
            //
            public bool vertified;

            public id_Table()
            {
                ip = new char[15];
                verti_Time = 0;
                vertified = false;
                vertinum = 0;
            }
            
            //5분에 1번실행됨 -> 테이블의 모든 정보 삭제
            public void reset_Id()
            {
                verti_Time = 0;
                ip[0] = '\0';
                vertified = false;
                vertinum = 0;
            }
        }
        private static readonly byte key_Length = 13;
        private static readonly byte index_Length = 100;
        private static byte time_now;
        private static byte[] cursor;

        
        private static id_Table[,] id_T = new id_Table[key_Length,index_Length];
        public static void boot()
        {
            try
            {
                //id_T = id_Table[key_Length][index_Length];
                for (byte t = 0; t < key_Length; t++)
                {
                    for (byte i = 0; i < index_Length; i++)
                    {
                        id_T[t, i] = new id_Table();
                    }
                }
                time_now = 0;
                cursor = new byte[13];
                Console.WriteLine("Key Table Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!Key Table Loading Failed!!!");
                Console.WriteLine("<Key Table Error>");
                Console.WriteLine(e.Message);
                Console.WriteLine("<Key Table Error>");
            }
            finally
            {

            }
            
        }
        private static Object obj = new object();
        public static short add(string ip, int vertinum)
        {
            try
            {
                lock (obj)
                {
                    
                    short key_index = 0;
                    byte index = 0;
                    key_index = (short)(time_now * 1000);
                    key_index += cursor[time_now];

                    //아이피 넣기
                    foreach (char item in ip)
                    {
                        id_T[time_now, cursor[time_now]].ip[index] = item;
                        ++index;
                    }
                    id_T[time_now, cursor[time_now]].verti_Time = time_now;
                    id_T[time_now, cursor[time_now]].vertinum = vertinum;
                    ++cursor[time_now];
                    return key_index;
                }
            }
            catch (Exception)
            {
                return -1;
            }            
        }

        public static bool email_Vertify(string ip, ushort key_index, int vertinum)
        {
            try
            {
                byte index = (byte)(key_index % 1000);
                byte key = (byte)(key_index / 1000);


                //아이피와 일치하는가?
                byte ip_Index = 0;
                foreach (char item in ip)
                {
                    if (id_T[key, index].ip[ip_Index] != item)
                    {
                        return false;
                    }
                    ++ip_Index;
                }
                //3분인 이메일 인증 시간을 안넘겼을때
                //(테이블이 생성된 시간 ~ 테이블이 생성되고 3분이 
                //흐른 시간 사이에 time_now 가 있다면 테이블이 생성되고 3분이 안넘었다는뜻
                // tip => 서버의 구조상 실제 유저에게 주어지는 시간은
                //생성한 시간(1분미만) + 3분이다
                if ((id_T[key, index].verti_Time) % key_Length <= time_now 
                    && (id_T[key, index].verti_Time + 3) % key_Length >= time_now)
                {

                    //이메일 인증으로 들어온 숫자와
                    //테이블에 있는 인증 숫자가 같다면 인증성공
                    if (id_T[key, index].vertinum == vertinum)
                    {                            
                        //인증이 성공했다고 변수를 바꿈
                        id_T[key, index].vertified = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool is_Email_Vertified(string ip, ushort key_index)
        {
            byte index = (byte)(key_index % 1000);
            byte key = (byte)(key_index / 1000);

            //아이피가 일치하는가?
            byte ip_Index = 0;
            foreach (char item in ip)
            {
                if (id_T[key, index].ip[ip_Index] != item)
                {
                    return false;
                }
                ++ip_Index;
            }
            if(id_T[key, index].vertified == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        //리셋하고
        //time_now + 1 하기
        //버그예상요인 : 만약 삭제되는 도중에 위에서 새 테이블을 생성한다면?
        //지금 쓰이고 있는 테이블 바로 그 다음의 테이블을 비운다
        //테이블은 13개가 있고 1분마다 time_now가 더해지니까
        //한바퀴를 돌고 다시 자리로 오면 총13분이 지난것!
        //(13분은 이메일인증3분 + 이메일인증을 성공한뒤주어지는10분-> 키를 활용할 수 있는)
        public static void timer_Call()
        {
            byte future_time = (byte)((++time_now) % key_Length);
            for (int cur = cursor[future_time]; cur >= 0; cur--)
            {
                id_T[(time_now - 1), cur].reset_Id();
            }
            cursor[future_time] = 0;
            time_now = future_time;
        }
    }
}






