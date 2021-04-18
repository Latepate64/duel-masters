using DuelMastersInterfaceModels;
using System.Net.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace DuelMastersServer
{
    internal class Client
    {
        internal DuelStartMode DuelStartMode { get; set; }
        internal TcpClient TCPClient { get; set; }
        internal string Name { get; set; }

        internal void Send(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            TCPClient.GetStream().Write(bytes);
        }
    }

    class Server
    {
        static TcpListener _tcpListener;
        static readonly Client _client1 = new() { Name = "Player1" };
        static readonly Client _client2 = new() { Name = "Player2" };

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
            Task task1 = new(() => ReceiveMessages(_client1));
            task1.Start();
            Console.WriteLine($"Waiting for {_client2.Name} to connect...");
            _client2.TCPClient = _tcpListener.AcceptTcpClient();
            Console.WriteLine($"{_client2.Name} connected.");
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
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            foreach (Client activeClient in GetActiveClients())
            {
                activeClient.TCPClient.GetStream().Write(bytes);
            }
        }

        static void ProcessMessage(Client client, MemoryStream stream)
        {
            XmlSerializer xmlSerializer = new(typeof(InterfaceDataWrapper));
            InterfaceDataWrapper wrapper;
            try
            {
                wrapper = (InterfaceDataWrapper)xmlSerializer.Deserialize(stream);
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
                client.Name = wrapper.ChangeName;
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
                        Broadcast($"{client.Name} is ready to start a duel and would like to go first.");
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
    }
}