using Login_Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

public class AsyncStateData
{
    public byte[] Buffer;
    public Socket Socket;
}

static class Program
{
    //60초에 한번씩 pop하기
    static void timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Email_Vertify_Table.destroy();
    }

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting Server....");
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60000 = 60 초
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();


            using (Socket srvSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11200);
                srvSocket.Bind(endPoint);

                srvSocket.Listen(10);
                Console.WriteLine("Server is running");
                while (true)
                {
                    //외부 접속 받아들이는 클래스
                    Socket clntSocket = srvSocket.Accept();
                    //받아드릴 데이터가 있을때만 이 밑으로 들어감.
                    //Thread addTh = new Thread(accept);
                    //addTh.IsBackground = true;
                    //addTh.Start(clntSocket);
                    
                    ThreadPool.QueueUserWorkItem(accept, clntSocket);
                    //// 서버 소켓이 동작하는 스레드
                    //Thread serverThread = new Thread(serverFunc);
                    //serverThread.IsBackground = true;
                    //serverThread.Start();
                    //Thread.Sleep(500); // 소켓 서버용 스레드가 실행될 시간을 주기 위해

                    //Console.WriteLine("Pressany key to exit...");
                    ////thread.join이 없으니
                    ////메인스레드는 계속 감... 여기서 멈춰있다. -> readLine()때문에
                    //Console.ReadLine();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {

        }
    }

    private static void accept(Object clntSocket)
    {
        try
        {
            Socket _clntSocket = clntSocket as Socket;

            AsyncStateData data = new AsyncStateData();
            data.Buffer = new byte[128];
            data.Socket = _clntSocket;

            //byte[] recvBytes = new byte[1024];
            //int nRecv = clntSocket.Receive(recvBytes);
            //string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
            //Console.WriteLine(txt);
            //소켓으로 데이터 받기 시작

            _clntSocket.BeginReceive(data.Buffer, 0, data.Buffer.Length,
            SocketFlags.None, asyncReceiveCallback, data);
        }
        catch (Exception)
        {

        }
    }
    /*
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
                //받아드릴 데이터가 있을때만 이 밑으로 들어감.
                Thread addTh = new Thread();
                addTh.IsBackground = true;
                addTh.Start();
                AsyncStateData data = new AsyncStateData();
                data.Buffer = new byte[128];
                data.Socket = clntSocket;

                //byte[] recvBytes = new byte[1024];
                //int nRecv = clntSocket.Receive(recvBytes);
                //string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
                //Console.WriteLine(txt);
                //소켓으로 데이터 받기 시작

                clntSocket.BeginReceive(data.Buffer, 0, data.Buffer.Length,
                SocketFlags.None, asyncReceiveCallback, data);
            }
        }
    }*/


    //클라이언트에게 넘겨주기
    private static void asyncReceiveCallback(IAsyncResult asyncResult)
    {
        try
        {
            //받은 데이터
            AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
            //받은데이터 길이
            int nRecv = rcvData.Socket.EndReceive(asyncResult);

            //클라이언트에게 보낼 값
            byte[] return_to_client = new byte[50];

            //스레드 끼리 겹치지 않기 위해 새 클래스 생성
            User_Identity uId = new User_Identity();

            //Form기능 정하기
            uId.Fn = rcvData.Buffer[0];

            //getString___From rcvData.Buffer -> <n ~ nRecv>
            uId.Json = Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv);

            //data Form확인하기
            //Console.WriteLine(rcvData.Buffer[0]);
            Console.WriteLine(uId.Json);
            //uId.Fn = (byte)uId.Json[8];

            unsafe
            {
                fixed (byte* _return_to_client = return_to_client)
                {
                    Form.chech_From(uId, _return_to_client);
                }
            }
            //보낼 데이터
            Console.WriteLine(return_to_client[0]);
            Console.WriteLine(return_to_client[1]);
            Console.WriteLine(return_to_client[2]);
            Console.WriteLine(return_to_client[3]);
            //byte[] sendBytes = Encoding.UTF8.GetBytes("hi");
            rcvData.Socket.BeginSend(return_to_client, 0, return_to_client.Length,
            SocketFlags.None, asyncSendCallback, rcvData.Socket);
        }catch(Exception)
        {

        }
    }

    //소켓으로 보내고 소켓 닫기
    private static void asyncSendCallback(IAsyncResult asyncResult)
    {
        try
        {
            //보낼 데이터 소켓객체화
            Socket socket = asyncResult.AsyncState as Socket;
            //소켓 보내기
            socket.EndSend(asyncResult);
            //소켓 닫기
            socket.Close();
        }
        catch (Exception)
        {

        }
    }
}