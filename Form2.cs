using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox1.Text != " " && textBox2.Text != " ")
                {
                    DirectoryInfo data = new DirectoryInfo("Client_Info");
                    data.Create();
                    var sw = new StreamWriter(@"Client_Info/data_info.txt");
                    sw.WriteLine(textBox1.Text + ":" + textBox2.Text);
                    sw.Close();
                    this.Close();
                    Application.Restart();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка: \n" + ex.Message);
            }
        }
    }
}
