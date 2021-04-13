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


        static SqlConnection con;

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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT PW " +
                    "FROM ACCOUNT WHERE ID='" + ins.Id + "'", con);
                
                con.Open();
                Console.WriteLine(1);
                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();


                rdr.Read();

                Console.WriteLine(rdr["PW"].ToString().Trim() + "|");
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
            catch(Exception e)
            {
                Console.WriteLine(e);
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
        }
        //내부적으로 사용할 로그인
        private static unsafe bool login(string id, string pw)
        {
            try
            {
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
            private string key;

            public string Id { get => id; set => id = value; }
            public string Pw { get => pw; set => pw = value; }
            public string New_Pw { get => new_Pw; set => new_Pw = value; }
            public string Key { get => key; set => key = value; }
        }
        public static unsafe void change_Pw(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                CHANGEPW_J ins = JsonConvert.DeserializeObject<CHANGEPW_J>(uId.Json);
                //if (uId.Key)
                //{

                //}
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                        "SET PW='" + ins.New_Pw + "' WHERE ID='" + ins.Id + "'", con);
                if (login(ins.Id, ins.Pw))
                {
                    con.Open();
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("DELETE FROM ACCOUNT " +
                    "WHERE ID = '" + ins.Id + "'", con);

                if (login(ins.Id, ins.Pw))
                {
                    con.Open();
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT EMAIL, ID FROM ACCOUNT " +
                    "WHERE EMAIL='" + ins.Email + "'", con);
                con.Open();
                
                cmd.ExecuteNonQuery();
                
                SqlDataReader rdr = cmd.ExecuteReader();

                rdr.Read();
                

                //첫번째에 이메일 주소
                //두번째에 보내는사람이름
                sbyte t = (sbyte)ms.send_Mail(rdr["EMAIL"].ToString().Trim(), rdr["ID"].ToString().Trim());
                
                if (t == -1)
                {
                    *_return_To_Client = FAIL;
                }
                else
                {
                    //t는 100을 넘지 못함
                    *_return_To_Client = SUCCED;
                    ++_return_To_Client;
                    *_return_To_Client = (byte)t;

                    //이제 여기에 키 등록
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hyunwoo\source\repos\Server\Server\Database1.mdf;Integrated Security=True");
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
        public static unsafe void email_Verti_Correct(User_Identity uId, byte*  _return_To_Client)
        {
            try
            {
                if (uId.getVerti() == Email_Vertify_Table.find(uId.getCursor()))
                {
                    *_return_To_Client = SUCCED;
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
    }
}