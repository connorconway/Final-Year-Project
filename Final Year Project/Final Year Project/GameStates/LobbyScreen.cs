using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Multiplayer_Software_Game_Engineering.Controls;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class LobbyScreen : BaseGameState
    {
        private List<Player> hosts; 
        private List<Texture2D> gameHostTexture2D;
        private string lobbyHostTexture;
        private List<LinkLabel> linksToRooms;

        public LobbyScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
        }

        public override void Initialize()
        {
            client = new TcpClient { NoDelay = true };
            client.Connect(NetworkConstants.hostname, NetworkConstants.port);

            readBuffer = new byte[NetworkConstants.bufferSize];

            client.GetStream().BeginRead(readBuffer, 0, NetworkConstants.bufferSize, StreamReceived, null);

            readStream = new MemoryStream();
            reader = new BinaryReader(readStream);

            writeStream = new MemoryStream();
            writer = new BinaryWriter(writeStream);

            gameHostTexture2D = new List<Texture2D>();
            linksToRooms = new List<LinkLabel>();
            hosts = new List<Player>();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            base.LoadContent();
            CreateOptions();
        }

        private void CreateOptions()
        {
            foreach (Texture2D texture in gameHostTexture2D)
            {
                LinkLabel linklabel = new LinkLabel {text = "Game 1: " + lobbyHostTexture.ToString()};
                linklabel.size = linklabel.spriteFont.MeasureString(linklabel.text);
                linklabel.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linklabel.size.X) >> 1,
                Game1.systemOptions.resolutionHeight / 2);
                linksToRooms.Add(linklabel);
                controlManager.Add(linklabel);
            }

            LinkLabel linkLabel1 = new LinkLabel { text = Constants.CREATE_LOBBY};
            linkLabel1.size = linkLabel1.spriteFont.MeasureString(linkLabel1.text);
            linkLabel1.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel1.size.X) >> 1,
                Game1.systemOptions.resolutionHeight / 2);
            linkLabel1.selected += linkLabel1_Selected;

            LinkLabel linkLabel2 = new LinkLabel { text = Constants.BACK };
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                linkLabel1.position.Y + 75);
            linkLabel2.selected += linkLabel2_Selected;

            controlManager.Add(linkLabel1);
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

            foreach (Texture2D texture in gameHostTexture2D)
            {
                player1.animatedSprite.Draw(gameReference.spriteBatch);
            }

            foreach (LinkLabel linkToRoom in linksToRooms)
            {
                
            }
            controlManager.Draw(gameReference.spriteBatch);
            gameReference.spriteBatch.End();            
        }

        private void linkLabel1_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            player1.isHost = true;
            stateManager.PushState(gameReference.gamePlayScreen);
        }

        private void joinLobby_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            stateManager.PushState(gameReference.lobbyScreen);
        }

        private void linkLabel2_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            stateManager.PopState();
        }
    }
}
