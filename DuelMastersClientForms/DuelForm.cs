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
            XmlSerializerNamespaces ns = new();
            ns.Add("", "");
            new XmlSerializer(typeof(InterfaceDataWrapper)).Serialize(stream, wrapper, ns);
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
                try
                {
                    NetworkStream stream = _tcpClient.GetStream();
                    MemoryStream ms = new();
                    byte[] buffer = new byte[1024];
                    int numBytesRead;

                    bool process = false;
                    while (stream.DataAvailable)
                    {
                        process = true;
                        numBytesRead = stream.Read(buffer);
                        ms.Write(buffer, 0, numBytesRead);
                    }
                    if (process)
                    {
                        ProcessMessage(ms);
                    }
                }
                catch (IOException ex)
                {
                    WriteNewLine(ex.Message);
                    break;
                }
            }
            WriteNewLine("You have been disconnected from the server.");
        }

        private void ProcessMessage(MemoryStream stream)
        {
            InterfaceDataWrapper wrapper;
            try
            {
                stream.Position = 0;
                wrapper = (InterfaceDataWrapper)new XmlSerializer(typeof(InterfaceDataWrapper)).Deserialize(stream);
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
            else if (wrapper.Events != null)
            {
                foreach (EventWrapper eventWrapper in wrapper.Events)
                {
                    ProcessEvent(eventWrapper);
                }
            }
            //TODO: Choice
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
            else if (other.PlayerWrapper != null)
            {
                if (other.PlayerWrapper.Player != null)
                {
                    _player = new Player(other.PlayerWrapper.Player.ID, other.PlayerWrapper.Player.Name);
                    WriteNewLine($"You have been connected to the server as {_player.Name}.");
                }
                if (other.PlayerWrapper.Opponent != null)
                {
                    _opponent = new Player(other.PlayerWrapper.Opponent.ID, other.PlayerWrapper.Opponent.Name);
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
            else if (wrapper.DeckTopCardToShieldEvent != null)
            {
                WriteNewLine($"{GetPlayer(wrapper.DeckTopCardToShieldEvent.PlayerID).Name} put the top card of their deck to their shields face down.");
            }
            else if (wrapper.DrawCardEvent != null)
            {
                WriteNewLine($"{GetPlayer(wrapper.DrawCardEvent.PlayerID).Name} drew a card.");
            }
            else if (wrapper.TurnStartEvent != null)
            {
                WriteNewLine($"{GetPlayer(wrapper.TurnStartEvent.PlayerID).Name} started turn {wrapper.TurnStartEvent.Number}.");
            }
            //TODO: Add more events
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
