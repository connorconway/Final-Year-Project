using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Listening for connections on {0}:{1}\n", LocalIPAddress(), Properties.NetworkSettings.Default.Port);

            try
            {
                new Server(Properties.NetworkSettings.Default.Port);

                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
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