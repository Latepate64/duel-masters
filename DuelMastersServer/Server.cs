using DuelMastersInterfaceModels;
using DuelMastersInterfaceModels.Events;
using DuelMastersModels;
using System.Net.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using DuelMastersModels.Zones;
using DuelMastersModels.Cards.Creatures;
using DuelMastersModels.Cards;
using DuelMastersModels.Managers;
using DuelMastersInterfaceModels.Cards;

namespace DuelMastersServer
{
    internal class Client
    {
        internal DuelStartMode DuelStartMode { get; set; }
        internal TcpClient TCPClient { get; set; }
        internal IPlayer Player { get; set; }
        internal string Name => Player.Name;

        internal void Send(string msg)
        {
            Send(new InterfaceDataWrapper() { Other = new() { ChatMessage = msg } });
        }

        internal void Send(InterfaceDataWrapper wrapper)
        {
            Send(Server.Serialize(wrapper));
        }

        internal void Send(byte[] bytes)
        {
            TCPClient.GetStream().Write(bytes);
        }
    }

    class Server
    {
        static TcpListener _tcpListener;
        static readonly Client _client1 = new() { Player = new Player { ID = 0, Name = "Player1" } };
        static readonly Client _client2 = new() { Player = new Player { ID = 1, Name = "Player2" } };
        static Duel _duel;

        static void Main(string[] args)
        {
            const int Port = 80;
            const string IP = "127.0.0.1";
            IPAddress address = IPAddress.Parse(IP);
            _tcpListener = new(address, Port);
            _tcpListener.Start();

            Console.WriteLine($"Waiting for {_client1.Name} to connect...");
            _client1.TCPClient = _tcpListener.AcceptTcpClient();
            Console.WriteLine($"{_client1.Name} connected.");
            _client1.Send(new InterfaceDataWrapper { Other = new() { PlayerWrapper = new() { Player = new() { ID = _client1.Player.ID, Name = _client1.Name } } } });
            Task task1 = new(() => ReceiveMessages(_client1));
            task1.Start();

            Console.WriteLine($"Waiting for {_client2.Name} to connect...");
            _client2.TCPClient = _tcpListener.AcceptTcpClient();
            Console.WriteLine($"{_client2.Name} connected.");
            _client2.Send(new InterfaceDataWrapper { Other = new() { PlayerWrapper = new() { Player = new() { ID = _client2.Player.ID, Name = _client2.Name }, Opponent = new() { ID = _client1.Player.ID, Name = _client1.Name } } } });
            _client1.Send(new InterfaceDataWrapper { Other = new() { PlayerWrapper = new() { Opponent = new() { ID = _client2.Player.ID, Name = _client2.Name } } } });
            Task task2 = new(() => ReceiveMessages(_client2));
            task2.Start();
            List<Task> tasks = new()
            {
                task1,
                task2,
            };
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("All clients have disconnected. Press any key to exit.");
            Console.ReadKey();
        }

