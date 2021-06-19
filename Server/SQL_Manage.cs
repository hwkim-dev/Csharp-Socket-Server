
using Login_Server;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Server_Command
{

    static class Log_File
    {
        public static string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }
        public static void Log(string msg)
        {
            string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Logs\" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Logs\";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);
                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", GetDateTime(), msg);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", GetDateTime(), msg);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
    static class LOGIN_SQL
    {
        static Mail_Sender ms;
        private const byte SUCCED = 1;
        private const byte OBJECT_NULL = 0;
        private const byte FAIL = 0;
        private const int INT_TO_BYTE = 0b00000000_00000000_00000000_11111111;


        static SqlConnection con;

        public static void boot()
        {
            try
            {
                ms = new Mail_Sender();

                Console.WriteLine("Login SQL Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!Login SQL Loading Failed!!!");
                Console.WriteLine("<Login SQL Error>");
                Console.WriteLine(e.Message);
                Console.WriteLine("<Login SQL Error>");
            }
            finally
            {

            }
        }

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
        public static unsafe void sign_Up(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                SIGNUP_J ins = JsonConvert.DeserializeObject<SIGNUP_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME, GOLD, DIAMOND, RUBY, SKILLS) " +
                    "VALUES('" + ins.Id + "', '" + ins.Pw + "', '" + ins.Email + "', '" + ins.Nickname + "', '3000', '5000', '30', '0');", con);
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
        private class EndReceive
        {
            public string gold, diamond, ruby;
            public string nickname;
            public string skills;
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
        public static unsafe void login(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                LOGIN_J ins = JsonConvert.DeserializeObject<LOGIN_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT PW " +
                    "FROM ACCOUNT WHERE ID='" + ins.Id + "'", con);


                con.Open();

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();

                //아이디의 존재여부?
                if (rdr.Read())
                {
                    //비밀번호와 매치하는가?
                    if (rdr["PW"].ToString().Trim().Equals(ins.Pw))
                    {
                        *_return_To_Client = SUCCED;
                        /*++_return_To_Client;

                        byte k = (byte)rdr["PW"].ToString().Trim().Length;
                        foreach (byte p in Encoding.UTF8.GetBytes(rdr["PW"].ToString().Trim().Substring(0, k)))
                        {
                            *_return_To_Client = p;
                            ++_return_To_Client;
                        }*/

                        if (ins.Purpose == true)
                        {
                            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");

                            SqlCommand cmd2 = new SqlCommand("SELECT NICKNAME, GOLD, RUBY, DIAMOND, SKILLS " +
                                "FROM ACCOUNT WHERE ID='" + ins.Id + "'", con);

                            con.Open();
                            cmd2.ExecuteNonQuery();
                            SqlDataReader rdr2 = cmd2.ExecuteReader();

                            if (rdr2.Read())
                            {
                                EndReceive enR = new EndReceive();
                                enR.gold = rdr2["GOLD"].ToString().Trim();
                                enR.ruby = rdr2["RUBY"].ToString().Trim();
                                enR.diamond = rdr2["DIAMOND"].ToString().Trim();
                                enR.skills = rdr2["SKILLS"].ToString().Trim();
                                enR.nickname = rdr2["NICKNAME"].ToString().Trim();
                                byte[] x = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(enR));

                                foreach (byte item in x)
                                {
                                    ++_return_To_Client;
                                    *_return_To_Client = item;
                                }

                                /*
                                ++_return_To_Client;
                                int k = (int)rdr2["GOLD"];
                                
                                *_return_To_Client = (byte)((k >> 24) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((k >> 16) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((k >> 8) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((k >> 0) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;

                                ++_return_To_Client;
                                int p = (int)rdr2["RUBY"];
                                
                                *_return_To_Client = (byte)((p >> 24) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((p >> 16) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((p >> 8) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((p >> 0) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);

                                ++_return_To_Client;
                                int j = (int)rdr2["DIAMOND"];
                                
                                *_return_To_Client = (byte)((j >> 24) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((j >> 16) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((j >> 8) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);
                                ++_return_To_Client;
                                *_return_To_Client = (byte)((j >> 0) & INT_TO_BYTE);
                                Console.WriteLine(*_return_To_Client);



                                ++_return_To_Client;
                                *_return_To_Client = (byte)rdr2["SKILLS"];
                                Console.WriteLine(*_return_To_Client);

                                */
                                /*
                                foreach (char item in rdr2["NICKNAME"].ToString().Trim().ToCharArray())
                                {
                                    ++_return_To_Client;
                                    *_return_To_Client = (byte)item;
                                    Console.WriteLine(*_return_To_Client);

                                }
                                ++_return_To_Client;
                                *_return_To_Client = 0;
                                */
                            }
                            else
                            {
                                *_return_To_Client = OBJECT_NULL;
                                //골드 루비 다이아 닉네임중 하나가 NULL임
                            }
                        }
                    }
                    else
                    {
                        *_return_To_Client = FAIL;
                    }
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
        //내부적으로 사용할 로그인
        private static unsafe bool login(string id, string pw)
        {
            try
            {
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT PW " +
                    "FROM ACCOUNT WHERE ID='" + id + "'", con);
                con.Open();

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();



                if (rdr.Read())
                {
                    if (rdr["PW"].ToString().Trim().Equals(pw))
                    {
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

        public static unsafe void find_Id(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                FINDID_J ins = JsonConvert.DeserializeObject<FINDID_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT ID " +
                    "FROM ACCOUNT WHERE EMAIL ='" + ins.Email + "'", con);

                con.Open();
                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    *_return_To_Client = SUCCED;
                    ++_return_To_Client;
                    //UTF8 = byte배열 2칸차지
                    byte k = (byte)rdr["ID"].ToString().Trim().Length;
                    foreach (byte p in Encoding.UTF8.GetBytes(rdr["ID"].ToString().Trim()))
                    {
                        *_return_To_Client = p;
                        ++_return_To_Client;
                    }
                    /*foreach (byte p in Encoding.UTF8.GetBytes(rdr["ID"].ToString().Trim().Substring(0, k - 3) + "***"))
                    {
                        *_return_To_Client = p;
                        ++_return_To_Client;
                    }*/
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
        private class CHANGEID_J
        {
            private string id;
            private string new_id;


            public string Id { get => id; set => id = value; }
            public string New_Id { get => new_id; set => new_id = value; }
        }
        public static unsafe void change_Id(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                CHANGEID_J ins = JsonConvert.DeserializeObject<CHANGEID_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
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
            private string new_Pw;
            private ushort key;

            public string Id { get => id; set => id = value; }
            public string New_Pw { get => new_Pw; set => new_Pw = value; }
            public ushort Key { get => key; set => key = value; }
        }
        public static unsafe void change_Pw(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                CHANGEPW_J ins = JsonConvert.DeserializeObject<CHANGEPW_J>(uId.Json);

                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                        "SET PW='" + ins.New_Pw + "' WHERE ID='" + ins.Id + "'", con);

                if (Key_Table.is_Email_Vertified(uId.get_Ip_Addr(), ins.Key))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
                /*
                if (Email_Succed_Table.search_Succed_List(ins.Key, uId.get_Ip_Addr()))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
                */
            }
            catch (Exception)
            {
                Console.WriteLine("exception");
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

        public static unsafe void delete_Accoutnt(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                DELETEACCOUNT_J ins = JsonConvert.DeserializeObject<DELETEACCOUNT_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
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
        public static unsafe void email_Vertify(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                EMAILVERTIFY_J ins = JsonConvert.DeserializeObject<EMAILVERTIFY_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("SELECT EMAIL, ID FROM ACCOUNT " +
                    "WHERE EMAIL='" + ins.Email + "'", con);
                con.Open();

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();

                rdr.Read();


                //첫번째에 이메일 주소
                //두번째에 보내는사람이름
                int t = ms.send_Mail(rdr["EMAIL"].ToString().Trim(), rdr["ID"].ToString().Trim());
                if (t == -1)
                {
                    *_return_To_Client = FAIL;
                }
                else
                {
                    short key = Key_Table.add(uId.get_Ip_Addr(), t);
                    //t는 100을 넘지 못함
                    if (key > -1)
                    {
                        *_return_To_Client = SUCCED;
                        ++_return_To_Client;
                        //1000으로 나눈
                        //키값이 들어감
                        //0~13사이값
                        *_return_To_Client = (byte)(key / 1000);
                        ++_return_To_Client;
                        //1000보다 작은
                        //0~100사이의 index값이 들어감
                        *_return_To_Client = (byte)(key % 1000);
                        ++_return_To_Client;
                        //*_return_To_Client = (byte)(t / 10000);
                        //++_return_To_Client;
                        //*_return_To_Client = (byte)((t / 100) % 100);
                        //++_return_To_Client;
                        //*_return_To_Client = (byte)((t) % 100);
                    }
                    else
                    {

                        *_return_To_Client = FAIL;
                    }
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
        public static unsafe void id_Overlap(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                IDOVERLAP_J ins = JsonConvert.DeserializeObject<IDOVERLAP_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
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
        public static unsafe void nick_Overlap(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                NICKOVERLAP_J ins = JsonConvert.DeserializeObject<NICKOVERLAP_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
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
        private class EMAILVERTICORRECT_J
        {
            private ushort keyIndex;
            private int vertinum;
            public ushort KeyIndex { get => keyIndex; set => keyIndex = value; }
            public int Vertinum { get => vertinum; set => vertinum = value; }
        }

        public static unsafe void email_Verti_Correct(User_Identity uId, byte* _return_To_Client)
        {
            EMAILVERTICORRECT_J ins = JsonConvert.DeserializeObject<EMAILVERTICORRECT_J>(uId.Json);
            try
            {
                if (Key_Table.email_Vertify(uId.get_Ip_Addr(), ins.KeyIndex, ins.Vertinum))
                {
                    *_return_To_Client = SUCCED;
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
                /*
                //여기서 fail이 리턴되는 문제가 발생하고있다.
                if (Email_Vertify_Table.find(uId.getCursor(), uId.getVerti()))
                {
                    sbyte[] key = Email_Succed_Table.add_Succed_List(uId.get_Ip_Addr());

                    if (key[0] != -1)
                    {
                        *_return_To_Client = SUCCED;
                        ++_return_To_Client;
                        *_return_To_Client = (byte)key[0];
                        ++_return_To_Client;
                        *_return_To_Client = (byte)key[1];
                    }
                    else
                    {
                        *_return_To_Client = FAIL;
                    }
                }
                else
                {
                    *_return_To_Client = FAIL;
                }
                */
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
        public static unsafe void email_Overlap(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                EMAILOVERLAP_J ins = JsonConvert.DeserializeObject<EMAILOVERLAP_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
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
                //*_return_To_Client = SUCCED;

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
        private class GOLDUPDATE_J
        {
            private string id;
            private int gold;

            public string Id { get => id; set => id = value; }
            public int Gold { get => gold; set => gold = value; }
        }
        public static unsafe void gold_update(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                GOLDUPDATE_J ins = JsonConvert.DeserializeObject<GOLDUPDATE_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET GOLD = " + ins.Gold + " WHERE ID='" + ins.Id + "'", con);
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
                //Console.WriteLine(Program.logFileWrite(ins.Id, "Gold", ins.Gold));
                //*_return_To_Client = SUCCED;


            }
            catch (Exception)
            {
                *_return_To_Client = FAIL;
            }
            finally
            {

            }
            /*Log_File lg = new Log_File();
            lg.Log(rdr["GOLD"].ToString());*/
        }

        [Serializable]
        private class DIAMONDUPDATE_J
        {
            private string id;
            private int diamond;

            public string Id { get => id; set => id = value; }
            public int Diamond { get => diamond; set => diamond = value; }
        }
        public static unsafe void diamond_update(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                DIAMONDUPDATE_J ins = JsonConvert.DeserializeObject<DIAMONDUPDATE_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET DIAMOND = " + ins.Diamond + " WHERE ID='" + ins.Id + "'", con);
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
                //*_return_To_Client = SUCCED;

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
        private class RUBYUPDATE_J
        {
            private string id;
            private int ruby;

            public string Id { get => id; set => id = value; }
            public int Ruby { get => ruby; set => ruby = value; }
        }
        public static unsafe void ruby_update(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                RUBYUPDATE_J ins = JsonConvert.DeserializeObject<RUBYUPDATE_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET RUBY = " + ins.Ruby + " WHERE ID='" + ins.Id + "'", con);
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
                //*_return_To_Client = SUCCED;

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
        private class GAMEREWARD_J
        {
            private string nickname;
            private int gold;
            private int diamond;
            private int ruby;

            public string Nickname { get => nickname; set => nickname = value; }
            public int Gold { get => gold; set => gold = value; }
            public int Diamond { get => diamond; set => diamond = value; }
            public int Ruby { get => ruby; set => ruby = value; }
        }
        public static unsafe void game_reward(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                GAMEREWARD_J ins = JsonConvert.DeserializeObject<GAMEREWARD_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET GOLD = " + ins.Gold + ", DIAMOND = " + ins.Diamond + ", RUBY = " + ins.Ruby + " WHERE NICKNAME='" + ins.Nickname + "'", con);
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
        private class SKILLSUPDATE_J
        {
            private string nickname;
            private int skills;

            public string Nickname { get => nickname; set => nickname = value; }
            public int Skills { get => skills; set => skills = value; }
        }
        public static unsafe void skills_update(User_Identity uId, byte* _return_To_Client)
        {
            try
            {
                SKILLSUPDATE_J ins = JsonConvert.DeserializeObject<SKILLSUPDATE_J>(uId.Json);
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\h\Desktop\Csharp-Socket-Server-master\Server\Database1.mdf;Integrated Security=True");
                SqlCommand cmd = new SqlCommand("UPDATE ACCOUNT " +
                    "SET SKILLS = " + ins.Skills + " WHERE NICKNAME='" + ins.Nickname + "'", con);

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
                //*_return_To_Client = SUCCED;

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


