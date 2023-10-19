using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;

public class Clients
{
    //보낼 데이터
    //2번째(위치 = 1번째) 배열부터 보낼데이터 담기
    //1번쨰는 sendFormCode가 들어감
    private byte[] buf;


    //받은데이터 저장공간.
    private StringBuilder server_resp;
    //받은데이터길이
    int nRecv;
    public StringBuilder geta()
    {
        return server_resp;
    }

    //받을데이터가 들어가는곳
    byte[] recvBytes;


    //스레드 관리
    Thread clientThread;

    enum Purpose : byte
    {
        CHECKPW = 0b10000000,
    }

    Json_Maker jm;

    public Clients()
    {
        server_resp = new StringBuilder();

        jm = new Json_Maker();
    }
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

    //buf를 서버로 보낼 데이터
    //buf맨앞에 sendform 
    //뒤에 아이디 + 비밀번호
    //null은 buf에서 아이디와 비밀번호를 나누기 위한 수단

    //buf = Encoding.UTF8.GetBytes(null + Id_Input.text + null + PassWord_Input.text);

    //Enum의 SendFormCode중 보낼 형태
    //buf[0] = (byte) SendFormCode.LOGIN;
    //connect();

    //서버로부터 받은 파일 txt -> true면 로그인

    public bool sing_Up(string id, string pw, string email, string nickname)
    {
        //null은 sendForm을 담을곳!
        buf = Encoding.UTF8.GetBytes('\0'+ jm.SIGNUP(id, pw, email, nickname));
        buf[0] = (byte)SendFormCode.SIGNUP;
        send();
        
        //test용
        return (recvBytes[0] == 1) ? true : false ;
    }

    //purpose는 기본 true로!
    public bool login(string id, string pw, bool purpose)
    {
        buf = Encoding.UTF8.GetBytes('\0'+ jm.LOGIN(id, pw, purpose));
        buf[0] = (byte)SendFormCode.LOGIN;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }
    public string find_Id(string email)
    {
        buf = Encoding.UTF8.GetBytes('\0'+ jm.FINDID(email));
        
        buf[0] = (byte)SendFormCode.FINDID;
        send();

        if(recvBytes[0] == 1)
        {
            server_resp.Append(Encoding.UTF8.GetString(recvBytes, 1, nRecv));
        }
        else
        {
            return "fail";
        }
        //returns ID
        return server_resp.ToString();
    }
    public bool change_Id(string id, string new_id)
    {

        buf = Encoding.UTF8.GetBytes('\0'+ jm.CHANGEID(id, new_id));
        buf[0] = (byte)SendFormCode.CHANGEID;
        send();


        return (recvBytes[0] == 1) ? true : false ;
    }
    public bool change_Pw(string id, string new_Pw, string key)
    {
        //sql_builder.FINDID(email);

        buf = Encoding.UTF8.GetBytes('\0'+ jm.CHANGEPW(id, new_Pw, key));
        buf[0] = (byte)SendFormCode.CHANGEPW;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }
    public bool delete_Account(string id, string pw)
    {
        //sql_builder.FINDID(email);

        buf = Encoding.UTF8.GetBytes('\0'+ jm.DELETEACCOUNT(id, pw));
        buf[0] = (byte)SendFormCode.DELETEACCOUNT;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }
    //커서 리턴
    public sbyte email_Vertify(string email)
    {
        //sql_builder.FINDID(email);
        server_resp.Clear();
        buf = Encoding.UTF8.GetBytes('\0'+ jm.EMAILVERTIFY(email));
        buf[0] = (byte)SendFormCode.EMAILVERTIFY;
        send();

    
        if (recvBytes[0] == 1)
        {
            return (sbyte)recvBytes[1];
        }
        else
        {
            return -1;
        }
    }
    public bool id_Overlap(string id)
    {
        //sql_builder.FINDID(email);
        
        buf = Encoding.UTF8.GetBytes('\0'+ jm.IDOVERLAP(id));
        buf[0] = (byte)SendFormCode.IDOVERLAP;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }
    public bool nick_Overlap(string nickname)
    {
        //sql_builder.FINDID(email);

        buf = Encoding.UTF8.GetBytes('\0'+ jm.NICKOVERLAP(nickname));
        buf[0] = (byte)SendFormCode.NICKOVERLAP;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }
    public bool email_Overlap(string email)
    {
        //sql_builder.FINDID(email);

        buf = Encoding.UTF8.GetBytes('\0'+ jm.EMAILOVERLAP(email));
        buf[0] = (byte)SendFormCode.EMAILOVERLAP;
        send();

        return (recvBytes[0] == 1) ? true : false ;
    }

    //cursor는 아까 이메일인증으로 받은 값
    public ushort email_Verti_Correct(string _input, string _cursor)
    {
        //sql_builder.FINDID(email);

        buf = Encoding.UTF8.GetBytes('\0' + _input + _cursor);
        buf[0] = (byte)SendFormCode.EMAILVERTIFY;

        buf[0] = (byte)SendFormCode.EMAILVERTICORRECT;

        send();
        //key 리턴하기
        if (recvBytes[0] == 1)
        {
            ushort val = recvBytes[1];
            val = (ushort)(val << 8);
            val = recvBytes[2];
            return val;
        }
        else
        {
            return 0;
        }
    }

    private void send()
    {
        clientThread = new Thread(clientFunc);
        clientThread.IsBackground = true;
        clientThread.Start();
        clientThread.Join();
    }



    //==========수정금지==========
    private void clientFunc(object obj)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip_ad = IPAddress.Parse("20.194.44.146");
        EndPoint serverEP = new IPEndPoint(ip_ad, 11200);
        socket.Connect(serverEP);


        recvBytes = new byte[1024];

        socket.Send(buf);

        nRecv = socket.Receive(recvBytes);


        socket.Close();
    }
    //==========수정금지==========
}
