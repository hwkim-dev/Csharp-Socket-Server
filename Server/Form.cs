using System;
using System.Collections.Generic;
using System.Text;
using LOGIN_DATA;
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
    }
    enum Purpose
    {
        CHECKPW = 0b10000000,
    }

    static class Form
    {
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
                default:
                    break;
            }
        }
    }
}
