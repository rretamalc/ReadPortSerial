using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadPortSerial
{
    public partial class Form1 : Form
    {
        SerialPort port = new SerialPort();

        public Form1()
        {
            InitializeComponent();
            if (!OpenSerialPort())
            {
                textBox2.Text = "Desconectado";
            }

        }

        private bool OpenSerialPort()
        {

            try
            {
                port.PortName = "COM3";
                port.BaudRate = 9600;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Handshake = Handshake.None;
                port.ReadTimeout = 1000;
                port.Open();
                port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int dataLength = port.BytesToRead;
            byte[] data = new byte[dataLength];
            int lengthDataRead = port.Read(data, 0, dataLength);
            if (lengthDataRead == 0)
                return;
            if (lengthDataRead == 3)
            {
                ProcessData(data);

            }
        }

        void ProcessData(byte[] receivedata)
        {

            string dataFromSerial = Encoding.ASCII.GetString(receivedata);
            dataFromSerial = dataFromSerial.Substring(0, dataFromSerial.Length);
            if (!dataFromSerial.Contains("?"))
            {
                bool isAllZero = dataFromSerial.All(c => c == '\0');
                if (dataFromSerial.Length > 0 && !isAllZero)
                {

                    Invoke((MethodInvoker)delegate
                    {
                        textBox1.Text = dataFromSerial.ToString();
                    });
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();
                }
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            port.Close();
        }
    }
}
