using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.Controls;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.Handlers;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class HighScoresScreen : BaseGameState
    {
        private List<Player> hosts; 
        private List<Texture2D> gameHostTexture2D;
        private List<LinkLabel> linksToRooms;
        private List<Tuple<string, int>> highScores = new List<Tuple<string, int>>();
        List<Label> highscoreLabels = new List<Label>();

        public HighScoresScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
            
        }

        public override void Initialize()
        {
            highScores = DataBaseHandler.ReadData("Multiplayer_Game_Data", "HighScores3");
            highScores.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            Label linklabel = new Label { text = "HIGHSCORES" };
            linklabel.size = linklabel.spriteFont.MeasureString(linklabel.text);
            linklabel.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linklabel.size.X) >> 1,
            50);

            int posForLabel = 1;
            int posForLabelLane2 = 1;

            bool moreThan10 = highScores.Count > 9;

            foreach (var element in highScores)
            {
                posForLabel++;
                posForLabelLane2++;
                int extraMove = 0;
                if (moreThan10 && posForLabel < 12)
                    extraMove = -250;
                else if (moreThan10 && posForLabel > 11)
                {
                    extraMove = 250;
                    if (posForLabelLane2 > 9)
                        posForLabelLane2 = 2;

                }

                Label templabel = new Label { text = element.Item1 + " - " + element.Item2 };
                templabel.size = linklabel.spriteFont.MeasureString(templabel.text);
                templabel.position = new Vector2(((int)(Game1.systemOptions.resolutionWidth - templabel.size.X) >> 1) + extraMove,
                posForLabelLane2 * 50);

                highscoreLabels.Add(templabel);
            }

            LinkLabel highScoresToday = new LinkLabel { text = "Todays High Scores" };
            highScoresToday.size = highScoresToday.spriteFont.MeasureString(highScoresToday.text);
            highScoresToday.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - highScoresToday.size.X) >> 1,
                Game1.systemOptions.resolutionHeight - 175);

            LinkLabel linkLabel2 = new LinkLabel { text = Constants.BACK };
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                Game1.systemOptions.resolutionHeight - 125);

            linkLabel2.selected += linkLabel2_Selected;
            highScoresToday.selected += highScoresToday_Selected;

            base.Initialize();

            controlManager.Add(linklabel);
            controlManager.Add(linkLabel2);
            foreach (var label in highscoreLabels)
            {
                controlManager.Add(label);
            }
         //   controlManager.Add(highScoresToday);
            controlManager.NextControl();
        }


        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {          
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
            stateManager.PushState(gameReference.Level1);
        }

        private void joinLobby_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            stateManager.PushState(gameReference.Level1);
        }

        private void linkLabel2_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            controlManager.Clear();
            stateManager.PopState();
        }

        private void highScoresToday_Selected(object sender, EventArgs e)
        {
            controlManager.Clear();
        }
    }
}
