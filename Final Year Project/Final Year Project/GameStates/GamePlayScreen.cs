using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.Networking;
using Multiplayer_Software_Game_Engineering.TileEngine;
using Multiplayer_Software_Game_Engineering.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using TextBox = Multiplayer_Software_Game_Engineering.GameEntities.TextBox;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class GamePlayScreen : BaseGameState
    {
        private Engine engine = new Engine(32, 32);
        public static World world { private get; set; }
        private bool secondPlayerAnimating;
        private int playerKills;    

        public GamePlayScreen(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
            world = new World(game, gameReference.screenRectangle);
        }

        public override void Initialize()
        {
            try
            {
                client = new TcpClient { NoDelay = true };
                client.Connect(NetworkConstants.hostname, NetworkConstants.port);
                readBuffer = new byte[NetworkConstants.bufferSize];
                client.GetStream().BeginRead(readBuffer, 0, NetworkConstants.bufferSize, StreamReceived, null);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format(Constants.ERROR_CONNECTION + NetworkConstants.port));
            }

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

            textBox = new TextBox(textBoxSprite, new Vector2(player1.animatedSprite.position.X + 400, player1.animatedSprite.position.Y + 200), fontSprite,
                Constants.INFO_CONTROLS, 1.0f)
            {decreaseAlpha = true};

            scoreTextBox = new TextBox(textBoxSprite, new Vector2(0, 0), fontSprite,
                 "Kills: " + playerKills, 0.4f);
            scoreTextBox.setOpacity(0.7f);
            scoreTextBox.setAlphaTimeSubtract(0.0f);
            scoreTextBox.decreaseAlpha = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (player2 == null && textBox.decreaseAlpha == false && textBox.textBoxExists == TextBox.TextBoxExists.Transparent)
            {
                textBox.setPosition(new Vector2(textBox.position.X - 255, textBox.Position.Y + 55));
                textBox.setText(Constants.INFO_WAITING);
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

                        textBox.setText(Constants.INFO_DAMAGED);
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
                    scoreTextBox.setText(string.Format("Kills: {0}", playerKills));
                    scoreTextBox.decreaseAlpha = true;
                }
            }

            player1.Update(gameTime);
            textBox.Update(gameTime);
            scoreTextBox.Update(gameTime);
            scoreTextBox.setPosition(new Vector2(player1.camera.position.X + Game1.systemOptions.resolutionWidth - (textBoxSprite.Width * 0.4f), player1.camera.position.Y));

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
    }
}
