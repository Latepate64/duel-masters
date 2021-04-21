using DuelMastersInterfaceModels;
using DuelMastersInterfaceModels.Events;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DuelMastersClientForms
{
    public partial class DuelForm : Form
    {
        public DuelForm()
        {
            InitializeComponent();
            FormClosed += Form1_FormClosed;
            OutputTextBox.AppendText($"Welcome! Type connect HOST PORT and click send button. (eg. connect {DefaultHost} {DefaultPort})");
            InputTextBox.AppendText($"connect {DefaultHost} {DefaultPort}");
        }

        private const int DefaultPort = 80;
        private const string DefaultHost = "127.0.0.1";

        private delegate void SafeCallDelegate(string text);
        private TcpClient _tcpClient;
        private Player _player;
        private Player _opponent;

        private Player GetPlayer(int id)
        {
            if (_player.ID == id)
            {
                return _player;
            }
            else if (_opponent.ID == id)
            {
                return _opponent;
            }
            else
            {
                throw new InvalidOperationException($"Player with id {id} not found.");
            }
        }

        private static FlowLayoutPanel CreateCreature()
        {
            const int Width = 222;
            const int Height = 307;
            double scale = 0.395;
            Label cost = new() { Text = "2" };
            Label name = new() { Text = "Burning Mane" };
            Label race = new() { Text = "Beast Folk" };
            Label power = new() { Text = "2000" };
            FlowLayoutPanel card = new() { BackColor = Color.Green, Height = (int)(scale * Height), Width = (int)(scale * Width) };
            card.Controls.Add(cost);
            card.Controls.Add(name);
            card.Controls.Add(race);
            card.Controls.Add(power);
            return card;
        }

        private static byte[] Serialize(InterfaceDataWrapper wrapper)
        {
            MemoryStream stream = new();
            new XmlSerializer(typeof(InterfaceDataWrapper)).Serialize(stream, wrapper);
            return stream.GetBuffer();
        }

        private void TryDisconnect()
        {
            _tcpClient.GetStream().Close();
            _tcpClient.Close();
        }

        private void ReceiveMessages()
        {
            while (_tcpClient.Connected)
            {
                byte[] buffer = new byte[1024];
                int byteCount;
                try
                {
                    NetworkStream stream = _tcpClient.GetStream();
                    //stream.ReadTimeout = 1000;
                    byteCount = stream.Read(buffer);
                    //stream.Flush();
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

        private void ProcessMessage(MemoryStream stream)
        {
            InterfaceDataWrapper wrapper;
            try
            {
                var whatisthis = Encoding.ASCII.GetString(stream.ToArray());
                WriteNewLine(whatisthis);

                XmlReader reader = XmlReader.Create(stream);
                wrapper = (InterfaceDataWrapper)new XmlSerializer(typeof(InterfaceDataWrapper)).Deserialize(reader);

                //wrapper = (InterfaceDataWrapper)new XmlSerializer(typeof(InterfaceDataWrapper)).Deserialize(stream);
            }
            catch (Exception e)
            {
                WriteNewLine($"Could not process data received from server: {e.Message} {e.StackTrace}");
                return;
            }
            if (wrapper.Other != null)
            {
                ProcessWrapperOther(wrapper.Other);
            }
            else if (wrapper.Event != null)
            {
                ProcessEvent(wrapper.Event);
            }
            else
            {
                WriteNewLine("Data returned from server contained no valid data.");
            }
        }

        private void ProcessWrapperOther(OtherWrapper other)
        {
            if (other.ChatMessage != null)
            {
                WriteNewLine(other.ChatMessage);
            }
            else if (other.PlayerInfo != null)
            {
                if (other.PlayerInfo.Local)
                {
                    _player = new Player(other.PlayerInfo.ID, other.PlayerInfo.Name);
                    WriteNewLine($"You have been connected to the server as {_player.Name}.");
                }
                else
                {
                    _opponent = new Player(other.PlayerInfo.ID, other.PlayerInfo.Name);
                    WriteNewLine($"Your opponent has connected to the server as {_opponent.Name}.");
                }
            }
            else
            {
                WriteNewLine("Server returned invalid other wrapper.");
            }
        }

        private void ProcessEvent(EventWrapper wrapper)
        {
            if (wrapper.ShuffleDeckEvent != null)
            {
                WriteNewLine($"{GetPlayer(wrapper.ShuffleDeckEvent.PlayerID).Name} shuffled their deck.");
            }
            else
            {
                WriteNewLine("Server returned invalid event wrapper.");
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

        #region Events
        private void CreateHandCardButton_Click(object sender, System.EventArgs e)
        {
            FlowLayoutPanel card = CreateCreature();
            PlayerHand.Controls.Add(card);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_tcpClient != null)
            {
                TryDisconnect();
            }
        }
        
        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tcpClient == null && InputTextBox.Text.StartsWith("connect "))
                {
                    //TODO: Consider host and port given as arguments
                    ConnectToServer(DefaultHost, DefaultPort);
                }
                else if (InputTextBox.Text == "startduel")
                {
                    SendMessage(new() { Other = new() { DuelStartMode = DuelStartMode.First } });
                }
                else
                {
                    SendMessage(new() { Other = new() { ChatMessage = InputTextBox.Text } });
                }
            }
            catch (Exception ex)
            {
                WriteNewLine(ex.Message);
            }
            finally
            {
                InputTextBox.Clear();
            }
        }
        #endregion Events

        private void SendMessage(InterfaceDataWrapper wrapper)
        {
            _ = _tcpClient.GetStream().WriteAsync(Serialize(wrapper));
        }

        private void ConnectToServer(string host, int port)
        {
            IPAddress address = IPAddress.Parse(host);
            WriteNewLine("Connecting to the server...");
            TcpClient client = new();
            Task task = client.ConnectAsync(address, port);
            task.Wait();
            _tcpClient = client;
            Task receive = new(() => ReceiveMessages());
            receive.Start();
        }
    }
}
