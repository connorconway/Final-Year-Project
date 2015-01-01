using Multiplayer_Software_Game_Engineering.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Multiplayer_Software_Game_Engineering.GameData;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class StartMenuScreen : BaseGameState
    {
        PictureBox arrowImageLeft;
        PictureBox arrowImageRight;
        LinkLabel startGame;
        LinkLabel loadGame;
        LinkLabel gameSettings;
        LinkLabel highscores;
        LinkLabel exitGame;
        float maxItemWidth;

        public StartMenuScreen(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = Game.Content;

            Texture2D arrowTexture = content.Load<Texture2D>(@"Graphics\GUI\leftarrowUp");

            arrowImageLeft = new PictureBox(
                arrowTexture,
                new Rectangle( 0, 0,
                    arrowTexture.Width,
                    arrowTexture.Height));
            
            arrowImageLeft.setSpriteEffect(SpriteEffects.FlipHorizontally);

            arrowImageRight = new PictureBox(
                arrowTexture,
                new Rectangle(0, 0,
                    arrowTexture.Width,
                    arrowTexture.Height));

            startGame = new LinkLabel { text = Constants.NEW_CHARACTER };
            startGame.size = startGame.spriteFont.MeasureString(startGame.text);
            startGame.selected += MenuItemSelected;

            loadGame = new LinkLabel {text = Constants.LOAD_CHARACTER};
            loadGame.size = loadGame.spriteFont.MeasureString(loadGame.text);
            loadGame.selected += MenuItemSelected;

            gameSettings = new LinkLabel { text = Constants.GAME_SETTINGS };
            gameSettings.size = gameSettings.spriteFont.MeasureString(gameSettings.text);
            gameSettings.selected += MenuItemSelected;

            highscores = new LinkLabel { text = Constants.HIGH_SCORES };
            highscores.size = gameSettings.spriteFont.MeasureString(highscores.text);
            highscores.selected += MenuItemSelected;

            exitGame = new LinkLabel { text = Constants.QUIT_GAME };
            exitGame.size = exitGame.spriteFont.MeasureString(exitGame.text);
            exitGame.selected += MenuItemSelected;

            controlManager.Add(arrowImageLeft);
            controlManager.Add(arrowImageRight);
            controlManager.Add(startGame);
            controlManager.Add(loadGame);
            controlManager.Add(gameSettings);
            controlManager.Add(highscores);
            controlManager.Add(exitGame);
            controlManager.NextControl();

            controlManager.focusChanged += ControlManagerFocusChanged;

            int padding = 0;
            foreach (LinkLabel c in controlManager.OfType<LinkLabel>())
            {
                Vector2 position = new Vector2((int)(Game1.systemOptions.resolutionWidth - c.size.X) >> 1,
                    (Game1.systemOptions.resolutionHeight >> 1) + padding);

                if (c.size.X > maxItemWidth)
                    maxItemWidth = c.size.X;
                c.position = position;
                position.Y += c.size.Y + 5f;
                padding += 40;
            }

            ControlManagerFocusChanged(startGame, null);
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime, playerIndexInControl);
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

        void ControlManagerFocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;
            Vector2 positionLeft = new Vector2(startGame.position.X - ( (int)maxItemWidth >> 1) - 50f, control.position.Y);
            Vector2 positionRight = new Vector2(startGame.position.X + maxItemWidth + 10f, control.position.Y);
            arrowImageLeft.SetPosition(positionLeft);
            arrowImageRight.SetPosition(positionRight);
        }

        private void MenuItemSelected(object sender, EventArgs e)
        {
            if (sender == startGame)
            {
                stateManager.PushState(gameReference.characterCreationScreen);
            }
            if (sender == loadGame)
            {
                stateManager.PushState(gameReference.loadGameScreen);
            }
            if (sender == gameSettings)
            {
                stateManager.PushState(gameReference.optionsScreen);
            }
            if (sender == highscores)
            {
                stateManager.PushState(gameReference.highScoreScreen);
            }
            if (sender == exitGame)
            {
                gameReference.Exit();
            }

        }
    }
}