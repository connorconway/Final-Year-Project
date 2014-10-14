using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.Controls
{
    public class Label : Control
    {
        #region Constructor(s)
        public Label()
        {
            tabStop = false;
        }
        #endregion

        #region Abstract Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }
        #endregion
    }
}