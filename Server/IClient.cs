using System;
using System.IO;

namespace Server
{
    interface IClient
    {
        event DataReceivedEvent DataReceived;
        event ConnectionEvent UserDisconnected;
        
        void   SendData(byte[] b);
        void   SendMemoryStream(MemoryStream ms);
        string ToString();
    }
}