        static void ReceiveMessages(Client client)
        {
            while (client.TCPClient.Connected)
            {
                byte[] buffer = new byte[1024];
                int byteCount;
                try
                {
                    byteCount = client.TCPClient.GetStream().Read(buffer);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                if (byteCount > 0)
                {
                    ProcessMessage(client, new MemoryStream(buffer, 0, byteCount));
                }
            }
            Console.WriteLine($"{client.Name} disconnected.");
        }

        private static void Broadcast(string msg)
        {
            Console.WriteLine(msg);
            Broadcast(Serialize(new InterfaceDataWrapper() { Other = new() { ChatMessage = msg } }));
        }

        private static void Broadcast(byte[] bytes)
        {
            foreach (Client activeClient in GetActiveClients())
            {
                activeClient.Send(bytes);
            }
        }

        static void ProcessMessage(Client client, MemoryStream stream)
        {
            InterfaceDataWrapper wrapper;
            try
            {
                wrapper = (InterfaceDataWrapper)new XmlSerializer(typeof(InterfaceDataWrapper)).Deserialize(stream);
            }
            catch (Exception e)
            {
                WriteLineAndSendToClient(client, $"Could not process data: {e.Message}");
                return;
            }
            if (wrapper.Other != null)
            {
                ProcessWrapperOther(client, wrapper.Other);
            }
            else
            {
                WriteLineAndSendToClient(client, "Data contained no valid data.");
            }
        }

        private static void ProcessWrapperOther(Client client, OtherWrapper wrapper)
        {
            if (!string.IsNullOrEmpty(wrapper.ChatMessage))
            {
                Broadcast($"{client.Name}: {wrapper.ChatMessage}");
            }
            else if (!string.IsNullOrEmpty(wrapper.ChangeName))
            {
                string oldName = client.Name;
                client.Player.Name = wrapper.ChangeName;
                Broadcast($"{oldName} changed their name to {client.Name}.");
            }
            else if (wrapper.DuelStartMode != client.DuelStartMode)
            {
                client.DuelStartMode = wrapper.DuelStartMode;
                switch (client.DuelStartMode)
                {
                    case DuelStartMode.Wait:
                        Broadcast($"{client.Name} is not ready to start a duel.");
                        break;
                    case DuelStartMode.First:
                        StartDuel();
                        //Broadcast($"{client.Name} is ready to start a duel and would like to go first.");
                        break;
                    case DuelStartMode.Second:
                        Broadcast($"{client.Name} is ready to start a duel and would like to go second.");
                        break;
                    case DuelStartMode.Random:
                        Broadcast($"{client.Name} is ready to start a duel and would like to decide at random who goes first.");
                        break;
                    default:
                        WriteLineAndSendToClient(client, "Invalid DuelStartMode.");
                        break;
                }
            }
            else
            {
                WriteLineAndSendToClient(client, "Data.Other contained no valid data.");
            }
        }

        static void WriteLineAndSendToClient(Client client, string msg)
        {
            Console.WriteLine($"{client.Name} -> {msg}");
            client.Send(msg);
        }

        static Client GetOther(Client client)
        {
            if (client == _client1)
            {
                return _client2;
            }
            else if (client == _client2)
            {
                return _client1;
            }
            else
            {
                throw new InvalidOperationException("Cannot get other client.");
            }
        }

        static IEnumerable<Client> GetActiveClients()
        {
            List<Client> clients = new();
            if (_client1.TCPClient != null && _client1.TCPClient.Connected)
            {
                clients.Add(_client1);
            }
            if (_client2.TCPClient != null && _client2.TCPClient.Connected)
            {
                clients.Add(_client2);
            }
            return clients;
        }

        static void StartDuel()
        {
            EventManager eventManager = new();
            _duel = new Duel
            {
                Player1 = _client1.Player,
                Player2 = _client2.Player,
            };
            _duel.Player1.EventManager = eventManager;
            _duel.Player2.EventManager = eventManager;
            _duel.Player1.Opponent = _duel.Player2;
            _duel.Player2.Opponent = _duel.Player1;
            List<ICard> cards1 = new();
            List<ICard> cards2 = new();
            for (int i = 0; i < 40; ++i)
            {
                cards1.Add(new BurningMane());
                cards2.Add(new BurningMane());
            }
            _duel.Player1.Deck = new Deck(cards1);
            _duel.Player2.Deck = new Deck(cards2);
            _duel.StartingPlayer = _duel.Player1;
            DuelMastersInterfaceModels.Choices.IChoice choice = _duel.Start();

            InterfaceDataWrapper wrapper1 = new() { Events = new() };
            InterfaceDataWrapper wrapper2 = new() { Events = new() };
            while (eventManager.NewEvents.TryDequeue(out DuelEvent duelEvent))
            {
                wrapper1.Events.Add(GetEventWrapper(duelEvent, _client1.Player.ID));
                wrapper2.Events.Add(GetEventWrapper(duelEvent, _client2.Player.ID));
            }
            //TODO: include choice in wrapper
            _client1.Send(wrapper1);
            _client2.Send(wrapper2);
        }

        private static EventWrapper GetEventWrapper(DuelEvent duelEvent, int playerID)
        {
            EventWrapper wrapper = new();
            if (duelEvent is ShuffleDeckEvent shuffle)
            {
                wrapper.ShuffleDeckEvent = shuffle;
            }
            else if (duelEvent is DeckTopCardToShieldEvent shieldEvent)
            {
                wrapper.DeckTopCardToShieldEvent = shieldEvent;
            }
            else if (duelEvent is DrawCardEvent drawCard)
            {
                DrawCardEvent copy = new() { PlayerID = drawCard.PlayerID };
                // Show card information if receiver is the player who drew the card.
                if (drawCard.PlayerID == playerID)
                {
                    copy.Card = drawCard.Card;
                }
                wrapper.DrawCardEvent = copy;
            }
            else if (duelEvent is TurnStartEvent turnStart)
            {
                wrapper.TurnStartEvent = turnStart;
            }
            //TODO: Add more events
            else
            {
                throw new InvalidOperationException($"Unknown duel event: {duelEvent}");
            }
            return wrapper;
        }

        internal static byte[] Serialize(InterfaceDataWrapper wrapper)
        {
            MemoryStream stream = new();
            XmlSerializerNamespaces ns = new();
            ns.Add("", "");
            new XmlSerializer(typeof(InterfaceDataWrapper)).Serialize(stream, wrapper, ns);
            return stream.GetBuffer();
        }
    }
}