using System;
using System.IO;

namespace Server
{
    public class Server
    {
        Listener listener { get; set; }
        readonly Client[] client;
        int connectedClients;

        MemoryStream readStream;
        readonly MemoryStream writeStream;
        BinaryReader reader;
        readonly BinaryWriter writer;

        public Server(int port)
        {
            client = new Client[Properties.Settings.Default.MaxNumberOfClients];

            listener = new Listener(port);
            listener.userAdded += listener_userAdded;
            listener.Start();

            readStream = new MemoryStream();
            writeStream = new MemoryStream();
            reader = new BinaryReader(readStream);
            writer = new BinaryWriter(writeStream);

        }

        private void listener_userAdded(object sender, Client user)
        {
            connectedClients++;

            if (Properties.Settings.Default.SendMessageToClientsWhenAUserIsAdded)
            {
                writeStream.Position = 0;

                //Write in the form {Protocol}{User_ID}{User_IP}
                writer.Write(Properties.Settings.Default.NewPlayerByteProtocol);
                writer.Write(user.id);
                writer.Write(user.IP);

                SendData(GetDataFromMemoryStream(writeStream), user);
            }

            user.DataReceived += user_DataReceived;
            user.UserDisconnected += user_UserDisconnected;

            Console.WriteLine(user + " connected\tConnected Clients:  " + connectedClients + "\n");

            client[user.id] = user;
        }

        private void user_UserDisconnected(object sender, Client user)
        {
            connectedClients--;

            if (Properties.Settings.Default.SendMessageToClientsWhenAUserIsRemoved)
            {
                writeStream.Position = 0;

                //Write in the form {Protocol}{User_ID}{User_IP}
                writer.Write(Properties.Settings.Default.DisconnectedPlayerByteProtocol);
                writer.Write(user.id);
                writer.Write(user.IP);

                SendData(GetDataFromMemoryStream(writeStream), user);
            }

            Console.WriteLine(user + " disconnected\tConnected Clients:  " + connectedClients + "\n");

            client[user.id] = null;
        }

        private void user_DataReceived(Client sender, byte[] data)
        {
            writeStream.Position = 0;

            writer.Write(sender.id);
            writer.Write(sender.IP);
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

        private static byte[] CombineData(byte[] data, MemoryStream ms)
        {
            byte[] result = GetDataFromMemoryStream(ms);
            byte[] combinedData = new byte[data.Length + result.Length];

            for (int i = 0; i < data.Length; i++)
            {
                combinedData[i] = data[i];
            }

            for (int j = data.Length; j < data.Length + result.Length; j++)
            {
                combinedData[j] = result[j - data.Length];
            }

            return combinedData;
        }

        private void SendData(byte[] data, Client sender)
        {
            foreach (Client c in client)
            {
                if (c != null && c != sender)
                {
                    c.SendData(data);
                }
            }

            writeStream.Position = 0;
        }
    }
}
