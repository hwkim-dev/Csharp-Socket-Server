using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

public class Json_Maker
{
    [Serializable]
    private class SIGNUP_J
    {
        public string id;
        public string pw;
        public string email;
        public string nickname;

        public SIGNUP_J(string _id, string _pw, string _email, string _nickname)
        {
            this.id = _id;
            this.pw = _pw;
            this.email = _email;
            this.nickname = _nickname;
        }
    }
    public string SIGNUP(string _id, string _pw, string _email, string _nickname)
    {
        SIGNUP_J sign_Up = new SIGNUP_J(_id, _pw, _email, _nickname);
        return JsonConvert.SerializeObject(sign_Up);
    }

    [Serializable]
    private class LOGIN_J
    {
        public string id;
        public string pw;
        public bool purpose;

        public LOGIN_J(string _id, string _pw, bool _purpose)
        {
            this.id = _id;
            this.pw = _pw;
            this.purpose = _purpose;
        }
    }
    public string LOGIN(string _id, string _pw, bool _purpose)
    {
        LOGIN_J login = new LOGIN_J(_id, _pw, _purpose);
        return JsonConvert.SerializeObject(login);
    }

    [Serializable]
    private class FINDID_J
    {
        public string email;

        public FINDID_J(string _email)
        {
            this.email = _email;
        }
    }
    public string FINDID(string _email)//id?
    {
        FINDID_J find_Id = new FINDID_J(_email);
        return JsonConvert.SerializeObject(find_Id);
    }

    [Serializable]
    private class CHANGEID_J
    {
        public string id;
        public string new_id;

        public CHANGEID_J(string _id, string _new_id)
        {
            this.id = _id;
            this.new_id = _new_id;
        }
    }
    public string CHANGEID(string _id, string _new_id)
    {
        CHANGEID_J change_Id = new CHANGEID_J(_id, _new_id);
        return JsonConvert.SerializeObject(change_Id);
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class CHANGEPW_J
    {
        public string id;
        public string new_Pw;
        public string key;

        public CHANGEPW_J(string _id, string _new_Pw, string _key)
        {
            this.id = _id;
            this.new_Pw = _new_Pw;
            this.key = _key;
        }
    }
    public string CHANGEPW(string _id, string _new_Pw, string _key)
    {
        CHANGEPW_J change_Pw = new CHANGEPW_J(_id, _new_Pw, _key);
        return JsonConvert.SerializeObject(change_Pw);
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class DELETEACCOUNT_J
    {
        public string id;
        public string pw;

        public DELETEACCOUNT_J( string _id, string _pw)
        {
            this.id = _id;
            this.pw = _pw;
        }
    }
    public string DELETEACCOUNT(string _id, string _pw)//chech pw
    {
        DELETEACCOUNT_J delete_Account = new DELETEACCOUNT_J(_id, _pw);
        return JsonConvert.SerializeObject(delete_Account);
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class EMAILVERTIFY_J
    {
        public string email;

        public EMAILVERTIFY_J(string _email)
        {
            this.email = _email;
        }
    }
    public string EMAILVERTIFY(string _email)
    {
        EMAILVERTIFY_J email_Vertify = new EMAILVERTIFY_J(_email);
        return JsonConvert.SerializeObject(email_Vertify);
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class IDOVERLAP_J
    {
        public string id;

        public IDOVERLAP_J(string _id)
        {
            this.id = _id;
        }
    }
    public string IDOVERLAP(string _id)
    {
        IDOVERLAP_J id_Overlap = new IDOVERLAP_J(_id);
        return JsonConvert.SerializeObject(id_Overlap);
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class NICKOVERLAP_J
    {
        public string nickname;

        public NICKOVERLAP_J(string _nickname)
        {
            this.nickname = _nickname;
        }
    }
    public string NICKOVERLAP(string _nickname)
    {
        NICKOVERLAP_J nick_Overlap = new NICKOVERLAP_J(_nickname);
        return JsonConvert.SerializeObject(nick_Overlap);
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    private class EMAILOVERLAP_J
    {
        public string email;

        public EMAILOVERLAP_J(string _email)
        {
            this.email = _email;
        }
    }
    public string EMAILOVERLAP(string _email)
    {
        EMAILOVERLAP_J nick_Overlap = new EMAILOVERLAP_J(_email);
        return JsonConvert.SerializeObject(nick_Overlap);
    }

    //[Serializable]
    //private class EMAILVERTICORRECT_J
    //{
    //    public string email;

    //    public EMAILVERTICORRECT_J(string _input)
    //    {
    //        this.email = _input;
    //    }
    //}
    //public string EMAILVERTICORRECT(string _input)
    //{
    //    EMAILVERTICORRECT_J email_Verti_Correct = new EMAILVERTICORRECT_J(_input);
    //    return JsonConvert.SerializeObject(email_Verti_Correct);
    //}
}
