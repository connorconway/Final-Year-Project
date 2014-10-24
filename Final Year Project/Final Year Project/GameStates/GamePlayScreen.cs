using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Final_Year_Project.Components;
using Final_Year_Project.Handlers;
using Final_Year_Project.Networking;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.GameStates
{
    public class GamePlayScreen : BaseGameState
    {
        #region Variables
        private Engine engine = new Engine(32, 32);
        public static World world { get; set; }
        private bool secondPlayerAnimating;
        private int playerKills = 0;
        
            
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

            textBox = new TextBox(textBoxSprite, new Vector2(player1.animatedSprite.position.X + 400, player1.animatedSprite.position.Y + 200), font,
                "Kill the enemy player \nPress [space] to shoot \nPress [w a s d] to move \n\nPress [Enter] to close textbox", 1.0f);
            textBox.decreaseAlpha = true;

            scoreTextBox = new TextBox(textBoxSprite, new Vector2(0,0), font,
                 "Kills: " + playerKills, 0.4f);
            scoreTextBox.setOpacity(0.7f);
            scoreTextBox.setAlphaTimeSubtract(0.0f);
            scoreTextBox.decreaseAlpha = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (player2 == null && textBox.decreaseAlpha == false && textBox.textBoxExists == TextBox.TextBoxExists.Transparent)
            {
                textBox.setPosition(new Vector2(textBox.position.X - 255, textBox.Position.Y + 55));
                textBox.setText(
                            "\n\nWaiting for a player to join room");
                textBox.decreaseAlpha = true;
                waitingForPlayer = true;
            }
            if (InputHandler.KeyReleased(Keys.P))
            {
                stateManager.PushState(gameReference.pauseScreen);
            }
            if (InputHandler.KeyReleased(Keys.Enter) && waitingForPlayer == false)
            {
                textBox.decreaseAlpha = false;
            }

            world.Update(gameTime);

            if (player2 != null)
            {
                foreach (Bullet bullet in player1.bullets)
                {
                    if (bullet.boundingBox.Intersects(player2.animatedSprite.boundingBox))
                    {
                        bullet.bulletLife = BulletLife.Dead;
                        bullet.boundingBox = new Rectangle(0,0,0,0);
                        player2.playerHealth.currentHealth -= 8;
                    }
                }
                foreach (Bullet bullet in player2.bullets)
                {
                    if (bullet.boundingBox.Intersects(player1.animatedSprite.boundingBox))
                    {
                        bullet.bulletLife = BulletLife.Dead;
                        bullet.boundingBox = new Rectangle(0, 0, 0, 0);
                        player1.playerHealth.currentHealth -= 8;

                        textBox.setText(
                            "You have taken damage \nProtect yourself! \n\nPress [Enter] to close textbox");
                        textBox.decreaseAlpha = true;
                    }
                }
            }

            if (player1.createBullet)
            {
                writeStream.Position = 0;
                writer.Write((byte)Protocol.BulletCreated);
                writer.Write(player1.playerOrigin.X);
                writer.Write(player1.playerOrigin.Y);
                writer.Write(player1.animatedSprite.currentAnimation.ToString());
                writer.Write(player1.motion.X);
                writer.Write(player1.motion.Y);
                SendData(GetDataFromMemoryStream(writeStream));
                writer.Flush();
            }

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
        
            if (player2 != null)
            {
                player2.animatedSprite.Update(gameTime);
                player2.UpdateHealthBar(gameTime);
                foreach (Bullet bullet in player2.bullets)
                {
                    bullet.Update(gameTime);
                }
                if (player2.playerHealth.currentHealth <= 0)
                {
                    writeStream.Position = 0;
                    writer.Write((byte)Protocol.GameOver);
                    SendData(GetDataFromMemoryStream(writeStream));
                    writer.Flush();
                    playerKills += 1;
                    scoreTextBox.setText("Kills: " + playerKills);
                    scoreTextBox.decreaseAlpha = true;
                }
            }

            player1.Update(gameTime);
            textBox.Update(gameTime);
            scoreTextBox.Update(gameTime);
            scoreTextBox.setPosition(new Vector2(player1.camera.Position.X + Game1._systemOptions.resolutionWidth - (textBoxSprite.Width * 0.4f), player1.camera.Position.Y));

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

            textBox.Draw(gameTime, gameReference.spriteBatch);
            scoreTextBox.Draw(gameTime, gameReference.spriteBatch);

            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }

        #endregion
    }
}
