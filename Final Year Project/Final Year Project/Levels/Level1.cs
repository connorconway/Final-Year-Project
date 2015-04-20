using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.GameStates;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.Networking;
using Multiplayer_Software_Game_Engineering.TileEngine;
using Multiplayer_Software_Game_Engineering.WorldClasses;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using TextBox = Multiplayer_Software_Game_Engineering.GameEntities.TextBox;
using HUD = Multiplayer_Software_Game_Engineering.GameEntities.HUD;


namespace Multiplayer_Software_Game_Engineering.Levels
{
    public class Level1 : BaseGameState
    {
        private          System.Timers.Timer syncTimer; 
        public  static   World  world                  { private get; set; }
        private          Engine engine                 = new Engine(32, 32);
        private          bool   secondPlayerAnimating;
        private static   int    playerKills;    
        private          bool   shownHelp = false;
        private Texture2D characterImage;


        public Level1(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
            world         = new World(game, gameReference.screenRectangle);
            syncTimer     = new System.Timers.Timer();
            syncTimer.Elapsed  += SyncGames;
            syncTimer.Interval  = 20000;
            syncTimer.Enabled   = true;
        }

        public static int getPlayerKills()
        {
            return playerKills;
        }

        public override void Initialize()
        {
            if (client == null)
            {
                try
                {
                    client = new TcpClient { NoDelay = true };
                    client.Connect(NetworkConstants.hostname, NetworkConstants.port);
                    readBuffer = new byte[NetworkConstants.bufferSize];
                    client.GetStream().BeginRead(readBuffer, 0, NetworkConstants.bufferSize, StreamReceived, null);

                    readStream = new MemoryStream();
                    reader = new BinaryReader(readStream);

                    writeStream = new MemoryStream();
                    writer = new BinaryWriter(writeStream);

                    writeStream.Position = 0;
                    writer.Write((byte)Protocol.Connected);
                    writer.Write(player1.animatedSprite.textTexture);
                    writer.Write(player1.animatedSprite.Position.X);
                    writer.Write(player1.animatedSprite.Position.Y);
                    writer.Write(player1.isHost);
                    SendData(GetDataFromMemoryStream(writeStream));
                    writer.Flush();
                }
                catch (Exception)
                {
                    MessageBox.Show(string.Format(Constants.ERROR_CONNECTION + NetworkConstants.port));
                }
            }

            base.Initialize();

            textBox = new TextBox(textBoxSprite, new Vector2(player1.animatedSprite.position.X + 400, player1.animatedSprite.position.Y + 200), fontSprite,
                Constants.INFO_CONTROLS, 1.0f)

            {decreaseAlpha = true};

            scoreTextBox = new TextBox(textBoxSprite, new Vector2(0, 0), fontSprite,
                 "Kills: " + playerKills, 0.4f);
            scoreTextBox.setOpacity(0.7f);
            scoreTextBox.setAlphaTimeSubtract(0.0f);
            scoreTextBox.decreaseAlpha = false;

            playerHUD = new HUD(HUDSprite,
                new Vector2(Game1.systemOptions.resolutionWidth / 2.0f - (HUDSprite.Width*1.3f) / 2.0f, Game1.systemOptions.resolutionHeight - HUDSprite.Height),
                HUDFont, player1.level, player1.exp, player1.gold, player1.type, 1.3f);

            characterImage = Game.Content.Load<Texture2D>(@"Graphics\Sprites\" + player1.gender + player1.type);

        }

        public override void Update(GameTime gameTime)
        {
            if (client.Connected == false)
            {
                player2 = null;
            }

            if (player2 == null && textBox.decreaseAlpha == false && textBox.textBoxExists == TextBox.TextBoxExists.Transparent)
            {
                textBox.setPosition(new Vector2(textBox.position.X - 255, textBox.Position.Y + 55));
                textBox.setText(client.Connected ? Constants.INFO_WAITING : Constants.SINGLE_PLAYER);
                textBox.decreaseAlpha = true;
                waitingForPlayer = true;
            }
            if (InputHandler.KeyReleased(Keys.P))
                stateManager.PushState(gameReference.pauseScreen);
            if (InputHandler.KeyReleased(Keys.Enter) && waitingForPlayer == false)
                textBox.decreaseAlpha = false;

            world.Update(gameTime);

            if (player2 != null)
            {
                foreach (Bullet bullet in player1.bullets.Where(bullet => bullet.boundingBox.Intersects(player2.animatedSprite.boundingBox)))
                {
                    bullet.bulletLife                   = BulletLife.Dead;
                    bullet.boundingBox                  = new Rectangle(0,0,0,0);
                    player2.playerHealth.currentHealth -= 8;
                }
                foreach (Bullet bullet in player2.bullets.Where(bullet => bullet.boundingBox.Intersects(player1.animatedSprite.boundingBox)))
                {
                    bullet.bulletLife                   = BulletLife.Dead;
                    bullet.boundingBox                  = new Rectangle(0, 0, 0, 0);
                    player1.playerHealth.currentHealth -= 8;

                    if (shownHelp == false)
                    {
                        textBox.setText(Constants.INFO_DAMAGED);
                        shownHelp = true;
                    }
                    textBox.decreaseAlpha = true;
                }
            }

            if (player1.createBullet && client.Connected)
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

            if (player1.motion != Vector2.Zero && client.Connected) 
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
            else if (secondPlayerAnimating && client.Connected)
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
                player2.UpdateHealthBar();
                foreach (Bullet bullet in player2.bullets)
                    bullet.Update(gameTime, roommap);
                if (player2.playerHealth.currentHealth <= 0 && client.Connected)
                {
                    writeStream.Position = 0;
                    writer.Write((byte)Protocol.GameOver);
                    SendData(GetDataFromMemoryStream(writeStream));
                    writer.Flush();
                    playerKills += 1;
                    scoreTextBox.setText(string.Format("Kills: {0}", playerKills));
                    scoreTextBox.decreaseAlpha = true;
                    scoreTextBox.decreaseAlpha = true;
                }
            }

            player1.Update(gameTime, roommap);
            textBox.Update(gameTime);
            playerHUD.Update(gameTime);
            scoreTextBox.Update(gameTime);
            scoreTextBox.setPosition(new Vector2(player1.camera.position.X + Game1.systemOptions.resolutionWidth - (textBoxSprite.Width * 0.4f), player1.camera.position.Y));
            playerHUD.setPosition(new Vector2(player1.camera.position.X + Game1.systemOptions.resolutionWidth / 2.0f- ((HUDSprite.Width*1.3f) / 2.0f ), player1.camera.position.Y + Game1.systemOptions.resolutionHeight - (HUDSprite.Height*1.3f)));

            base.Update(gameTime);
        }

        private void SyncGames(object source, ElapsedEventArgs e)
        {
            writeStream.Position = 0;
            writer.Write((byte)Protocol.SyncGame);
            writer.Write(player1.animatedSprite.position.X);
            writer.Write(player1.animatedSprite.position.Y);
            writer.Write(player1.playerHealth.currentHealth);
            SendData(GetDataFromMemoryStream(writeStream));
            writer.Flush();
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

           // textBox.Draw(gameTime, gameReference.spriteBatch);
            playerHUD.Draw(gameTime, gameReference.spriteBatch, characterImage, player1.playerHealth);
            scoreTextBox.Draw(gameTime, gameReference.spriteBatch);

            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }
    }
}