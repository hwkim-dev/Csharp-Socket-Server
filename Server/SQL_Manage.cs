using System;
using System.Data;
using System.Data.SqlClient;

namespace LOGIN_DATA
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("",con);
            con.Open();
            cmd.ExecuteNonQuery();
            Console.WriteLine("hi~");
        }
    }
}