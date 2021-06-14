using Login_Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.IO;

public class AsyncStateData
{
    public byte[] Buffer;
    public Socket Socket;
    public string Ip;
}

namespace Server_Command
{
    static class Program
    {

        //60초에 한번씩 pop하기
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Key_Table.timer_Call();

            //Email_Vertify_Table.destroy();
            //Email_Succed_Table.destroy();
        }
        static void time_T(object inst)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60000 = 60 초
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static void Main(string[] args)
        {
            try
            {
                {
                    string myCom = Dns.GetHostName();

                    IPHostEntry entry = Dns.GetHostEntry(myCom);
                    foreach (IPAddress iPAddress in entry.AddressList)
                    {
                        Console.WriteLine(iPAddress.AddressFamily + ":" + iPAddress);
                    }
                }
                Console.WriteLine("Starting Server....");
                Console.WriteLine("Time : " + DateTime.Now + "\n");
                Thread time_Th = new Thread(time_T);
                time_Th.Start();
                Console.WriteLine("Timer Loaded");
                Key_Table.boot();
                //ip_Ban_List.boot();
                LOGIN_SQL.boot();
                Form.boot();



                using (Socket srvSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11200);
                    
                    srvSocket.Bind(endPoint);

                    //접속가능 최대 클라이언트
                    srvSocket.Listen(10000);
                    Console.WriteLine("\nServer is running");
                    Console.WriteLine("Time : " + DateTime.Now);
                    while (true)
                    {
                        //Accept input
                        Socket clntSocket = srvSocket.Accept();
                        
                        //Console.WriteLine(clntSocket.RemoteEndPoint.ToString());

                        ThreadPool.QueueUserWorkItem(accept, clntSocket);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
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
                data.Ip = _clntSocket.RemoteEndPoint.ToString();


                _clntSocket.BeginReceive(data.Buffer, 0, data.Buffer.Length,
                SocketFlags.None, asyncReceiveCallback, data);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR! : Unable to Receive Data");
            }
            finally
            {

            }
        }
        //헤더정의. 
        //클라이언트에게 넘겨주기
        private static void asyncReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                //받은 데이터
                AsyncStateData rcvData = asyncResult.AsyncState as AsyncStateData;
                //받은데이터 길이
                int nRecv = rcvData.Socket.EndReceive(asyncResult);

                

                //스레드 끼리 겹치지 않기 위해 스레드가 생성된 후에 새 클래스 생성
                User_Identity uId = new User_Identity();

                //Form function
                uId.Fn = rcvData.Buffer[0];

                uId.set_Ip_Addr = rcvData.Ip;
                Console.WriteLine("\n<New Connection Established>\n" +
                    "IP : " + uId.get_Ip_Addr());

                uId.Json = Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv);


                Console.WriteLine(uId.Json);

                // 1 / 38 / 13 / 1 / 1 / 1 / 3 / 1 / 1 / 1 / 1 / 
                //로그인 -> return_to_client = new byte[50];
                //

                /*
                //클라이언트에게 보낼 값
                byte[] return_to_client;
                switch(uId.type())
                {
                    case (byte)SendFormCode.LOGIN:
                        return_to_client = new byte[26];
                        break;
                    case (byte)SendFormCode.FINDID:
                        return_to_client = new byte[13];
                        break;
                    case (byte)SendFormCode.EMAILVERTIFY:
                        return_to_client = new byte[3];
                        break;
                    default:
                        return_to_client = new byte[1];
                        break;
                }
                */
                byte[] return_to_client = new byte[100];

                unsafe
                {
                    fixed (byte* _return_to_client = return_to_client)
                    {
                        //Form.chech_From(uId, _return_to_client);
                        Form.fm[uId.type()].excute(uId, _return_to_client);
                    }
                }
                

                rcvData.Socket.BeginSend(return_to_client, 0, return_to_client.Length,
                SocketFlags.None, asyncSendCallback, rcvData.Socket);
            }
            catch (Exception e)
            {
                Console.WriteLine("!!!Connection Error Occur!!!\n" +
                    "<Connection Error>\n" +
                    e.Message + "\n" +
                    "<Connection Error>");
            }
            finally
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
            catch (Exception e)
            {
                Console.WriteLine("!!!Callback Error Occur!!!");
                Console.WriteLine("<Callback Error>");
                Console.WriteLine(e.Message);
                Console.WriteLine("<Callback Error>");
            }
            finally
            {

            }
        }

        
    }
}
