using DuelMastersInterfaceModels;
using DuelMastersInterfaceModels.Events;
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
                int byteCount;
                try
                {
                    byteCount = _tcpClient.GetStream().Read(buffer);
                }
                catch (IOException ex)
                {
                    WriteNewLine(ex.Message);
                    break;
                }
                if (byteCount > 0)
                {
                    ProcessMessage(new MemoryStream(buffer, 0, byteCount));
                }
            }
            WriteNewLine("You have been disconnected from the server.");
        }

        void ProcessMessage(MemoryStream stream)
        {
            InterfaceDataWrapper wrapper;
            try
            {
                wrapper = (InterfaceDataWrapper)new XmlSerializer(typeof(InterfaceDataWrapper)).Deserialize(stream);
            }
            catch (Exception e)
            {
                WriteNewLine($"Could not process data received from server: {e.Message}");
                return;
            }
            //if (wrapper.Other != null)
            //{
            //    ProcessWrapperOther(client, wrapper.Other);
            //}
            if (wrapper.Event != null)
            {
                ProcessEvent(wrapper.Event);
            }
            else
            {
                WriteNewLine("Data returned from server contained no valid data.");
            }
        }

        private void ProcessEvent(EventWrapper wrapper)
        {
            if (wrapper.ShuffleDeckEvent != null)
            {
                WriteNewLine($"{wrapper.ShuffleDeckEvent.PlayerID} shuffled their deck.");
            }
            else
            {
                WriteNewLine("Server did not return event.");
            }
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
                ValueTask write = _tcpClient.GetStream().WriteAsync(bytes);
                InputTextBox.Clear();
            }
            catch (Exception ex)
            {
                WriteNewLine(ex.Message);
            }
        }

        private static byte[] Serialize(InterfaceDataWrapper wrapper)
        {
            MemoryStream stream = new();
            new XmlSerializer(typeof(InterfaceDataWrapper)).Serialize(stream, wrapper);
            return stream.GetBuffer();
        }

        private void DuelButton_Click(object sender, EventArgs e)
        {
            DuelForm duelForm = new();
            duelForm.Show();
        }
    }
}
