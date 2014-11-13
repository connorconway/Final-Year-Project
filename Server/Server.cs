using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server
{
    public class Server
    {
        private readonly MemoryStream writeStream;
        private readonly BinaryWriter writer;
        private          BinaryReader reader;
        private readonly Client[]     clients;
        private          Listener     listener          { get; set; }
        private          int          connectedClients;

        public Server(int port)
        {
            MemoryStream readStream = new MemoryStream();
                         clients    = new Client[Properties.NetworkSettings.Default.MaxNumberOfClients];
                         listener   = new Listener(port);

            listener.userAdded += UserConnected;
            listener.Start();

            writeStream = new MemoryStream();
            reader      = new BinaryReader(readStream);
            writer      = new BinaryWriter(writeStream);
        }

        private void UserConnected(object sender, Client user)
        {
            connectedClients++;
            
            writeStream.Position = 0;
            writer.Write(Properties.NetworkSettings.Default.NewPlayerByteProtocol);
            writer.Write(user.clientID);
            writer.Write(user.clientIP);

            SendData(GetDataFromMemoryStream(writeStream), user);
            
            user.DataReceived     += DataReceived;
            user.UserDisconnected += UserDisconnected;

            clients[user.clientID] = user;

            PrintInformation(user, "Connected");
        }

        private void UserDisconnected(object sender, Client user)
        {
            connectedClients--;

            writeStream.Position = 0;
            writer.Write(Properties.NetworkSettings.Default.DisconnectedPlayerByteProtocol);
            writer.Write(user.clientID);
            writer.Write(user.clientIP);

            SendData(GetDataFromMemoryStream(writeStream), user);
            
            clients[user.clientID] = null;

            PrintInformation(user, "Disconnected");
        }

        private void PrintInformation(Client user, string action)
        {
            Console.WriteLine("{0} {1} \tConnected Clients: {2}", user, action, connectedClients);

            foreach (Client client in clients.Where(client => client != null))
            {
                Console.WriteLine("\t\t\t\t {0}", client);
            }
        }

        private void DataReceived(Client sender, byte[] data)
        {
            writeStream.Position = 0;
            writer.Write(sender.clientID);
            writer.Write(sender.clientIP);
            data = CombineData(data, writeStream);

            SendData(data, sender);
        }

        private static byte[] GetDataFromMemoryStream(MemoryStream ms)
        {
            byte[] result;

            lock (ms)
            {
                int bytesWritten = (int)ms.Position;
                result = new byte[bytesWritten];
                ms.Position = 0;
                ms.Read(result, 0, bytesWritten);
            }

            return result;
        }

        private static byte[] CombineData(IList<byte> data, MemoryStream ms)
        {
            byte[] result       = GetDataFromMemoryStream(ms);
            byte[] combinedData = new byte[data.Count + result.Length];

            for (int i = 0; i < data.Count; i++)
                combinedData[i] = data[i];

            for (int j = data.Count; j < data.Count + result.Length; j++)
                combinedData[j] = result[j - data.Count];

            return combinedData;
        }

        private void SendData(byte[] data, Client sender)
        {
            foreach (Client c in clients.Where(c => c != null && c != sender))
                c.SendData(data);

            writeStream.Position = 0;
        }
    }
}