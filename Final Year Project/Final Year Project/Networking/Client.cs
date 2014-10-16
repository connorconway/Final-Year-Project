using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Final_Year_Project.Components;
using Final_Year_Project.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        protected static Game1 gameReference;


        public Client(Game game)
        {
            tcpClient = new TcpClient {ReceiveTimeout = 5000};
            gameReference = (Game1)game;
        }

        public bool Connect(string hostname, int port)
        {
            try
            {
                tcpClient.Connect(hostname, port);
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
            StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);

            while (true)
            {
                writeToServer(streamWriter);
                responseFromServer(streamReader);
            }
        }


        private static void writeToServer(StreamWriter streamWriter)
        {
            Console.Write("Writing to server");
            streamWriter.WriteLine(GamePlayScreen.player.camera.Position);
            streamWriter.Flush();
        }

        private static void responseFromServer(StreamReader streamReader)
        {
            try
            {
                string sentFromClient;
                while ((sentFromClient = streamReader.ReadLine()) != null)
                {
                    Console.Write("Received from server: " + sentFromClient);

                    int startInd = sentFromClient.IndexOf("X:") + 2;
                    float aXPosition =
                        float.Parse(sentFromClient.Substring(startInd, sentFromClient.IndexOf(" Y") - startInd));
                    startInd = sentFromClient.IndexOf("Y:") + 2;
                    float aYPosition =
                        float.Parse(sentFromClient.Substring(startInd, sentFromClient.IndexOf("}") - startInd));

                    Console.Write("After conversion, X: " + aXPosition + ", Y: " + aYPosition);

                    Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
                    Animation animation = new Animation(3, 32, 32, 0, 0);
                    animations.Add(AnimationKey.Down, animation);
                    animation = new Animation(3, 32, 32, 0, 32);
                    animations.Add(AnimationKey.Left, animation);
                    animation = new Animation(3, 32, 32, 0, 64);
                    animations.Add(AnimationKey.Right, animation);
                    animation = new Animation(3, 32, 32, 0, 96);
                    animations.Add(AnimationKey.Up, animation);
                    AnimatedSprite sprite =
                        new AnimatedSprite(gameReference.Content.Load<Texture2D>(@"Graphics\Sprites\malefighter"),
                            animations);

                    try
                    {
                        GamePlayScreen.playersInServer.Add(new Player(gameReference, sprite));
                        Console.Write(GamePlayScreen.playersInServer.Count);
                    }
                    catch (Exception e)
                    {
                        Console.Write("ERROR: " + e);
                    }

                }

            }
            catch (NullReferenceException e)
            {
                Console.Write("Error: " + e);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
