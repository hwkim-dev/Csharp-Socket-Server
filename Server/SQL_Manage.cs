using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace LOGIN_DATA
{
    enum SendFormCode : byte
    {
        SignUp = 0b0000000001,
        Login = 0b0000000010,
        FindID = 0b0000000011,
        FindPW = 0b0000000100,
        ChangeID = 0b0000000101,
        ChangePW = 0b0000000110,
        DeleteAccount = 0b0000000111,
        id_Overlap = 0b0000001000,
        isInputCorrect = 0b0000001001,
    }
    
    static class LOGIN_SQL
    {
        /*SqlConnection con;
        SqlCommand cmd;
        LOGIN_SQL(SqlConnection con)
        {
            this.con = con;
        }*/
        //데이터베이스의 위치 location of Database
        static SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
        static SqlCommand cmd;
        public static unsafe void SignUp(char* id, char* pw, char* email, char* nickname, bool* succed)
        {
            //새로 가입했을때 SQL문
            //INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME)
            //VALUES(, , , );
            try
            {
                cmd = new SqlCommand("INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME) " +
                "VALUES('" + (*id).ToString() + "','" + (*pw).ToString() + "','" +
                (*email).ToString() + "' , '" + (*nickname).ToString() + "')", con);

                con.Open();
                cmd.ExecuteNonQuery();

                *succed = true;
            }
            catch(Exception e)
            {
                *succed = false;
            }
            
        }


        //아이디 찾기 SQL문 -> 아이디중복체크 / 로그인
        //SELECT ID
        //FROM ACCOUNT
        //WHERE ID = 아이디입력
        public static unsafe void Login(char* id, char* pw, bool* succed)
        {
            cmd = new SqlCommand("SELECT PW FROM ACCOUNT WHERE ID ='" + (*id).ToString() + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();

            SqlDataReader rdr = cmd.ExecuteReader();
            
            rdr.Read();

            

            foreach (char k in rdr["PW"].ToString())
            {
                if (k == (char)32) { break; }
                if (k == *pw)
                {
                    Console.WriteLine(k);
                    Console.WriteLine((int)pw);
                    ++pw;
                }
                else //(k != *pww)
                {
                    //로그인 실패!
                    *succed = false;
                    break;
                }
            }

            //사용후 닫음
            rdr.Close();
        }
        public static unsafe void FindID(char* id, char* email, bool* succed)
        {
            cmd = new SqlCommand("SELECT ID FROM ACCOUNT WHERE EMAIL ='" + (*id).ToString() + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();

            SqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read();

            foreach (char k in rdr["EMAIL"].ToString())
            {
                if (k == (char)32) { break; }
                if (k == *email)
                {
                    Console.WriteLine((int)email);
                    ++email;
                }
                else //(k != *pww)
                {
                    //로그인 실패!
                    rdr.Close();
                    *succed = false;
                }
            }
        }
        public static unsafe void FindPW(char* id, char* email, int* correct, bool* succed)
        {
   
        }
        public static unsafe void ChangeID(char* id, char* new_id, bool* succed)
        {

        }
        public static unsafe void ChangePW(char* id, char* new_pw, bool* succed)
        {

        }
        public static unsafe void DeleteAccount(char* id, char* pw, bool* succed)
        {

        }
        public static unsafe void id_Overlap(char* id, bool* succed)
        {

        }
        public static unsafe void nick_Overlap(char* nickname, bool* succed)
        {

        }
        public static unsafe void email_Overlap(char* email, bool* succed)
        {

        }
        public static unsafe void isInputCorrect(int* correct, bool* succed)
        {

        }
        //아이디 찾기 SQL문 -> EMAIL로 아이디 찾을때/이메일로 보낼때
        //SELECT ID
        //FROM ACCOUNT
        //WHERE EMAIL = 입력된이메일

        //패스워드 찾기 SQL문 -> 로그인
        // SELECT PW
        // FROM ACCOUNT
        // WHERE PW = 비밀번호입력

        //아이디 바꾸기 SQL문 -> 
        //UPDATE ACCOUNT
        //SET ID = 새아이디
        //WHERE ID = 과거아이디

        //비밀번호 바꾸기 SQL문 ->
        //UPDATE ACCOUNT
        //SET PW = 새아이디
        //WHERE PW = 과거아이디

        //계졍삭제(하기전에 아이디비밀번호 맞는지부터 체크)
        //DELETE FROM ACCOUNT
        //WHERE ID = 입력한아이디
    }
}