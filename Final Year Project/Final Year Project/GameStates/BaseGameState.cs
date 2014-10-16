using Final_Year_Project.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.GameStates
{
    public abstract partial class BaseGameState : GameState
    {
        #region Variables
        protected readonly Game1 gameReference;
        protected ControlManager controlManager;
        protected PlayerIndex playerIndexInControl;

        protected static Texture2D backgroundImage;
        protected static Texture2D backgroundBorder;
        protected static Color color = Color.White;
        private const float AlphaTime = 3500f;                                          // total animate time (at milliseconds)
        static float AlphaTimeSubtract = 500.0f;                                        // at milliseconds
        static private bool increaseAlpha;

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
    }
}
