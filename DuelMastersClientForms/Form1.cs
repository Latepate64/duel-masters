using DuelMastersInterfaceModels;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DuelMastersClientForms
{
    public partial class Form1 : Form
    {
        const int Port = 80;
        const string IP = "127.0.0.1";

        TcpClient _tcpClient;

        delegate void SafeCallDelegate(string text);

        public Form1()
        {
            InitializeComponent();
            FormClosed += Form1_FormClosed;
            OutputTextBox.AppendText("Welcome!");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_tcpClient != null)
            {
                TryDisconnect();
            }
        }

        private void TryDisconnect()
        {
            _tcpClient.GetStream().Close();
            _tcpClient.Close();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress address = IPAddress.Parse(IP);
                if (_tcpClient == null)
                {
                    _tcpClient = new();
                }
                WriteNewLine("Connecting to the server...");
                Task task = _tcpClient.ConnectAsync(address, Port);
                task.Wait();
                Task receive = new Task(() => ReceiveMessages());
                receive.Start();
            }
            catch (Exception ex)
            {
                WriteNewLine(ex.Message);
            }
        }

        void ReceiveMessages()
        {
            while (_tcpClient.Connected)
            {
                byte[] buffer = new byte[1024];
                int received;
                try
                {
                    received = _tcpClient.GetStream().Read(buffer);
                }
                catch (System.IO.IOException ex)
                {
                    WriteNewLine(ex.Message);
                    break;
                }
                if (received > 0)
                {
                    WriteNewLine(Encoding.UTF8.GetString(buffer, 0, received));
                }
            }
            WriteNewLine("You have been disconnected from the server.");
        }

        private void WriteNewLine(string text)
        {
            if (OutputTextBox.InvokeRequired)
            {
                OutputTextBox.Invoke(new SafeCallDelegate(WriteNewLine), new object[] { text });
            }
            else
            {
                OutputTextBox.AppendText(Environment.NewLine + text);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                InterfaceDataWrapper wrapper = new() { Other = new() { ChatMessage = InputTextBox.Text } };
                byte[] bytes = Serialize(wrapper);

                //byte[] bytes = Encoding.UTF8.GetBytes(InputTextBox.Text);
                ValueTask write = _tcpClient.GetStream().WriteAsync(bytes);
                InputTextBox.Clear();
            }
            catch (Exception ex)
            {
                WriteNewLine(ex.Message);
            }
        }

        private byte[] Serialize(InterfaceDataWrapper wrapper)
        {
            //byte[] buffer = new byte[1024];
            MemoryStream stream = new();

            XmlSerializer xmlSerializer = new(typeof(InterfaceDataWrapper));
            xmlSerializer.Serialize(stream, wrapper);

            return stream.GetBuffer();
            //ValueTask write = _tcpClient.GetStream().WriteAsync(bytes);
        }
    }
}
