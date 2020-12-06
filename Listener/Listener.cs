using System;
using System.Net;
using System.Net.Sockets;

namespace ListenerNamespace
{
    public class Listener
    {
    private TcpListener _server;
        void Start(){
            _server = null;// Set the TcpListener on port 13000.
            int port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            _server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            _server.Start();
        }
        private void StartWaitingForConnections(){
            // Enter the listening loop.
            
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = _server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    //clientcreation!!!
                }
        }
        private void AddNewConnection(){

        }
    }
}