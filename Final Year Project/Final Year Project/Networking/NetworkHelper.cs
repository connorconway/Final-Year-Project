//using System;
//using System.IO;
//using System.Net.Sockets;
//
//namespace Multiplayer_Software_Game_Engineering.Networking
//{
//    class NetworkHelper
//    {
//        public static TcpClient client;
//
//        public static MemoryStream readStream;
//        public static BinaryReader reader;
//        public static MemoryStream writeStream;
//        public static BinaryWriter writer;
//
//        public static byte[] readBuffer;
//
//        public static void ReadStream(byte[] data)
//        {
//            readStream.SetLength(0);
//            readStream.Position = 0;
//            readStream.Write(data, 0, data.Length);
//            readStream.Position = 0;
//        }
//
//        public static byte[] GetDataFromMemoryStream(MemoryStream ms)
//        {
//            byte[] result;
//
//            lock (ms)
//            {
//                int bytesWritten = (int)ms.Position;
//                result = new byte[bytesWritten];
//
//                ms.Position = 0;
//                ms.Read(result, 0, bytesWritten);
//            }
//
//            return result;
//        }
//
//        public static void SendData(byte[] b)
//        {
//            try
//            {
//                lock (client.GetStream())
//                {
//                    client.GetStream().BeginWrite(b, 0, b.Length, null, null);
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Client {0}:  {1}", NetworkConstants.hostname, e);
//            }
//        }
//    }
//}