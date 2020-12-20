using System;
using System.Net.Sockets;
using System.Threading;

namespace ClientClassNamespace
{
    public class ClientClass
    {
        private readonly string _serverAddress;
        private readonly int _port;
        private NetworkStream _stream;
        private Thread _listeningThread;
        private bool _isListening;
        private TcpClient _client;

        public ClientClass(string serverAddress, int port)
        {
            _serverAddress = serverAddress;
            _port = port;
        }

        public void Connect()
        {
            _client = new TcpClient(_serverAddress, _port);
            _stream = _client.GetStream();
            StartListening();
        }

        public string SendMessage(string message)
        {
            if (_stream!=null)//_isListening )
            {
                try
                {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);
                    return "sent was successful";
                }
                catch
                {
                    return "failed to connect";
                }
            }
            else{
                    return "not connected";
            }
        }

        public event Action<string> OnMessageReceived;

        private void StartListening()
        {
            _isListening = true;

            _listeningThread = new Thread(() => 
            {
                while(_isListening)
                {
                    byte[] data = new byte[256];
                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    OnMessageReceived?.Invoke(message);
                }
            });

            _listeningThread.Start();
        }

        private void StopListen()
        {
            _isListening = false;
        }

        public void Disconnect()
        {
            StopListen();
            _stream.Close();
            //_listeningThread.Abort();
            _client.Close();
        }
    }
}