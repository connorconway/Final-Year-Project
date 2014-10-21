using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        public static World world { get; set; }
        private bool secondPlayerAnimating;
        
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
            client = new TcpClient {NoDelay = true};
            client.Connect(hostname, port);

            readBuffer = new byte[bufferSize];

            client.GetStream().BeginRead(readBuffer, 0, bufferSize, StreamReceived, null);

            readStream = new MemoryStream();
            reader = new BinaryReader(readStream);

            writeStream = new MemoryStream();
            writer = new BinaryWriter(writeStream);

            writeStream.Position = 0;
            writer.Write((byte)Protocol.Connected);
            writer.Write(player1.animatedSprite.textTexture);
            writer.Write(player1.animatedSprite.Position.X);
            writer.Write(player1.animatedSprite.Position.Y);
            SendData(GetDataFromMemoryStream(writeStream));
            writer.Flush();

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

            if (player1.motion != Vector2.Zero)
            {
                secondPlayerAnimating = true;
                writeStream.Position = 0;
                writer.Write((byte) Protocol.PlayerMoved);
                writer.Write(player1.motion.X);
                writer.Write(player1.motion.Y);
                writer.Write(player1.animatedSprite.isAnimating);
                SendData(GetDataFromMemoryStream(writeStream));
                writer.Flush();
            }
            else if (secondPlayerAnimating)
            {
                secondPlayerAnimating = false;
                writeStream.Position = 0;
                writer.Write((byte)Protocol.PlayerMoved);
                writer.Write(player1.motion.X);
                writer.Write(player1.motion.Y);
                writer.Write(player1.animatedSprite.isAnimating);
                SendData(GetDataFromMemoryStream(writeStream));
                writer.Flush();
            }

            if (player1 != null)
                player1.Update(gameTime);
            if (player2 != null)
                player2.animatedSprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameReference.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    null, null, null, player1.camera.Transformation);

            world.DrawLevel(gameReference.spriteBatch, player1.camera);

            if (player1 != null)
                player1.Draw(gameTime, gameReference.spriteBatch);

            if (player2 != null)
                player2.Draw(gameTime, gameReference.spriteBatch);               

            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }
        #endregion
    }
}
