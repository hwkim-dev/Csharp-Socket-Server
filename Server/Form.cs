
using System;
using System.Collections.Generic;
using System.Text;
using Server_Command;
using Newtonsoft.Json;

namespace Login_Server
{
    [Flags]
    enum SendFormCode
    {
        SIGNUP = 1,
        LOGIN = 2,
        FINDID = 3,
        CHANGEID = 4,
        CHANGEPW = 5,
        DELETEACCOUNT = 6,
        EMAILVERTIFY = 7,
        IDOVERLAP = 8,
        NICKOVERLAP = 9,
        EMAILOVERLAP = 10,
        EMAILVERTICORRECT = 11,
        GOLDUPDATE = 12,
        GOLDSELECT = 13,
        DIAMONDUPDATE = 14,
        DIAMONDSELECT = 15,
        RUBYUPDATE = 16,
        RUBYSELECT = 17,
        PWEMAILSELECT = 18,
        GAMEREWARD = 19,
        SKILLSUPDATE = 20,
    }
    enum Purpose
    {
        CHECKPW = 0b10000000,
    }

    static class Form
    {
        public abstract class form_Manage
        {
            public abstract unsafe void excute(User_Identity uId, byte* _return_to_client);

        }
        class SIGNUP : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.sign_Up(uId, _return_to_client);
            }
        }
        class LOGIN : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.login(uId, _return_to_client);
            }
        }
        class FINDID : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.find_Id(uId, _return_to_client);
            }
        }
        class CHANGEID : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.change_Id(uId, _return_to_client);
            }
        }
        class CHANGEPW : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.change_Pw(uId, _return_to_client);
            }
        }
        class DELETEACCOUNT : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.delete_Accoutnt(uId, _return_to_client);
            }
        }
        class EMAILVERTIFY : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.email_Vertify(uId, _return_to_client);
            }
        }
        class IDOVERLAP : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.id_Overlap(uId, _return_to_client);
            }
        }
        class NICKOVERLAP : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.nick_Overlap(uId, _return_to_client);
            }
        }
        class EMAILOVERLAP : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.email_Overlap(uId, _return_to_client);
            }
        }
        class EMAILVERTICORRECT : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.email_Verti_Correct(uId, _return_to_client);
            }
        }
        class GOLDUPDATE : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.gold_update(uId, _return_to_client);
            }
        }
        class DIAMONDUPDATE : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.diamond_update(uId, _return_to_client);
            }
        }

        class RUBYUPDATE : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.ruby_update(uId, _return_to_client);
            }
        }

        class PWEMAILSELECT : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                //LOGIN_SQL.pwemail_select(uId, _return_to_client);
            }
        }

        class GAMEREWARD : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.game_reward(uId, _return_to_client);
            }
        }

        class SKILLSUPDATE : form_Manage
        {
            public override unsafe void excute(User_Identity uId, byte* _return_to_client)
            {
                LOGIN_SQL.skills_update(uId, _return_to_client);
            }
        }

        private static readonly byte form_Manage_Length = 21;
        public static form_Manage[] fm = new form_Manage[form_Manage_Length];
        public static void boot()
        {
            try
            {
                fm[(byte)SendFormCode.SIGNUP] = new SIGNUP();
                fm[(byte)SendFormCode.LOGIN] = new LOGIN();
                fm[(byte)SendFormCode.FINDID] = new FINDID();
                fm[(byte)SendFormCode.CHANGEID] = new CHANGEID();
                fm[(byte)SendFormCode.CHANGEPW] = new CHANGEPW();
                fm[(byte)SendFormCode.DELETEACCOUNT] = new DELETEACCOUNT();
                fm[(byte)SendFormCode.EMAILVERTIFY] = new EMAILVERTIFY();
                fm[(byte)SendFormCode.IDOVERLAP] = new IDOVERLAP();
                fm[(byte)SendFormCode.NICKOVERLAP] = new NICKOVERLAP();
                fm[(byte)SendFormCode.EMAILOVERLAP] = new EMAILOVERLAP();
                fm[(byte)SendFormCode.EMAILVERTICORRECT] = new EMAILVERTICORRECT();
                fm[(byte)SendFormCode.GOLDUPDATE] = new GOLDUPDATE();
                fm[(byte)SendFormCode.DIAMONDUPDATE] = new DIAMONDUPDATE();
                fm[(byte)SendFormCode.RUBYUPDATE] = new RUBYUPDATE();
                fm[(byte)SendFormCode.PWEMAILSELECT] = new PWEMAILSELECT();
                fm[(byte)SendFormCode.GAMEREWARD] = new GAMEREWARD();
                fm[(byte)SendFormCode.SKILLSUPDATE] = new SKILLSUPDATE();
                Console.WriteLine("Form Loaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!Form Loading Failed!!!");
                Console.WriteLine("<Form Load Error>");
                Console.WriteLine(e.Message);
                Console.WriteLine("<Form Load Error>");
            }
            finally
            {

            }
        }


        //fm[uId.type()].(uId, _return_to_client);

        /*
        public static unsafe void chech_From(User_Identity uId, byte* _return_to_client)
        {
            switch (uId.type())
            {
                case (byte)SendFormCode.SIGNUP:
                    LOGIN_SQL.sign_Up(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.LOGIN:
                    LOGIN_SQL.login(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.FINDID:
                    LOGIN_SQL.find_Id(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.CHANGEID:
                    LOGIN_SQL.change_Id(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.CHANGEPW:
                    LOGIN_SQL.change_Pw(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.DELETEACCOUNT:
                    LOGIN_SQL.delete_Accoutnt(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.EMAILVERTIFY:
                    LOGIN_SQL.email_Vertify(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.IDOVERLAP:
                    LOGIN_SQL.id_Overlap(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.NICKOVERLAP:
                    LOGIN_SQL.nick_Overlap(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.EMAILOVERLAP:
                    LOGIN_SQL.email_Overlap(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.EMAILVERTICORRECT:
                    LOGIN_SQL.email_Verti_Correct(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.GOLDUPDATE:
                    LOGIN_SQL.gold_update(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.GOLDSELECT:
                    LOGIN_SQL.gold_select(uId, _return_to_client);
                    break;
                case (byte)SendFormCode.DIAMONDUPDATE:
                     LOGIN_SQL.diamond_update(uId, _return_to_client);
                     break;
                case (byte)SendFormCode.DIAMONDSELECT:
                     LOGIN_SQL.diamond_select(uId, _return_to_client);
                     break;
                case (byte)SendFormCode.RUBYUPDATE:
                     LOGIN_SQL.ruby_update(uId, _return_to_client);
                     break;
                case (byte)SendFormCode.RUBYSELECT:
                     LOGIN_SQL.ruby_select(uId, _return_to_client);
                     break;
                case (byte)SendFormCode.PWEMAILSELECT:
                    //LOGIN_SQL.pwemail_select(uId, _return_to_client);
                    break;

                default:
                    break;
            }
        }*/
    }
}



