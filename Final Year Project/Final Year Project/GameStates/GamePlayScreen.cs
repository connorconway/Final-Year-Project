using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Final_Year_Project.Components;
using Final_Year_Project.Handlers;
using Final_Year_Project.Networking;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.GameStates
{
    public class GamePlayScreen : BaseGameState
    {
        #region Variables
        private Engine engine = new Engine(32, 32);
        public static Player player { get; set; }
        public static List<Player> playersInServer { get; set; } 
        public static World world { get; set; }
        private Client client;
        private const string hostname = "192.168.0.11";
        private const int port = 4444;
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        #endregion

        #region Constructor(s)
        public GamePlayScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
            AllocConsole();  
            world = new World(game, gameReference.screenRectangle);
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            try
            {
                client = new Client(gameReference);
                if (!client.Connect(hostname, port))
                {
                    Console.WriteLine("Could not connect. Ensure the server is running");                              //TODO: Lobby, Connection GameState
                    stateManager.PopState();
                }
                else
                {
                    playersInServer = new List<Player>();
                    Thread clientThread = new Thread(Client.Run);
                    clientThread.Start();
                }  
            }
            catch (Exception)
            {
                Console.WriteLine("An error has occured.");
                stateManager.PopState();
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.P))
            {
                stateManager.PushState(gameReference.pauseScreen);
            }

            
            world.Update(gameTime);
            player.Update(gameTime);
            foreach (var onlinePlayers in playersInServer)
            {
                //onlinePlayers.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameReference.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                null, null, null, player.camera.Transformation);

            world.DrawLevel(gameReference.spriteBatch, player.camera);
            player.Draw(gameTime, gameReference.spriteBatch);
            foreach (var onlinePlayers in playersInServer)
            {
                Console.Write("online players: " + onlinePlayers.ToString());
                onlinePlayers.Draw(gameTime, gameReference.spriteBatch);
            }
            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }
        #endregion
    }
}
