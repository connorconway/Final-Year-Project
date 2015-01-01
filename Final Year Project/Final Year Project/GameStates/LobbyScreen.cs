using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LinkLabel = Multiplayer_Software_Game_Engineering.Controls.LinkLabel;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class LobbyScreen : BaseGameState
    {
        private List<Player> hosts; 
        private List<Texture2D> gameHostTexture2D;
        private List<LinkLabel> linksToRooms;

        public LobbyScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
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

            gameHostTexture2D = new List<Texture2D>();
            linksToRooms = new List<LinkLabel>();
            hosts = new List<Player>();

            base.Initialize();

            CreateOptions();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        private void CreateOptions()
        {
//            writeStream.Position = 0;
//            writer.Write((byte)Protocol.Connected);
//            writer.Write(player1.animatedSprite.textTexture);
//            writer.Write(player1.animatedSprite.Position.X);
//            writer.Write(player1.animatedSprite.Position.Y);
//            writer.Write(player1.isHost);
//            SendData(GetDataFromMemoryStream(writeStream));
//            writer.Flush();

            LinkLabel linklabel;

            if (player2 == null)
            {
                linklabel = new LinkLabel {text = "Game 1 - FEMALE ROGUE"};
                linklabel.size = linklabel.spriteFont.MeasureString(linklabel.text);
                linklabel.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linklabel.size.X) >> 1,
                Game1.systemOptions.resolutionHeight / 2);
            }
            else
            {
                linklabel = new LinkLabel { text = Constants.CREATE_LOBBY };
                linklabel.size = linklabel.spriteFont.MeasureString(linklabel.text);
                linklabel.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linklabel.size.X) >> 1,
                    Game1.systemOptions.resolutionHeight / 2);
            }

 
            LinkLabel linkLabel2 = new LinkLabel { text = Constants.BACK };
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                linklabel.position.Y + 75);

            linklabel.selected += linkLabel_Selected;
            linkLabel2.selected += linkLabel2_Selected;

            controlManager.Add(linklabel);
            controlManager.Add(linkLabel2);
        }


        public override void Update(GameTime gameTime)
        {

            if (player1 != null && player1.isHost)
            {
                hosts.Add(player1);
            }

            foreach (Player player in hosts)
            {
                gameHostTexture2D.Add(player.animatedSprite.sprite);
            }

         
            controlManager.Update(gameTime, PlayerIndex.One);
                        
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameReference.spriteBatch.Begin();
            base.Draw(gameTime);
            gameReference.spriteBatch.Draw(backgroundImage, gameReference.screenRectangle, color);
            gameReference.spriteBatch.Draw(backgroundBorder, gameReference.screenRectangle, Color.White);

            controlManager.Draw(gameReference.spriteBatch);
            gameReference.spriteBatch.End();            
        }

        private void linkLabel_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            player1.isHost = true;
            stateManager.PushState(gameReference.gamePlayScreen);
        }

        private void joinLobby_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            stateManager.PushState(gameReference.gamePlayScreen);
        }

        private void linkLabel2_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            client.Close();
            stateManager.PopState();
        }
    }
}
