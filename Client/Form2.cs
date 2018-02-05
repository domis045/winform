using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.DPSBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();

            // NetworkComms.AppendGlobalIncomingPacketHandler<String>("UsersCount", UsersCount);
            NetworkComms.AppendGlobalIncomingPacketHandler<String>("UsersCount", UsersCount);
            NetworkComms.AppendGlobalIncomingPacketHandler<String>("NewsData", NewsData);
        }

        private void NewsData(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            //richTextBox1.Text = incomingObject.ToString();
            richTextBox1.Invoke(new Action(() => richTextBox1.Text = incomingObject));
            //MessageBox.Show(incomingObject);
        }

        private void UsersCount(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            MessageBox.Show(incomingObject);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Network.socket.SendObject("RequestUsersCount");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            /*Control c = new Control("aa", 100, 100, 200, 200);
            c.BackColor = Color.AliceBlue;
            tableLayoutPanel1.Controls.Add(c);
            tableLayoutPanel1.Controls.Add(c);
            tableLayoutPanel1.Controls.Add(c);
            tableLayoutPanel1.Controls.Add(c);
            tableLayoutPanel1.Controls.Add(c);
            tableLayoutPanel1.Controls.Add(c);*/
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Network.socket.SendObject("RequestNewsData");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
