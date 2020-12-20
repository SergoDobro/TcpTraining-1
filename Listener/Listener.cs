using System;
using System.Net;
using System.Net.Sockets;
using ClientClassNamespace;
using System.Collections.Generic;

namespace ListenerNamespace
{
    public class Listener
    {
        private TcpListener _server;
        private Dictionary<string, ClientClass> _connectedUsers = new Dictionary<string, ClientClass>();
        
        public void Start()
        {
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            _server = new TcpListener(localAddr, port);
            _server.Start();
            StartWaitingForConnections();
        }
        private void StartWaitingForConnections()
        {
            while (true)
            {
                TcpClient client = _server.AcceptTcpClient();
                AddNewConnection(client);
            }
        }
        private void AddNewConnection(TcpClient connection){
            ClientClass user = new ClientClass(connection);
            user.Connect();
            user.OnMessageReceived += (message) =>{
                if (message.Contains("Auth:")){
                    
                    string username = message.Split(':')[1];
                    _connectedUsers.Add(username,user);
                }
                if (message.Contains("MessageTo:")){ //MessageTo:SergoDobro:message
                    string toUser = message.Split(':')[1];
                    string userMessage = message.Split(':')[2];
                    _connectedUsers[toUser].SendMessage(userMessage);
                }
                Console.WriteLine(message);
            };
            _connectedUsers.Add(((IPEndPoint)connection.Client.RemoteEndPoint).Address.ToString(),user);

        }
    }
}