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

    //클라이언트에게 넘겨주기
    private static void asyncReceiveCallback(IAsyncResult asyncResult)
    {
        AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
        int nRecv = rcvData.Socket.EndReceive(asyncResult);
        string txt = Encoding.UTF8.GetString(rcvData.Buffer, 0, nRecv);
        Console.WriteLine(txt);
        byte[] sendBytes = Encoding.UTF8.GetBytes("Hello: " + txt);
        rcvData.Socket.BeginSend(sendBytes, 0, sendBytes.Length,
        SocketFlags.None, asyncSendCallback, rcvData.Socket);

    }

    //소켓으로 보내고 소켓 닫기
    private static void asyncSendCallback(IAsyncResult asyncResult)
    {
        Socket socket = asyncResult.AsyncState as Socket;
        socket.EndSend(asyncResult);
        socket.Close();
    }
}