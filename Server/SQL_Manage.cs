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
        static Mail_Sender ms = new Mail_Sender();
        private const byte SUCCED = 1;
        private const byte FAIL = 0;
        /*SqlConnection con;
        SqlCommand cmd;
        LOGIN_SQL(SqlConnection con)
        {
            this.con = con;
        }*/

        //Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
        //데이터베이스의 위치 location of Database
        static SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");

        [Serializable]
        private class SIGNUP_J
        {
            private string id;
            private string pw;
            private string email;
            private string nickname;

            public string Id { get => id; set => id = value; }
            public string Pw { get => pw; set => pw = value; }
            public string Email { get => email; set => email = value; }
            public string Nickname { get => nickname; set => nickname = value; }
        }
        public static unsafe void sign_Up(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                SIGNUP_J ins = JsonConvert.DeserializeObject<SIGNUP_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME) " +
                    "VALUES('" + ins.Id + "', '" + ins.Pw + "', '" + ins.Email + "', '" + ins.Nickname + "');", con);
                //cmd = new SqlCommand(uId.Json, con);

                con.Open();
                cmd.ExecuteNonQuery();
                *_return_To_Client = SUCCED;
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        [Serializable]
        private class LOGIN_J
        {
            private string id;
            private string pw;
            private bool purpose;

            
            public string Id { get => id; set => id = value; }
            public string Pw { get => pw; set => pw = value; }
            public bool Purpose { get => purpose; set => purpose = value; }
        }
        public static unsafe void login(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                LOGIN_J ins = JsonConvert.DeserializeObject<LOGIN_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT PW " +
                    "FROM ACCOUNT WHERE ID='" + ins.Id + "'", con);
                con.Open();

                //문제발생
                cmd.ExecuteNonQuery();
            
                SqlDataReader rdr = cmd.ExecuteReader();



                rdr.Read();

                Console.WriteLine(rdr["PW"].ToString().Trim() + "a");
                if (rdr["PW"].ToString().Trim().Equals(ins.Pw))
                {
                    Console.WriteLine("t");
                    * _return_To_Client = SUCCED;
                }
                else
                {
                    Console.WriteLine("F");
                    *_return_To_Client = FAIL;
                }
            }
            catch(Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }
        private static unsafe bool login(string id, string pw)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT PW " +
                    "FROM ACCOUNT WHERE ID='" + id + "'", con);
                con.Open();

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();



                rdr.Read();

                Console.WriteLine(rdr["PW"].ToString().Trim() + "a");
                if (rdr["PW"].ToString().Trim().Equals(pw))
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

        [Serializable]
        private class FINDID_J
        {
            private string email;
            
            public string Email { get => email; set => email = value; }
        }

        public static unsafe void find_Id(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                FINDID_J ins = JsonConvert.DeserializeObject<FINDID_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT ID " +
                    "FROM ACCOUNT WHERE EMAIL ='" + ins.Email + "'", con);

                con.Open();
                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();

                rdr.Read();
                *_return_To_Client = SUCCED;
                ++_return_To_Client;
                foreach(byte p in Encoding.UTF8.GetBytes(rdr["ID"].ToString().Trim()))
                {
                    
                    *_return_To_Client = p;
                    ++_return_To_Client;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        [Serializable]
        private class CHANGEID_J
        {
            private string id;
            private string new_id;

            
            public string Id { get => id; set => id = value; }
            public string New_Id { get => new_id; set => new_id = value; }
        }
        public static unsafe void change_Id(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                CHANGEID_J ins = JsonConvert.DeserializeObject<CHANGEID_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET ID='" + ins.New_Id + "' WHERE ID='" + ins.Id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();

                *_return_To_Client = SUCCED;
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        private class CHANGEPW_J
        {
            private string id;
            private string pw;
            private string new_Pw;

            
            public string Id { get => id; set => id = value; }
            public string Pw { get => pw; set => pw = value; }
            public string New_Pw { get => new_Pw; set => new_Pw = value; }
        }
        public static unsafe void change_Pw(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                CHANGEPW_J ins = JsonConvert.DeserializeObject<CHANGEPW_J>(uId.Json);
                SqlConnection con_New = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                        "SET PW='" + ins.New_Pw + "' WHERE ID='" + ins.Id + "'", con_New);
                if (login(ins.Id, ins.Pw))
                {
                    con_New.Open();
                    cmd.ExecuteNonQuery();
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        private class DELETEACCOUNT_J
        {
            private string id;
            private string pw;

            
            public string Id { get => id; set => id = value; }
            public string Pw { get => pw; set => pw = value; }
        }
        
        public static unsafe void delete_Accoutnt(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                DELETEACCOUNT_J ins = JsonConvert.DeserializeObject<DELETEACCOUNT_J>(uId.Json);
                SqlConnection con_New = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("DELETE FROM ACCOUNT " +
                    "WHERE ID = '" + ins.Id + "'", con_New);

                if (login(ins.Id, ins.Pw))
                {
                    con_New.Open();
                    cmd.ExecuteNonQuery();
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        private class EMAILVERTIFY_J
        {
            private string email;

            public string Email { get => email; set => email = value; }
        }
        public static unsafe void email_Vertify(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                EMAILVERTIFY_J ins = JsonConvert.DeserializeObject<EMAILVERTIFY_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT EMAIL FROM ACCOUNT " +
                    "WHERE EMAIL='" + ins.Email + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();

                rdr.Read();

                Console.WriteLine(rdr["EMAIL"].ToString().Trim());
                if (ms.send_Mail("hgim15338@gmail.com", "someone"))
                {
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        private class IDOVERLAP_J
        {
            private string id;

            public string Id { get => id; set => id = value; }
        }
        public static unsafe void id_Overlap(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                IDOVERLAP_J ins = JsonConvert.DeserializeObject<IDOVERLAP_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT ID " +
                    "FROM ACCOUNT WHERE ID='" + ins.Id + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        [Serializable]
        private class NICKOVERLAP_J
        {
            private string nickname;

            
            public string Nickname { get => nickname; set => nickname = value; }
        }
        public static unsafe void nick_Overlap(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                NICKOVERLAP_J ins = JsonConvert.DeserializeObject<NICKOVERLAP_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT NICKNAME " +
                    "FROM ACCOUNT WHERE NICKNAME='" + ins.Nickname + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }

        [Serializable]
        private class EMAILOVERLAP_J
        {
            private string email;

            
            public string Email { get => email; set => email = value; }
        }
        public static unsafe void email_Overlap(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                EMAILOVERLAP_J ins = JsonConvert.DeserializeObject<EMAILOVERLAP_J>(uId.Json);
                SqlCommand cmd = new SqlCommand("SELECT EMAIL " +
                    "FROM ACCOUNT WHERE EMAIL='" + ins.Email + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
                *_return_To_Client = SUCCED;
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }


        //[Serializable]
        //private class EMAILVERTICORRECT
        //{
        //    private string email;

            
        //    public string Email { get => email; set => email = value; }
        //}
        public static unsafe void email_Verti_Correct(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                //6자리의 수에 인증이있고
                //uId.Json[0~5]; 
                //여기에 아이디가 있다.
                //uId.Json[5~끝까지]; 
            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
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
    }
}