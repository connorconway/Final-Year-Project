using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Multiplayer_Software_Game_Engineering.Controls;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.Handlers;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public abstract class BaseGameState : GameState
    {
        protected          TcpClient      client;
        protected          byte[]         readBuffer;
        protected          MemoryStream   readStream;
        protected          BinaryReader   reader;
        protected          MemoryStream   writeStream;
        protected          BinaryWriter   writer;

        protected static   Player         player1;
        protected static   Player         player2;

        protected readonly Game1          gameReference;
        protected          ControlManager controlManager;
        protected readonly PlayerIndex    playerIndexInControl;

        protected static   Texture2D      backgroundImage;
        protected static   Texture2D      backgroundBorder;
        protected static   Color          color;
        private static     float          AlphaTime;                                        
        private static     float          AlphaTimeSubtract;                                        
        private static     bool           increaseAlpha;

        protected          Texture2D      bulletSprite;
        protected          Texture2D      fireBallBulletSprite;
        protected          Texture2D      shurikenBulletSprite;
        protected          Texture2D      healthBulletSprite;
        protected          Texture2D      healthBarSprite;
        protected          Texture2D      textBoxSprite;
        protected          SpriteFont     fontSprite;

        protected static   TextBox        textBox;
        protected static   TextBox        scoreTextBox;
        protected          bool           waitingForPlayer;

        protected BaseGameState(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
            gameReference        = (Game1)game;
            playerIndexInControl = PlayerIndex.One;
            color                = Color.White;
            AlphaTime            = 3500f;
            AlphaTimeSubtract    = 500f;
        }

        protected override void LoadContent()
        {
            var Content            = Game.Content;
            backgroundImage        = Content.Load<Texture2D>(@"Graphics/Menus/titlescreen3");
            backgroundBorder       = Content.Load<Texture2D>(@"Graphics/Menus/border");
            bulletSprite           = Content.Load<Texture2D>(@"Graphics/Sprites/normalbullet");
            fireBallBulletSprite   = Content.Load<Texture2D>(@"Graphics/Sprites/fireballbullet");
            healthBulletSprite     = Content.Load<Texture2D>(@"Graphics/Sprites/healthbullet");
            shurikenBulletSprite   = Content.Load<Texture2D>(@"Graphics/Sprites/shurikenBullet");
            healthBarSprite        = Content.Load<Texture2D>(@"Graphics/Sprites/healthBarOutline");
            textBoxSprite          = Content.Load<Texture2D>(@"Graphics/GUI/textBox");
            fontSprite             = Content.Load<SpriteFont>(@"Fonts/ControlFont");

            controlManager = new ControlManager(fontSprite);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (increaseAlpha)
            {
                AlphaTimeSubtract -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
                color = Color.White * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                if (AlphaTimeSubtract <= 500)
                    increaseAlpha = false;
            }
            else
            {
                AlphaTimeSubtract += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
                color = Color.White * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                if (AlphaTimeSubtract >= 2500)
                    increaseAlpha = true;
            }

            base.Update(gameTime);
        }

        protected void StreamReceived(IAsyncResult ar)
        {
            var bytesRead = 0;

            try
            {
                lock (client.GetStream())
                {
                    bytesRead = client.GetStream().EndRead(ar);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (bytesRead == 0)
            {
                client.Close();
                return;
            }

            var data = new byte[bytesRead];

            for (var i = 0; i < bytesRead; i++)
            {
                data[i] = readBuffer[i];
            }

            ProcessData(data);

            client.GetStream().BeginRead(readBuffer, 0, NetworkConstants.bufferSize, StreamReceived, null);
        }

        private void ProcessData(byte[] data)
        {
            readStream.SetLength(0);
            readStream.Position = 0;

            readStream.Write(data, 0, data.Length);

            readStream.Position = 0;

            try
            {
                byte id;
                string ip;

                Vector2 motion;
                switch ((Protocol)reader.ReadByte())
                {
                    case Protocol.Connected:
                        var textTexture = reader.ReadString();
                        var posX        = reader.ReadSingle();
                        var posY        = reader.ReadSingle();
                        var isHost      = reader.ReadBoolean();
                        id              = reader.ReadByte();
                        ip              = reader.ReadString();

                        try
                        {
                            if (player2 == null)
                            {
                                var animations = new Dictionary<Constants.Direction, Animation>();
                                var animation  = new Animation(3, 32, 32, 0, 0);

                                animations.Add(Constants.Direction.Down, animation);
                                animation = new Animation(3, 32, 32, 0, 32);
                                animations.Add(Constants.Direction.Left, animation);
                                animation = new Animation(3, 32, 32, 0, 64);
                                animations.Add(Constants.Direction.Right, animation);
                                animation = new Animation(3, 32, 32, 0, 96);
                                animations.Add(Constants.Direction.Up, animation);

                                var sprite = new AnimatedSprite(Game.Content.Load<Texture2D>(@"Graphics\Sprites\" + textTexture), animations);

                                Texture2D spriteToUse;
                                if (textTexture.Contains("Fighter"))
                                    spriteToUse = bulletSprite;
                                else if (textTexture.Contains("Rogue"))
                                    spriteToUse = shurikenBulletSprite;
                                else if (textTexture.Contains("Priest"))
                                    spriteToUse = healthBulletSprite;
                                else
                                    spriteToUse = fireBallBulletSprite;

                                player2 = new Player(gameReference, sprite, spriteToUse, healthBarSprite, Color.Red)
                                {
                                    animatedSprite = { Position = new Vector2(posX, posY) },
                                    isHost = isHost
                                };

                                waitingForPlayer      = false;
                                textBox.decreaseAlpha = false;

                                writeStream.Position = 0;
                                writer.Write((byte)Protocol.Connected);
                                writer.Write(player1.animatedSprite.textTexture);
                                writer.Write(player1.animatedSprite.Position.X); 
                                writer.Write(player1.animatedSprite.Position.Y);
                                writer.Write(player1.isHost);
                                SendData(GetDataFromMemoryStream(writeStream));
                                writer.Flush();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Write(Constants.ERROR_GENERIC + e);
                        }
                        break;
                    case Protocol.Disconnected:
                        id = reader.ReadByte();
                        ip = reader.ReadString();
                        Console.WriteLine("Player has disconnected: {0}  The IP address is: {1}", id, ip);
                        player2 = null;
                        break;
                    case Protocol.PlayerMoved:
                        posX = reader.ReadSingle();
                        posY = reader.ReadSingle();
                        var animating = reader.ReadBoolean();
                        id = reader.ReadByte();
                        ip = reader.ReadString();
                        
                        if (player2 != null)
                        {
                            motion = new Vector2(posX, posY);

                            const double tolerance = 0.005;

                            if (Math.Abs(motion.Y - (-1)) < tolerance)
                            {
                                player2.animatedSprite.currentAnimation = Constants.Direction.Up;
                            }
                            else if (Math.Abs(motion.Y - 1) < tolerance)
                            {
                                player2.animatedSprite.currentAnimation = Constants.Direction.Down;
                            }
                            if (Math.Abs(motion.X - (-1)) < tolerance)
                            {
                                player2.animatedSprite.currentAnimation = Constants.Direction.Left;
                            }
                            else if (Math.Abs(motion.X - 1) < tolerance)
                            {
                                player2.animatedSprite.currentAnimation = Constants.Direction.Right;
                            }
                        
                            player2.animatedSprite.isAnimating = animating;
                            player2.motion.Normalize();
                            player2.animatedSprite.Position += motion * player2.animatedSprite.Speed;
                            player2.animatedSprite.LockToMap();
                        }                     
                        break;
                    case Protocol.BulletCreated:
                        posX = reader.ReadSingle();
                        posY = reader.ReadSingle();
                        var spriteFacing = reader.ReadString();
                        var motionX = reader.ReadSingle();
                        var motionY = reader.ReadSingle();
                        id = reader.ReadByte();
                        ip = reader.ReadString();

                        var position = new Vector2(posX, posY);
                        motion = new Vector2(motionX, motionY);

                        if (player2 != null)
                        {
                            player2.bullets.Add(new Bullet(player2.bulletSprite, position, spriteFacing, motion));
                        }
                        break;
                    case Protocol.GameOver:
                        id = reader.ReadByte();
                        ip = reader.ReadString();

                        writeStream.Position = 0;
                        writer.Write((byte)Protocol.Disconnected);
                        SendData(GetDataFromMemoryStream(writeStream));
                        writer.Flush();
                        stateManager.PushState(gameReference.gameLoseScreen);

                        var name = Microsoft.VisualBasic.Interaction.InputBox("What is your name?", "High Score Board Entry", "", 100, 100 );
                        DataBaseHandler.InputData("Multiplayer_Game_Data", "HighScores3", name, GamePlayScreen.getPlayerKills().ToString());

                        break;
                    case Protocol.SyncGame:
                        posX = reader.ReadSingle();
                        posY = reader.ReadSingle();
                        int playerHealth = reader.ReadInt32();
                        id = reader.ReadByte();
                        ip = reader.ReadString();

                        if (player2 != null)
                        {
                            player2.animatedSprite.position = new Vector2(posX, posY);
                            player2.playerHealth.currentHealth = playerHealth;
                        }
                        break;
                    case Protocol.CheckForHosts:
                        isHost = reader.ReadBoolean();
                        id = reader.ReadByte();
                        ip = reader.ReadString();

                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected static byte[] GetDataFromMemoryStream(MemoryStream ms)
        {
            byte[] result;

            lock (ms)
            {
                var bytesWritten = (int)ms.Position;
                result = new byte[bytesWritten];

                ms.Position = 0;
                ms.Read(result, 0, bytesWritten);
            }

            return result;
        }

        protected void SendData(byte[] b)
        {
            try
            {
                lock (client.GetStream())
                {
                    client.GetStream().BeginWrite(b, 0, b.Length, null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client {0}:  {1}", NetworkConstants.hostname, e);
            }
        }
    }
}