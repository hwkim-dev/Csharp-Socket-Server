using Login_Server;
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

static class Program
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
    
    private static char form;

    //클라이언트로부터 받을 값
    private static string txt;

    //클라이언트에게 보낼 값
    private static byte[] return_To_Client = new byte[50];

    //클라이언트에게 넘겨주기
    private static void asyncReceiveCallback(IAsyncResult asyncResult)
    {
        //받은 데이터
        AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
        //받은데이터 길이
        int nRecv = rcvData.Socket.EndReceive(asyncResult);

        //getString___From rcvData.Buffer -> <n ~ nRecv>
        string txt = Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv);
        //data Form확인하기
        Console.WriteLine(rcvData.Buffer[0]);
        Console.WriteLine(txt);

        
        unsafe
        {
            fixed (char* _rcvdata = txt) fixed (byte* _return_To_Client = return_To_Client)
            {
                
                Form.chech_From(_rcvdata, _return_To_Client);
            }
        }
        //보낼 데이터
        

        byte[] sendBytes = Encoding.UTF8.GetBytes("hi");
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
}