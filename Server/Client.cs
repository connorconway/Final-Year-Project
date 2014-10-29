using System;
using System.Net.Sockets;

namespace Server
{
    public class Client
    {
        public  event    DataReceivedEvent DataReceived;
        public  event    ConnectionEvent   UserDisconnected;
        private readonly TcpClient         client;
        private readonly byte[]            readBuffer;
        public  readonly string            IP;
        public  readonly byte              ID;
        private          bool              connected;

        public Client(TcpClient client, byte ID)
        {
            readBuffer     = new byte[Properties.Settings.Default.ReadBufferSize];
            this.ID        = ID;
            this.client    = client;
            IP             = client.Client.RemoteEndPoint.ToString();
            client.NoDelay = true;

            StartListening();
            connected = true;
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

        private void StartListening()
        {
            client.GetStream().BeginRead(readBuffer, 0, Properties.Settings.Default.ReadBufferSize, StreamReceived, null);
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

            StartListening();

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
                Console.WriteLine("Client {0}:  {1}", IP, e);
            }
        }

        public void SendMemoryStream(System.IO.MemoryStream ms)
        {
            lock (ms)
            {
                var bytesWritten = (int)ms.Position;
                var result       = new byte[bytesWritten];
                ms.Position      = 0;

                ms.Read(result, 0, bytesWritten);
                SendData(result);
            }
        }

        public override string ToString()
        {
            return IP;
        }
    }
}