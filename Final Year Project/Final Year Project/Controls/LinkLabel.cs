using Final_Year_Project.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.Controls
{
    public class LinkLabel : Control
    {
        #region Variables
        Color selectedColor { get; set; }
        #endregion

        #region Contructor(s)
        public LinkLabel()
        {
            selectedColor = Color.Red;
            tabStop = true;
            hasFocus = false;
            position = Vector2.Zero;
        }
        #endregion

        #region Abstract Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position, hasFocus ? selectedColor : color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!hasFocus)
                return;
            if (InputHandler.KeyReleased(Keys.Enter) ||
            InputHandler.ButtonReleased(Buttons.A, playerIndex))
                base.OnSelected(null);
        }
        #endregion
    }
}
