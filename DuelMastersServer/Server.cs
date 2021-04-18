using System.Net.Sockets;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace DuelMastersServer
{
    internal class Client
    {
        internal TcpClient TCPClient { get; set; }
        internal string Name { get; set; }
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
                int received;
                try
                {
                    received = client.TCPClient.GetStream().Read(buffer);
                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
                if (received > 0)
                {
                    string msg = $"{client.Name}: {Encoding.ASCII.GetString(buffer, 0, received)}";
                    Console.WriteLine(msg);
                    byte[] bytes = Encoding.ASCII.GetBytes(msg);
                    foreach (Client activeClient in GetActiveClients())
                    {
                        activeClient.TCPClient.GetStream().Write(bytes);
                    }
                }
            }
            Console.WriteLine($"{client.Name} disconnected.");
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
