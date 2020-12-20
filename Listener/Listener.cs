using System;
using System.Net;
using System.Net.Sockets;
using ClientClassNamespace;
using System.Collections.Generic;
using System.Threading;

namespace ListenerNamespace
{
    public class Listener
    {
        private TcpListener _server;
        private bool _isListening;
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
            _isListening = true;
            Thread thread = new Thread(()=>{
                while (_isListening)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    AddNewConnection(client);
                }
            });
            thread.Start();
            
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
            Console.WriteLine(((IPEndPoint)connection.Client.RemoteEndPoint).Address.ToString());
            Console.WriteLine(connection.Client.RemoteEndPoint.ToString());
        }

        public void Stop(){
            StopWaitingForConnections();
            foreach (var user in _connectedUsers.Values){
                user.Disconnect();
            }
            _server.Stop();
        }
        private void StopWaitingForConnections(){
            _isListening = false;
        }

    }
}