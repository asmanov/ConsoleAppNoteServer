using Newtonsoft.Json;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConsoleAppNoteServer.Model
{
    public class AsyncServer
    {
        private int flag = 1;
        private readonly IPEndPoint endP;
        private Socket socket;

        public AsyncServer(string strAddr, int port)
        {
            endP = new IPEndPoint(IPAddress.Parse(strAddr), port);
        }
        public AsyncServer()
        {
            endP = new IPEndPoint(IPAddress.Any, 0);
        }

        void MyAcceptCallbackFunction(IAsyncResult ia)
        {
            Socket socket = (Socket)ia.AsyncState;
            Socket ns = socket.EndAccept(ia);
            Console.WriteLine(ns.RemoteEndPoint.ToString());
            //
            NoteResponse response = new NoteResponse();
            if (flag==1)
            {
                using (NoteDbContext context = new NoteDbContext())
                {

                    foreach (Note item in context.Notes)
                    {
                        response.Notes.Add(item);
                    }
                }
                var temp = System.Text.Json.JsonSerializer.Serialize<NoteResponse>(response);
                byte[] sendBufer = Encoding.UTF8.GetBytes(temp);
                ns.BeginSend(sendBufer, 0, sendBufer.Length, SocketFlags.None, new AsyncCallback(MySendCallbackFunction), ns);

            }
            else
            {
                byte[] data = new byte[256];
                int bytes = ns.Receive(data);
                var strAns = Encoding.Unicode.GetString(data, 0, bytes);
                NoteResponse response1 = new NoteResponse();
                Console.WriteLine(strAns);
                response1 = JsonConvert.DeserializeObject<NoteResponse>(strAns);
                using (NoteDbContext context = new NoteDbContext())
                {
                    context.Notes.Add(response1.Notes.LastOrDefault());
                    context.SaveChanges();
                }
                //var temp = Encoding.Unicode.GetString(strAns);
                //Console.WriteLine(temp);
                //byte[] receiveBuffer = new byte[1024];
                //ns.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(MyReceiveCallbackFunction), ns);
            }
            //
            flag++;
            socket.BeginAccept(new AsyncCallback(MyAcceptCallbackFunction), socket);
        }

        //void MyReceiveCallbackFunction(IAsyncResult ia)
        //{
        //    try
        //    {
        //        Socket ns = (Socket)ia.AsyncState;
        //        int n = ((Socket)ia.AsyncState).EndReceive(ia);
        //        //if (n > 0)
        //        //{
        //        byte[] bytes = new byte[n];
                
        //        NoteResponse response = new NoteResponse();
        //        var temp = Encoding.Unicode.GetString(bytes);
        //        Console.WriteLine(temp);
        //        //response = JsonSerializer.Deserialize<NoteResponse>(temp);
        //        response = JsonConvert.DeserializeObject<NoteResponse>(temp);
        //        using(NoteDbContext context = new NoteDbContext())
        //        {
        //            context.Notes.Add(response.Notes.LastOrDefault());
        //            context.SaveChanges();
        //        }
        //        //}
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
            
            
        //}

        void MySendCallbackFunction(IAsyncResult ia)
        {
            Socket ns = (Socket)ia.AsyncState;
            int n = ((Socket)ia.AsyncState).EndSend(ia);

            ns.Shutdown(SocketShutdown.Send);
            ns.Close();
        }

        public void StartServer()
        {
            if (socket != null) return;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(endP);
            socket.Listen(10);
            Console.WriteLine(socket.LocalEndPoint);
            socket.BeginAccept(new AsyncCallback(MyAcceptCallbackFunction), socket);

        }
    }
}
