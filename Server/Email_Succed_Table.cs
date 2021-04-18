using System;
using System.Collections.Generic;
using System.Text;

namespace Login_Server
{
    static class Email_Succed_Table
    {
        private readonly static ushort[] SUCCED_TIMER = {   0b_0000_0000_0000_0000,
                                                            0b_0010_0000_0000_0000,
                                                            0b_0100_0000_0000_0000,
                                                            0b_0110_0000_0000_0000,
                                                            0b_1000_0000_0000_0000,
                                                            0b_1010_0000_0000_0000,
                                                            0b_1100_0000_0000_0000,
                                                            0b_1110_0000_0000_0000,};
        private static ushort time_Now = 0;
        private static ushort[] key_Length_List;

        private static ushort now_Cursor = 0;
        private static string[,] succedList;

        private static readonly byte TABLECOUNTS = 7;
        private static readonly byte TABLEUSERS = 60;
        public static void boot()
        {
            succedList = new string[TABLECOUNTS, TABLEUSERS];
            key_Length_List = new ushort[TABLECOUNTS];
        }
        public static sbyte[] add_Succed_List(string ip)
        {
            sbyte[] cursor = new sbyte[2];
            try
            {
                Console.WriteLine(time_Now);
                Console.WriteLine(now_Cursor);
                succedList[time_Now, now_Cursor] = ip;
                //now_Cursor 는 더하고
                //return 되는 cursor 는 (cursor +1 -1 = cursor)
                cursor[0] = (sbyte)(SUCCED_TIMER[time_Now] >> 8);
                cursor[1] = (sbyte)(++now_Cursor - 1);

                ++key_Length_List[time_Now];
                return cursor;
            }
            catch (Exception)
            {
                cursor[0] = -1;
                return cursor;
            }
            finally
            {

            }
        }

        //return IpAddress
        public static bool search_Succed_List(ushort _key, string Ip)
        {
            try
            {
                int current_Key = 0;
                foreach (ushort item in SUCCED_TIMER)
                {
                    //0b_0000_0000_0000_0000,
                    //0b_0010_0000_0000_0000,
                    //실제 키가 들어가는곳은
                    //뒤의 12자리이다
                    if (item < _key)
                    {

                    }
                    else
                    {
                        current_Key = item / 8192;
                        break;
                    }
                }
                _key = (ushort)(_key % 4096);
                
                Console.WriteLine(succedList[0, 0]);
                if (Ip.Equals(succedList[current_Key, _key]))
                {
                    return true;
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
            finally
            {

            }
        }
        private static ushort future_byte;
        public static void destroy()
        {
            future_byte = (ushort)((time_Now + 1) % TABLECOUNTS);
            key_Length_List[time_Now] = now_Cursor;
            now_Cursor = 0;
            try
            {
                do
                {
                    succedList[time_Now, key_Length_List[future_byte]] = "\0";
                    --key_Length_List[future_byte];
                    //의도된 오버플로우
                } while (key_Length_List[future_byte] != ushort.MaxValue);
                key_Length_List[future_byte] = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "cursor=" + now_Cursor);
            }
            finally
            {

            }


            time_Now = (ushort)((++time_Now) % TABLECOUNTS);
        }
    }
}
