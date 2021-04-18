using Login_Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;

public class AsyncStateData
{
    public byte[] Buffer;
    public Socket Socket;
    public string Ip;
}

namespace LOGIN_DATA
{
    static class Program
    {

        //60초에 한번씩 pop하기
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Email_Vertify_Table.destroy();
            //Key_Table.key_Expires();
            Email_Succed_Table.destroy();
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
                Console.WriteLine("Starting Server....");
                Thread time_Th = new Thread(time_T);
                time_Th.Start();
                Email_Succed_Table.boot();
                Email_Vertify_Table.boot();
                //sbyte[] hi = Email_Succed_Table.add_Succed_List("182.168.4.2");
                //Console.WriteLine(hi[0] + "|" + hi[1]);
                //Console.WriteLine(Email_Succed_Table.search_Succed_List((byte)hi[0], "182.168.4.2"));
                LOGIN_SQL.boot();

            using (Socket srvSocket =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 11200);
                    srvSocket.Bind(endPoint);
                    
                    srvSocket.Listen(10);
                    Console.WriteLine("Server is running");
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

                //스레드 끼리 겹치지 않기 위해 스레드가 생성된 후에 새 클래스 생성
                User_Identity uId = new User_Identity();

                //Form function
                uId.Fn = rcvData.Buffer[0];
                Console.WriteLine(Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv));

                uId.set_Ip_Addr = rcvData.Ip;
                Console.WriteLine("IP:" + uId.get_Ip_Addr());

                //getString___From rcvData.Buffer -> <n ~ nRecv>
                uId.Json = Encoding.UTF8.GetString(rcvData.Buffer, 1, nRecv);
                

                Console.WriteLine(uId.Json);

                unsafe
                {
                    fixed (byte* _return_to_client = return_to_client)
                    {
                        Form.chech_From(uId, _return_to_client);
                    }
                }
                //보낼 데이터
                //for (int i = 0; i < return_to_client.Length; i++)
                //{
                //    Console.WriteLine(return_to_client[i]);
                //}
                //byte[] sendBytes = Encoding.UTF8.GetBytes("hi");
                rcvData.Socket.BeginSend(return_to_client, 0, return_to_client.Length,
                SocketFlags.None, asyncSendCallback, rcvData.Socket);
            }
            catch (Exception)
            {

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
            catch (Exception)
            {

            }
            finally
            {

            }
        }
    }
}