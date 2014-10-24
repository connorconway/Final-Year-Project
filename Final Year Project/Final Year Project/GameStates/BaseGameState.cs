using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Final_Year_Project.Components;
using Final_Year_Project.Controls;
using Final_Year_Project.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.GameStates
{
    public abstract class BaseGameState : GameState
    {
        #region Variables
        protected const string hostname = "127.0.0.1";
        protected const int port = 4444;
        protected TcpClient client;
        protected const int bufferSize = 2048;
        protected byte[] readBuffer;
        protected static Player player1;
        protected static Player player2;

        protected MemoryStream readStream;
        protected BinaryReader reader;
        public MemoryStream writeStream;
        protected BinaryWriter writer;

        public static int clientID;
        public static string performAction = "";

        protected readonly Game1 gameReference;
        protected ControlManager controlManager;
        protected PlayerIndex playerIndexInControl;

        protected static Texture2D backgroundImage;
        protected static Texture2D backgroundBorder;
        protected static Color color = Color.White;
        private const float AlphaTime = 3500f;                                          // total animate time (at milliseconds)
        static float AlphaTimeSubtract = 500.0f;                                        // at milliseconds
        static private bool increaseAlpha;

        protected Texture2D bulletSprite;
        protected Texture2D healthBarSprite;
        protected Texture2D textBoxSprite;
        protected SpriteFont font;

        protected static TextBox textBox;
        protected static TextBox scoreTextBox;
        protected bool waitingForPlayer = false;

        #endregion

        #region Constructor(s)
        protected BaseGameState(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
            gameReference = (Game1)game;
            playerIndexInControl = PlayerIndex.One;
        }
        #endregion

        #region Override Methods
        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            SpriteFont menuFont = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            backgroundImage = Content.Load<Texture2D>(@"Graphics/Menus/titlescreen3");
            backgroundBorder = Content.Load<Texture2D>(@"Graphics/Menus/border");
            bulletSprite = Content.Load<Texture2D>(@"Graphics/Sprites/normalbullet");
            healthBarSprite = Content.Load<Texture2D>(@"Graphics/Sprites/healthBarOutline");
            textBoxSprite = Content.Load<Texture2D>(@"Graphics/GUI/textBox");
            font = Content.Load<SpriteFont>(@"Fonts/ControlFont");

            controlManager = new ControlManager(menuFont);
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

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region Networking Methods
        protected void StreamReceived(IAsyncResult ar)
        {
            int bytesRead = 0;

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

            byte[] data = new byte[bytesRead];

            for (int i = 0; i < bytesRead; i++)
            {
                data[i] = readBuffer[i];
            }

            ProcessData(data);

            client.GetStream().BeginRead(readBuffer, 0, bufferSize, StreamReceived, null);
        }

        private void ProcessData(byte[] data)
        {
            readStream.SetLength(0);
            readStream.Position = 0;

            readStream.Write(data, 0, data.Length);

            readStream.Position = 0;

            try
            {
                Protocol protocol = (Protocol)reader.ReadByte();

                
                if (protocol == Protocol.Connected)
                {
                    string textTexture = reader.ReadString();
                    float posX= reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    byte id = reader.ReadByte();
                    string ip = reader.ReadString();

                    try
                    {
                        if (player2 == null)
                        {
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
                                  new AnimatedSprite(Game.Content.Load<Texture2D>(@"Graphics\Sprites\" + textTexture), animations);

                            player2 = new Player(gameReference, sprite, bulletSprite, healthBarSprite, Color.Red);
                            player2.animatedSprite.Position = new Vector2(posX, posY);

                            waitingForPlayer = false;
                            textBox.decreaseAlpha = false;

                            writeStream.Position = 0;
                            writer.Write((byte)Protocol.Connected);
                            writer.Write(player1.animatedSprite.textTexture);
                            writer.Write(player1.animatedSprite.Position.X);
                            writer.Write(player1.animatedSprite.Position.Y);
                            SendData(GetDataFromMemoryStream(writeStream));
                            writer.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        
                    }             
                }
                if (protocol == Protocol.Disconnected)
                {
                    byte id = reader.ReadByte();
                    string ip = reader.ReadString();
                    Console.WriteLine("Player has disconnected: {0}  The IP address is: {1}", id, ip);
                    player2 = null;
                }
                if (protocol == Protocol.PlayerMoved)
                {
                    float posX = reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    bool animating = reader.ReadBoolean();
                    byte id = reader.ReadByte();
                    string ip = reader.ReadString();

                    if (player2 != null)
                    {
                        Vector2 motion = new Vector2(posX, posY);

                        if (motion.Y == -1)
                        {
                            player2.animatedSprite.currentAnimation = AnimationKey.Up;
                        }
                        else if (motion.Y == 1)
                        {
                            player2.animatedSprite.currentAnimation = AnimationKey.Down;
                        }
                        if (motion.X == -1)
                        {
                            player2.animatedSprite.currentAnimation = AnimationKey.Left;
                        }
                        else if (motion.X == 1)
                        {
                            player2.animatedSprite.currentAnimation = AnimationKey.Right;
                        }

                        player2.animatedSprite.isAnimating = animating;
                        player2.motion.Normalize();
                        player2.animatedSprite.Position += motion * player2.animatedSprite.Speed;
                        player2.animatedSprite.LockToMap();
                    }
                }
                if (protocol == Protocol.BulletCreated)
                {
                    float posX = reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    string spriteFacing = reader.ReadString();
                    float motionX = reader.ReadSingle();
                    float motionY = reader.ReadSingle();
                    byte id = reader.ReadByte();
                    string ip = reader.ReadString();

                    Vector2 position = new Vector2(posX, posY);
                    Vector2 motion = new Vector2(motionX, motionY);

                    if (player2 != null)
                    {
                        player2.bullets.Add(new Bullet(bulletSprite, position, spriteFacing, motion));
                    }
                }
                if (protocol == Protocol.GameOver)
                {
                    byte id = reader.ReadByte();
                    string ip = reader.ReadString();
                    writeStream.Position = 0;
                    writer.Write((byte)Protocol.Disconnected);
                    SendData(GetDataFromMemoryStream(writeStream));
                    writer.Flush();

                    stateManager.PushState(gameReference.gameLoseScreen);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        protected byte[] GetDataFromMemoryStream(MemoryStream ms)
        {
            byte[] result;

            lock (ms)
            {
                int bytesWritten = (int)ms.Position;
                result = new byte[bytesWritten];

                ms.Position = 0;
                ms.Read(result, 0, bytesWritten);
            }

            return result;
        }

        public void SendData(byte[] b)
        {
            //Try to send the data.  If an exception is thrown, disconnect the client
            try
            {
                lock (client.GetStream())
                {
                    client.GetStream().BeginWrite(b, 0, b.Length, null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Client {0}:  {1}", hostname, e.ToString());
            }
        }
        #endregion
    }
}
