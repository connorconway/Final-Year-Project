using System;
using System.Linq;
using Final_Year_Project.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.GameStates
{
    public class PauseScreen : BaseGameState
    {
        #region Variables
        private PictureBox arrowImageLeft;
        private PictureBox arrowImageRight;
        private LinkLabel continueGame;
        private LinkLabel gameControls;
        private LinkLabel mainMenu;
        private float maxItemWidth;
        #endregion

        #region Constructor(S)
        public PauseScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = Game.Content;

            Texture2D arrowTexture = content.Load<Texture2D>(@"Graphics\GUI\leftarrowUp");

            arrowImageLeft = new PictureBox(
                arrowTexture,
                new Rectangle(0, 0,
                    arrowTexture.Width,
                    arrowTexture.Height));

            arrowImageLeft.setSpriteEffect(SpriteEffects.FlipHorizontally);

            arrowImageRight = new PictureBox(
                arrowTexture,
                new Rectangle(0, 0,
                    arrowTexture.Width,
                    arrowTexture.Height));


            continueGame = new LinkLabel {text = "Continue Story"};
            continueGame.size = continueGame.spriteFont.MeasureString(continueGame.text);
            continueGame.selected += MenuItemSelected;

            gameControls = new LinkLabel { text = "Game Controls" };
            gameControls.size = gameControls.spriteFont.MeasureString(gameControls.text);
            gameControls.selected += MenuItemSelected;

            mainMenu = new LinkLabel { text = "Return To Main Menu" };
            mainMenu.size = mainMenu.spriteFont.MeasureString(mainMenu.text);
            mainMenu.selected += MenuItemSelected;

            controlManager.Add(arrowImageLeft);
            controlManager.Add(arrowImageRight);
            controlManager.Add(continueGame);
            controlManager.Add(gameControls);
            controlManager.Add(mainMenu);
            controlManager.NextControl();

            controlManager.focusChanged += ControlManagerFocusChanged;

            int padding = 0;
            foreach (LinkLabel c in controlManager.OfType<LinkLabel>())
            {
                Vector2 position = new Vector2((int)(Game1._systemOptions.resolutionWidth - c.size.X) >> 1,
                    (Game1._systemOptions.resolutionHeight >> 1) + padding);

                if (c.size.X > maxItemWidth)
                    maxItemWidth = c.size.X;
                c.position = position;
                position.Y += c.size.Y + 5f;
                padding += 40;
            }

            ControlManagerFocusChanged(continueGame, null);
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
        #endregion

        #region General Methods
        void ControlManagerFocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control == null) return;
            Vector2 positionLeft = new Vector2(continueGame.position.X - ( (int)maxItemWidth >> 1) - 10f, control.position.Y);
            Vector2 positionRight = new Vector2(continueGame.position.X + maxItemWidth + 80f, control.position.Y);
            arrowImageLeft.SetPosition(positionLeft);
            arrowImageRight.SetPosition(positionRight);
        }

        private void MenuItemSelected(object sender, EventArgs e)
        {
            if (sender == continueGame)
            {
                stateManager.PopState();
            }
            if (sender == gameControls)
            {
                //TODO: Game Controls Screen. P = Pause, etc
            }
            if (sender == mainMenu)
            {
                stateManager.PopState();
                stateManager.PopState();
                stateManager.PopState();
                stateManager.PopState();
                stateManager.PushState(gameReference.startMenuScreen);
            }
        }
        #endregion
    }
}
