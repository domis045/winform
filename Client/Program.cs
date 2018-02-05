using NetworkCommsDotNet.Connections.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Client
{
    static class Program
    {
        public static void OpenWindow()
        {
            /* Form2 form2 = new Form2();
             form2.Show();*/
            Thread t = new Thread(() => { Application.Run(new Form2()); });
            t.Start();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
           // Engine.socket = TCPConnection.GetConnection(Engine.conn, Engine.options);
        }
    }
}
