﻿using System;
using Multiplayer_Software_Game_Engineering.Controls;
using Microsoft.Xna.Framework;
using Multiplayer_Software_Game_Engineering.GameData;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class GameLoseScreen : BaseGameState
    {
        public GameLoseScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            CreateControls();
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

        private void CreateControls()
        {
            Label label1 = new Label { text = Constants.GAME_OVER };
            label1.size = label1.spriteFont.MeasureString(label1.text);
            label1.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - label1.size.X) >> 1,
                Game1.systemOptions.resolutionHeight >> 1);

            LinkLabel linkLabel1 = new LinkLabel { text = Constants.CREATE_LOBBY };
            linkLabel1.size = linkLabel1.spriteFont.MeasureString(linkLabel1.text);
            linkLabel1.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel1.size.X) >> 1,
                label1.position.Y + 100);
            linkLabel1.selected += linkLabel1_Selected;

            LinkLabel linkLabel2 = new LinkLabel { text = Constants.JOIN_LOBBY };
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                linkLabel1.position.Y + 50);
            linkLabel2.selected += linkLabel2_Selected;

            LinkLabel exitGame = new LinkLabel { text = Constants.QUIT_GAME };
            exitGame.size = exitGame.spriteFont.MeasureString(exitGame.text);
            exitGame.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - exitGame.size.X) >> 1,
                linkLabel2.position.Y + 50);
            exitGame.selected += exitGame_selected;

            controlManager.Add(label1);
            controlManager.Add(linkLabel1);
            controlManager.Add(linkLabel2);
            controlManager.Add(exitGame);
            controlManager.NextControl();
        }

        private void linkLabel1_Selected(object sender, EventArgs e)
        {

        }

        private void linkLabel2_Selected(object sender, EventArgs e)
        {

        }

        private void exitGame_selected(object sender, EventArgs e)
        {
            gameReference.Exit();
        }
    }
}
