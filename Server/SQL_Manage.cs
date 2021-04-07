using Login_Server;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace LOGIN_DATA
{
    static class LOGIN_SQL
    {
        /*SqlConnection con;
        SqlCommand cmd;
        LOGIN_SQL(SqlConnection con)
        {
            this.con = con;
        }*/

        //Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        //데이터베이스의 위치 location of Database
        static SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
        static SqlCommand cmd;

        [Serializable]
        private class SIGNUP_J
        {
            public byte fn;
            public string id;
            public string pw;
            public string email;
            public string nickname;
        }
        public static unsafe void sign_Up(byte* _return_To_Client)
        {
            SIGNUP_J instance = JsonConvert.DeserializeObject<SIGNUP_J>(User_Identity.Json);
            try
            {
                cmd = new SqlCommand(User_Identity.Json, con);

                con.Open();
                cmd.ExecuteNonQuery();

                _return_To_Client[0] = 1; //1 == true
            }
            catch (Exception)
            {
                _return_To_Client[0] = 0; //0 == false
            }
        }
        [Serializable]
        private class LOGIN_J
        {
            public byte fn;
            public string id;
            public string pw;
            public bool purpose;
        }
        public static unsafe void login(byte* _return_To_Client)
        {
            LOGIN_J instance = JsonConvert.DeserializeObject<LOGIN_J>(User_Identity.Json);
            cmd = new SqlCommand(User_Identity.Json, con);

            con.Open();
            cmd.ExecuteNonQuery();

            SqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read();

            //sbyte l = -1;
            foreach (byte k in Encoding.UTF8.GetBytes(rdr["ID"].ToString()))
            {
                if (k == (char)32) { break; }
                //_return_To_Client[++l] = k;
                *_return_To_Client = k;

                //배열의 다음인덱스
                ++_return_To_Client;
            }
        }
        public static unsafe void find_Id(byte* _return_To_Client)
        {

        }
        public static unsafe void find_Pw(byte* _return_To_Client)
        {

        }
        public static unsafe void change_Id(byte* _return_To_Client)
        {

        }
        public static unsafe void change_Pw(byte* _return_To_Client)
        {

        }
        public static unsafe void delete_Accoutnt(byte* _return_To_Client)
        {

        }
        public static unsafe void email_Vertify(byte* _return_To_Client)
        {

        }
        public static unsafe void id_Overlap(byte* _return_To_Client)
        {

        }
        public static unsafe void nick_Overlap(byte* _return_To_Client)
        {

        }
        public static unsafe void email_Overlap(byte* _return_To_Client)
        {

        }
        public static unsafe void is_Input_Correct(byte* _return_To_Client)
        {

        }
/*
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
//WHERE ID = 입력한아이디*/





[Serializable]
private class FINDID_J
{
    public byte fn;
    public string email;
}


[Serializable]
private class CHANGEID_J
{
    public byte fn;
    public string id;
    public string new_id;
}


/// <summary>
/// 
/// </summary>
[Serializable]
private class CHANGEPW_J
{
    public byte fn;
    public string id;
    public string pw;
    public string new_Pw;
}

/// <summary>
/// 
/// </summary>
[Serializable]
private class DELETEACCOUNT_J
{
    public byte fn;
    public string id;
    public string pw;
}


/// <summary>
/// 
/// </summary>
[Serializable]
private class EMAILVERTIFY_J
{
    public byte fn;
    public string email;
}


/// <summary>
/// 
/// </summary>
[Serializable]
private class IDOVERLAP_J
{
    public byte fn;
    public string id;
}

[Serializable]
private class NICKOVERLAP_J
{
    public byte fn;
    public string nickname;
}
[Serializable]
private class EMAILOVERLAP_J
{
    public byte fn;
    public string email;
}

[Serializable]
private class ISINPUTCORRECT_J
{
    public byte fn;
    public string email;
}
    }
}