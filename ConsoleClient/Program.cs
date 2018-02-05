using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProtoBuf;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections.TCP;
using System.Timers;



namespace ConsoleClient
{
    class Program
    {
        public static String response { get; set; }

        static void Main(string[] args)
        {
            SendReceiveOptions options = new SendReceiveOptions<ProtobufSerializer>();
            ConnectionInfo conn = new ConnectionInfo("127.0.0.1", 7777);
            TCPConnection socket = TCPConnection.GetConnection(conn, options);

            //response = socket.SendReceiveObject<string>("RequestLogin", "Response", int.MaxValue);
            socket.SendObject("RequestLogin");

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("RequestLoginData", (packetHeader, connection, input) =>
            {
                /*Console.WriteLine("Please enter your username: ");
                String login = Console.ReadLine();
                Console.WriteLine("Please enter your password: ");
                String passw = Console.ReadLine();*/

                Credentials cred = new Credentials("domis045", "admin");

                connection.SendObject("LoginData", cred);
                /* connection.SendObject("LoginData", "No");
                 Console.WriteLine(connection);
                 //connection.Dispose();*/
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("DisconnectMessage", (packetHeader, connection, input) =>
            {
                Console.WriteLine("Your connection has been dropped by the server!");
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("ConnectionMessage", (packetHeader, connection, input) =>
            {
                Console.WriteLine("You have connected sucesfully!");
            });

            Console.ReadLine();

           // Credentials _cred = connection.SendReceiveObject<Credentials>("RequestLoginData", "LoginData", int.MaxValue);
        }

        [ProtoContract]
        public class Credentials
        {
            public Credentials(string Username, string Password)
            {
                this.Username = Username;
                this.Password = Password;
            }

            public Credentials()
            {

            }

            [ProtoMember(1)]
            public string Username { get; set; }

            [ProtoMember(2)]
            public string Password { get; set; }

        }
    }
}
