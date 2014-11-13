using System;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    public class Client : IClient
    {
        public  event    DataReceivedEvent DataReceived;
        public  event    ConnectionEvent   UserDisconnected;
        private readonly TcpClient         client;
        private readonly byte[]            readBuffer;
        public  readonly string            clientIP;
        public  readonly byte              clientID;
        private          bool              connected;

        public Client(TcpClient client, byte clientID)
        {
            readBuffer     = new byte[2048];
            this.clientID  = clientID;
            this.client    = client;
            clientIP       = client.Client.RemoteEndPoint.ToString();
            client.NoDelay = true;

            ListenForEvents();
            connected = true;
        }

        private void ListenForEvents()
        {
            client.GetStream().BeginRead(readBuffer, 0, 2048, StreamReceived, null);
        }

        private void StreamReceived(IAsyncResult ar)
        {
            int bytesRead = 0;
            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(ar);
                }
            }
            catch { }

            if (bytesRead == 0)
            {
                Disconnect();
                return;
            }

            var data = new byte[bytesRead];

            for (int i = 0; i < bytesRead; i++)
                data[i] = readBuffer[i];

            ListenForEvents();

            if (DataReceived != null)
                DataReceived(this, data);
        }

        public void SendData(byte[] b)
        {
            try
            {
                lock (client.GetStream())
                {
                    client.GetStream().BeginWrite(b, 0, b.Length, null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client {0}:  {1}", clientIP, e);
            }
        }

        public void SendMemoryStream(MemoryStream ms)
        {
            lock (ms)
            {
                var bytesWritten = (int) ms.Position;
                var result       = new byte[bytesWritten];
                ms.Position      = 0;

                ms.Read(result, 0, bytesWritten);
                SendData(result);
            }
        }

        private void Disconnect()
        {
            if (!connected)
                return;
            connected = false;

            client.Close();

            if (UserDisconnected != null)
                UserDisconnected(this, this);
        }

        public override string ToString()
        {
            return clientIP;
        }
    }
}