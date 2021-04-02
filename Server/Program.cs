using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
public class AsyncStateData
{
    public byte[] Buffer;
    public Socket Socket;
}
enum SendFormCode : byte
{
    SignUp = 0b0000000001,
    Login = 0b0000000010,
    FindID = 0b0000000011,
    FindPW = 0b0000000100,
    //가능하면 추가
    ChangeID = 0b0000000101,
    ChangePW = 0b0000000110,
    DeleteAccount = 0b0000000111,
}

class Program
{
    
    static void Main(string[] args)
    {
        // 서버 소켓이 동작하는 스레드
        Thread serverThread = new Thread(serverFunc);
        serverThread.IsBackground = true;
        serverThread.Start();
        Thread.Sleep(500); // 소켓 서버용 스레드가 실행될 시간을 주기 위해

        Console.WriteLine("Pressany key to exit...");
        Console.ReadLine();
    }

    private static void serverFunc(object obj)
    {
        using (Socket srvSocket =
        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11200);
            srvSocket.Bind(endPoint);

            srvSocket.Listen(10);
            while (true)
            {
                //외부 접속 받아들이는 클래스
                Socket clntSocket = srvSocket.Accept();
                AsyncStateData data = new AsyncStateData();
                data.Buffer = new byte[1024];
                data.Socket = clntSocket;

                /*byte[] recvBytes = new byte[1024];
                int nRecv = clntSocket.Receive(recvBytes);
                string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
                Console.WriteLine(txt);*/
                //소켓으로 데이터 받기 시작

                clntSocket.BeginReceive(data.Buffer, 0, data.Buffer.Length,
                SocketFlags.None, asyncReceiveCallback, data);
            }
        }
    }
    private static String income;
    private static string txt;
    //클라이언트에게 넘겨주기
    private static void asyncReceiveCallback(IAsyncResult asyncResult)
    {
        //받은 데이터
        AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
        //받은데이터 길이
        int nRecv = rcvData.Socket.EndReceive(asyncResult);

        income = Encoding.UTF8.GetString(rcvData.Buffer, 0, 1);
        //data Form확인하기
        
        Console.WriteLine(Encoding.UTF8.GetString(rcvData.Buffer, 0, 1));

        //getString___From rcvData.Buffer -> <n ~ nRecv>
        txt = Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv);
        Console.WriteLine(txt);
        
        //보낼 데이터
        byte[] sendBytes = Encoding.UTF8.GetBytes("Hello: " + txt);

        rcvData.Socket.BeginSend(sendBytes, 0, sendBytes.Length,
        SocketFlags.None, asyncSendCallback, rcvData.Socket);

    }

    //소켓으로 보내고 소켓 닫기
    private static void asyncSendCallback(IAsyncResult asyncResult)
    {
        //보낼 데이터 소켓객체화
        Socket socket = asyncResult.AsyncState as Socket;
        //소켓 보내기
        socket.EndSend(asyncResult);
        //소켓 닫기
        socket.Close();
    }
    
    //나중에 비트연산으로 바꾸기
    //static class[] form = {  };
    private static string check_Form(byte income)
    {
        foreach (byte form in Enum.GetValues(typeof(SendFormCode)))
        {
            if(income == form)
            {
                return Enum.GetName(typeof(SendFormCode), form); ;
            }
        }
        return null;
    }
}