﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


static class Email_Vertify_Table
{
    
    //1분마다 총3분 -> 100개의 정보를담음
    private static int[,] table;
    private static sbyte cursor = 0;
    //커서의 정보를 담을 테이블 필요!
    private static sbyte[] cursor_Table;

    private static sbyte time = 0;
    //private static Dictionary<string, int> succedList = new Dictionary<string, int>();
    
    private static readonly byte TABLECOUNTS = 3;
    private static readonly byte TABLEUSERS = 100;
    
    private static object obj = new object();

    public static void boot()
    {
        cursor_Table = new sbyte[TABLECOUNTS];
        table = new int[TABLECOUNTS, TABLEUSERS];
    }

    
    
    public static sbyte add_Item(int verti_num)
    {
        //succedList.Contains
        lock (obj)
        {
            try
            {
                table[time, cursor] = verti_num;
                ++cursor;
                return cursor;
            }
            catch (Exception)
            {
                ++cursor;
                return -1;
            }
            finally
            {
            }
        }
    }
    
    public static bool find(byte _cursor, int correct)
    {
        byte _time = 0;
        //동시접근 여부 상관X
        //lock 하지 않음
        try
        {
            while (_time < 3)
            {
                if (table[_time, _cursor] == correct)
                {
                    return true;
                }
                else
                {
                    ++_time;
                }
            }
            return false;
        }
        catch (Exception)
        {
            //returns Null
            return false;
        }
        finally
        {

        }
    }

    private static sbyte future_byte;
    //C# Time 라이브러리에서 mm = minute만 받음
    public static void destroy()
    {
        try
        {
            future_byte = (sbyte)((time + 1) % TABLECOUNTS);
            cursor_Table[time] = cursor;
            do
            {
                //O(N)시간 소요
                table[cursor_Table[future_byte], cursor_Table[time]] = 0;
                --cursor_Table[time];
            } while (cursor >= 0);
            //다음에 사용하기 위해 cursor를 0으로 만들어줌
            ++cursor_Table[time];
            //++cursor 아님..
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "cursor=" + cursor);
        }
        finally
        {

        }
        //Destory()가 실행된후 time을 옮겨야 
        //삭제되는 도중 삽입되는 오류를 방지
        //3분에서 4분으로 바뀔때 실행
        //시간 + 1
        //TABLECOUNTS으로 나눠서 테이블 최대길이 초과하지 못하게 맞음
        time = (sbyte)((++time) % TABLECOUNTS);


    }

}