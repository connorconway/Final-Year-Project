using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final_Year_Project.Networking
{
    class NotConnectedException : Exception
    {
        public NotConnectedException()
            : base("TcpClient not connected.")
        {
        }

        public NotConnectedException(string message)
            : base(message)
        {

        }
    }

    public class Client
    {
        private static TcpClient tcpClient;
        private static NetworkStream stream;
        private Vector2 position;


        public Client()
        {
            tcpClient = new TcpClient {ReceiveTimeout = 5000};
        }

        public bool Connect(string hostname, int port)
        {
            try
            {
                tcpClient.Connect(hostname, port);
                tcpClient.ReceiveTimeout = 2000;
                stream = tcpClient.GetStream();
                Console.WriteLine("Connected successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("A connection to the server could not be established");
                return false;
            }

            return true;
        }

        public static void Run()
        {
            if (!tcpClient.Connected)
                throw new NotConnectedException();

            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);


            while (true)
            {
                writeToServer();
                responseFromServer(streamReader);
            }

        }

        private void sendDataToServer()
        {

        }


        private static void writeToServer()
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            Console.WriteLine("\n \n Sending to server: 'Client Connected'");
            writer.WriteLine("Client connected");
            writer.Flush();
        }

        private static void responseFromServer(StreamReader streamReader)
        {
            try
            {
                if (streamReader.Peek() > 0)
                {
                    string sentFromClient;
                    while ((sentFromClient = streamReader.ReadLine()) != null)
                    {
                        Console.Write(sentFromClient);
                    }
                }
               

            }
            catch (Exception e)
            {
                Console.Write("Error: " + e);
            }
            
            
               

              
            

        }
    }
}
