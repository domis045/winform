using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Network
    {

        public static SendReceiveOptions options = new SendReceiveOptions<ProtobufSerializer>();
        public static ConnectionInfo conn = new ConnectionInfo("127.0.0.1", 7777);
        public static TCPConnection socket = TCPConnection.GetConnection(conn, options);

    }
}
