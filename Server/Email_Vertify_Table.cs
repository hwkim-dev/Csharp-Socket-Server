using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

static class Email_Vertify_Table
{
    //1분마다 총3분 -> 100개의 정보를담음
    private static int[,] table = new int[3, 100];

    private static sbyte cursor = 0;
    private static sbyte time = 0;
    //public Email_Vertify_Table()
    //{
    //    cursor = 0;
    //    time = 0;
    //    table = new int[3, 100];
    //}

    public static sbyte add_Item(int verti_num)
    {
        lock (null)
        {
            try
            {
                table[time, cursor] = verti_num;
                ++cursor;
                return cursor;
            }
            catch (Exception)
            {
                return cursor;
            }
            finally
            {

            }
        }
    }

    public static int find(byte _cursor)
    {
        //동시접근 여부 상관X
        //lock 하지 않음
        try
        {
            return table[time, _cursor];
        }
        catch (Exception)
        {
            //returns Null
            return '\0';
        }
        finally
        {

        }
    }

    //C# Time 라이브러리에서 mm = minute만 받음
    public static void Destroy()
    {
        try
        {
            do
            {
                //O(N)시간 소요
                table[time, cursor] = 0;
                --cursor;
            } while (cursor >= 0);
            //다음에 사용하기 위해 cursor를 0으로 만들어줌
            ++cursor;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "cursor=" + cursor);
        }

        //Destory()가 실행된후 time을 옮겨야 
        //삭제되는 도중 삽입되는 오류를 방지
        //3분에서 4분으로 바뀔때 실행
        //시간 + 1
        //3으로 나눠서 3을 초과하지 못하게 맞음
        time = (sbyte)((++time) % 3);
    }

}