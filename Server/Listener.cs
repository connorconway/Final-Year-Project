using System;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class Listener
    {
        public  event    ConnectionEvent userAdded;
        private readonly TcpListener     listener;
        private readonly bool[]          usedUserID;

        public Listener(int port)
        {
            usedUserID = new bool[Properties.Settings.Default.MaxNumberOfClients];
            listener   = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            ListenForNewClient();
        }

        public void Stop()
        {
            listener.Stop();
        }

        private void ListenForNewClient()
        {
            listener.BeginAcceptTcpClient(AcceptClient, null);
        }

        private void AcceptClient(IAsyncResult ar)
        {
            TcpClient client = listener.EndAcceptTcpClient(ar);
            int       id     = -1;

            for (byte i = 0; i < usedUserID.Length; i++)
            {
                if (usedUserID[i])
                    continue;
                id = i;
                break;
            }

            if (id == -1)
            {
                Console.WriteLine("Client {0}:  Cannot connect", client.Client.RemoteEndPoint);
                return;
            }

            usedUserID[id]   = true;
            Client newClient = new Client(client, (byte)id);

            newClient.UserDisconnected += client_UserDisconnected;

            if (userAdded != null)
                userAdded(this, newClient);

            ListenForNewClient();
        }

        void client_UserDisconnected(object sender, Client user)
        {
            usedUserID[user.ID] = false;
        }
    }
}