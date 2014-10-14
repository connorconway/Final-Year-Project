using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        static readonly List<Socket> connectedSockets = new List<Socket>();
        static readonly Dictionary<string, User> users = new Dictionary<string, User>();
        static TcpListener tcpListener;

        public Server(bool UseLoopback, int port)
        {
            IPAddress ipAddress = UseLoopback ? IPAddress.Loopback : LocalIPAddress();
            tcpListener = new TcpListener(ipAddress, port);
            tcpListener.Start();
        }

        public static void Start()
        {
            try
            {
                Console.WriteLine("Listening for connections \n");
                while (true)
                {
                    Socket socket = tcpListener.AcceptSocket();
                    new Thread(SocketMethod).Start(socket);
                    connectedSockets.Add(socket);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Could not listen on port: 4444. Possible port clash");
            }
        }

        public static void Stop()
        {
            tcpListener.Stop();
        }

        private static void SocketMethod(object socketObject)
        {
            Socket socket = socketObject as Socket;
            if (socket == null)
                return;

            receiveData(socket);
        }

        private static void receiveData(Socket socket)
        {
            Console.Write("Client Connected");

            try
            {
                NetworkStream stream = new NetworkStream(socket, true);
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string receivedMessage = reader.ReadLine();

                while (receivedMessage != null)
                {
                    foreach (var so in connectedSockets)
                    {
                        Console.WriteLine(connectedSockets.Count);
                        Console.WriteLine("Sending Message: " + receivedMessage + "\n to " + so.RemoteEndPoint);

                        stream = new NetworkStream(so, true);
                        StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                        writer.Write(receivedMessage);
                        writer.Flush();

                        Console.WriteLine("Message sent to the the client");
                    }

                    receivedMessage = null;
                }        
            }
            catch
            {
                //
            }
        }

        private static void sendData(Socket socket, String username, String message)
        {
            Console.Write(socket.RemoteEndPoint + "(" + username + ") : " + message);

            foreach (Socket so in connectedSockets.Where(so => message != null))
                so.Send(Encoding.UTF8.GetBytes(username + ": " + message));
        }

        private static void connectUser(Socket socket, String username)
        {
            try
            {
                users.Add(socket.RemoteEndPoint.ToString(), new User(username));

                Console.Write("Connection made by: " + socket.RemoteEndPoint + "(" + username + ")");

                foreach (Socket so in connectedSockets)
                {
                    so.Send(Encoding.UTF8.GetBytes(username + " has entered the chat room :)"));
                }
            }
            catch (ArgumentException e)
            {
                Console.Write("Could not connect user");
            }
        }

        private static void disconnectUser(Socket socket)
        {
            connectedSockets.Remove(socket);

            User userReturned;
            users.TryGetValue(socket.RemoteEndPoint.ToString(), out userReturned);

            if (userReturned == null)
                return;
            Console.Write("Connection terminated by " + socket.RemoteEndPoint + "(" + userReturned.username + ") \n");

            foreach (Socket so in connectedSockets)
                so.Send(Encoding.UTF8.GetBytes(userReturned.username + " has left the chat room :("));

            socket.Shutdown(SocketShutdown.Receive);
        }

        private static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return null;

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .LastOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}