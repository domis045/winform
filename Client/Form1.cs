using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkCommsDotNet.DPSBase;
using NetworkCommsDotNet.Connections.TCP;
using ProtoBuf;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {

        public Form1()
        {

            InitializeComponent();

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("RequestLoginData", (packetHeader, connection, input) =>
            {

                Credentials cred = new Credentials(textBox1.Text, textBox2.Text);

                connection.SendObject("LoginData", cred);

            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("DisconnectMessage", (packetHeader, connection, input) =>
            {
                MessageBox.Show("Your connection has been dropped by the server!");
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("ConnectionMessage", (packetHeader, connection, input) =>
            {
                MessageBox.Show("You have connected sucesfully!");
            });

            NetworkComms.AppendGlobalIncomingPacketHandler<int>("LoginApproved", (packetHeader, connection, input) =>
            {
                test();
                Invoke(new Action(() => Close())); 

            });

            NetworkComms.AppendGlobalIncomingPacketHandler<String>("Message", IncMessage);
            NetworkComms.AppendGlobalIncomingPacketHandler<String>("UsersCount", UsersCount);
            
        }

        private void UsersCount(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            //label3.Text = "Online users: " + incomingObject;
            //listBox1.Items.Add("Online users: " + incomingObject);
        }

        private void IncMessage(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            MessageBox.Show(incomingObject);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Network.socket.SendObject("RequestLogin");
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

        private void button2_Click(object sender, EventArgs e)
        {
            Network.socket.SendObject("RequestUsersCount");
            //this.Controls.Remove(button2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
            
        }

        private void test()
        {
            Program.OpenWindow();

           /* Thread thred = new Thread(() => {
                MessageBox.Show("test");
                Form2 form2 = new Form2();
                form2.Show();
               // this.Hide();
            
            });

            thred.Start();
            while (true) {
                Thread.Sleep(100);
                // mine bitcoin
            }*/

        }
    }
}

