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
using System.Timers;

namespace ConsoleApp1
{
    class Server
    {
        // Static user data for tests for now.
        public static Dictionary<string, string> _users = new Dictionary<string, string>();

        //Proper users dictionary and a main class
        public static Dictionary<string, Client> Users = new Dictionary<string, Client>();

        // Todo use a enum with a switch to generate messages to make it less "static"
        public enum Days { Greeting, Disconnected };


        static void Main(string[] args)
        {
            // Initialization of default global packet handlers TODO: reasign everything there
            Initialize();

            // Adding temp data.
            _users.Add("domis045", "admin");
            _users.Add("test", "test");

           NetworkComms.AppendGlobalIncomingPacketHandler<int>("RequestLogin", (packetHeader, connection, input) =>
            {
                string netID = connection.ConnectionInfo.NetworkIdentifier;
                Client temp;

                Credentials _cred = connection.SendReceiveObject<Credentials>("RequestLoginData", "LoginData", int.MaxValue);
                if(_users.ContainsKey(_cred.Username) && _users.ContainsValue(_cred.Password))
                {
                    connection.SendObject("Message", "You have connected sucesfully! Welcome: " + _cred.Username);

                    Users.TryGetValue(connection.ConnectionInfo.NetworkIdentifier, out temp);
                    temp.SetConnected();
                    UpdateUser(temp, netID);

                    // Update client's UI state to the app iteself
                    connection.SendObject("LoginApproved");
                }
                else
                {
                    connection.SendObject("Message","Your connection has been dropped by the server!");

                    Users.TryGetValue(connection.ConnectionInfo.NetworkIdentifier, out temp);
                    temp.Disconnect();
                    UpdateUser(temp, netID);

                    //connection.CloseConnection(true);
                }
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("RequestNewsData", (packetHeader, connection, input) => 
            {
                connection.SendObject("NewsData", "Lorem ipsum");
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("RequestUsersCount", (packetHeader, connection, input) =>
            {
                string netID = connection.ConnectionInfo.NetworkIdentifier;
                Client temp;
                int online = 0;

                Users.TryGetValue(netID, out temp);

                if (temp.ConnectionState())
                {

                    foreach(Client key in Users.Values)
                    {
                        if (key.ConnectionState() == true)
                            online++;
                    }

                    connection.SendObject("UsersCount", online.ToString());
                }
                else
                    connection.SendObject("Message", "You don't have access!");
            });

            #region Listener
            // Any ip with 7777 port
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 7777));

            //Kokie ip ir portai available

            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            Console.WriteLine("\n Press any key to close the server.");
            Console.ReadLine();

            NetworkComms.Shutdown();
            #endregion
        }

        public static void UpdateUser(Client data, string netID)
        {
            Users[netID] = data;
        }

        public static void Initialize()
        {
            // Connection related notifications if debug enabled
            NetworkComms.AppendGlobalConnectionEstablishHandler(ClientConnected);
            NetworkComms.AppendGlobalConnectionCloseHandler(ClientDisconnected);
        }

        static void ClientConnected(Connection connection)
        {
            string netID = connection.ConnectionInfo.NetworkIdentifier;
            Console.WriteLine("A new client connected - " + connection.ToString());
            Users.Add(netID, new Client(netID));

        }

        static void ClientDisconnected(Connection connection)
        {
            string netID = connection.ConnectionInfo.NetworkIdentifier;
            Console.WriteLine("A client has disconnected - " + connection.ToString());
            Users.Remove(netID);
        }

        #region Classes
        public class Client
        {
            public Client(string networkID)
            {
                Identify(networkID);
            }

            public void SetConnected()
            {
                connected = true;
            }

            public void Disconnect()
            {
                connected = false;
            }

            public bool ConnectionState()
            {
                return connected;
            }

            public void Identify(string networkID)
            {
                networkIndentifier = networkID;
            }

            public string GetNetworkID()
            {
                return networkIndentifier;
            }

            private bool connected;
            private string networkIndentifier;
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
    #endregion
}
