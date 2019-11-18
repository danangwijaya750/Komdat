using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        MySqlConnection con;
        MySqlCommand com;

        public Form1()
        {
            InitializeComponent();
        }
        string rxString="";
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                con = new MySqlConnection("server=127.0.0.1;database=db_komdat;uid=root;pwd=passwd");
                con.Open();
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void showData(object sender,EventArgs e) {
            richTextBox1.AppendText(rxString);
            int wordCount=2;
            String[] separator={" = "};
            String[] splited = rxString.Split(separator, wordCount, StringSplitOptions.RemoveEmptyEntries);
            if (splited != null&&splited.Length==2)
            {
                string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string query = "insert into tb_data_adc(data,waktu) values ("+splited[1]+",'"+dt+"')";
                com = new MySqlCommand(query, con);
                com.ExecuteNonQuery();
                chart1.Series["Data ADC"].Points.AddXY(splited[0], splited[1]);
            }
        }
      

        private void ReciveHandler(object sender, SerialDataReceivedEventArgs e) {
            try
            {
                rxString = serialPort1.ReadLine();
                this.Invoke(new EventHandler(showData));
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
             serialPort1.Close();
             con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             if (!serialPort1.IsOpen)
             {
                 try
                 {
                     serialPort1.PortName = "COM4";
                     serialPort1.DataReceived += new SerialDataReceivedEventHandler(ReciveHandler);
                     serialPort1.Open();
                     button1.Text = "Close Port";
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine(ex.Message);
                     MessageBox.Show(ex.Message);
                 }
             }
             else {
                 serialPort1.Close();
                 button1.Text = "Open Port";
             }
        }

        private void button2_Click(object sender, EventArgs e)
        {
             this.Close();
        }
    }
}
