using System.Net.Sockets;

namespace Final_Year_Project.Networking
{
     public delegate void ConnectionEvent(object sender, TcpClient user);
     public delegate void DataReceivedEvent(TcpClient sender, byte[] data);
}