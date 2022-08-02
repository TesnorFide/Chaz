using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static private Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Enabled = false;
            textBox2.Enabled = false;

            try
            {
                var sr = new StreamReader(@"Client_Info/data_info.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                string[] connect_info = buffer.Split(':');
                ip = IPAddress.Parse(connect_info[0]);
                port = int.Parse(connect_info[1]);

                checkBox1.ForeColor = Color.Green;
                checkBox1.Text = "Всё отлично!";
                checkBox1.Checked = true;
            }
            catch(Exception ex)
            {
                checkBox1.ForeColor = Color.Red;
                checkBox1.Text = "Ошибка :(";
                checkBox1.Checked = false;
                Form2 form = new Form2();
                form.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Вход";
            FakeTransTextBox richTextBox1 = new FakeTransTextBox();
            richTextBox1.BackColor = Color.Transparent;
            richTextBox1.ScrollBars = ScrollBars.None;
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(th!=null)
                th.Abort();
            if (Client != null)
                Client.Close();
            Close();
        }

        private void checkBox1_MouseHover(object sender, EventArgs e)
        {
            if (checkBox1.ForeColor == Color.Red)
            {
                MessageBox.Show("Что-то пошло не так. Возможно не указаны настройки, либо у Вас нет интернета. Проверьте настройки сети и перезапустите программу.");
            }
        }
        void SendMessage(string message)
        {
            if (message != "" && message != " ")
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                Client.Send(buffer);
            }
        }
        void RecvMessage()
        {
            byte[] buffer = new byte[1024];
            for (int i=0; i<buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            for (; ; )
            {
                try
                {
                    Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    int count = message.IndexOf(";;;5");
                    if (count == -1)
                    {
                        continue;
                    }
                    string Clear_Message = "";
                    for (int i = 0; i < count; i++)
                    {
                        Clear_Message += message[i];
                    }
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = 0;
                    }
                    this.Invoke((MethodInvoker)delegate()
                    {
                        richTextBox1.AppendText(Clear_Message);
                        richTextBox1.SelectionStart = richTextBox1.Text.Length;
                        richTextBox1.SelectionLength = 0;
                        richTextBox1.ScrollToCaret();
                    });
                }
                catch (Exception ex)
                {

                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Вход" && textBox1.Text != "" && textBox1.Text != " ")
            {
                button1.Text = "Отправить";
                textBox2.Enabled = true;
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if(ip!=null)
                {
                    Client.Connect(ip, port);
                    th = new Thread(delegate() { RecvMessage(); });
                    th.Start();
                }
            }
            else if (button1.Text == "Отправить")
            {
                SendMessage("\n"+textBox1.Text+":  "+textBox2.Text+";;;5");
                textBox2.Clear();
                textBox2.Focus();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                SendMessage("\n" + textBox1.Text + ":  " + textBox2.Text + ";;;5");
                textBox2.Clear();
            }
        }

        private void создателиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("(c)2016");
        }

        private void версияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия приложения v. 1.0");
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void задатьСерверToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        private void сменитьФонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                this.BackgroundImage = img;
            }
        }
    }
}
