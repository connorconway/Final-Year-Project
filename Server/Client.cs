﻿using System;
using System.Net.Sockets;

namespace Server
{
    public class Client
    {
        private readonly TcpClient client;
        private readonly byte[] readBuffer;

        public event ConnectionEvent UserDisconnected;
        public event DataReceivedEvent DataReceived;

        public readonly byte id;
        public readonly string IP;

        bool connected;

        public Client(TcpClient client, byte id)
        {
            readBuffer = new byte[Properties.Settings.Default.ReadBufferSize];
            this.id = id;
            this.client = client;
            IP = client.Client.RemoteEndPoint.ToString();
            client.NoDelay = true;

            StartListening();
            connected = true;
        }

        public Client(string ip, int port)
        {
            readBuffer = new byte[Properties.Settings.Default.ReadBufferSize];
            id = byte.MaxValue;
            client = new TcpClient { NoDelay = true };
            client.Connect(ip, port);

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
            catch (Exception e)
            {
                
            }

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
                var result = new byte[bytesWritten];

                ms.Position = 0;
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
