using System.Net.Sockets;

namespace Multiplayer_Software_Game_Engineering.Networking
{
     public delegate void ConnectionEvent(object sender, TcpClient user);
     public delegate void DataReceivedEvent(TcpClient sender, byte[] data);
}