using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.Controls
{
    public class PictureBox : Control
    {
        public Texture2D texture { private get; set; }
        Rectangle sourceRect { get; set; }
        Rectangle destRect { get; set; }

        public PictureBox(Texture2D texture, Rectangle destRect)
        {
            this.texture = texture;
            this.destRect = destRect;
            spriteEffect = SpriteEffects.None;
            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            color = Color.White;
        }

        public PictureBox(Texture2D texture, Rectangle destRect, Rectangle sourceRect)
        {
            this.texture = texture;
            this.destRect = destRect;
            this.sourceRect = sourceRect;
            spriteEffect = SpriteEffects.None;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, color, 0, new Vector2(0,0), spriteEffect, 0);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        public void SetPosition(Vector2 newPosition)
        {
            destRect = new Rectangle(
            (int)newPosition.X,
            (int)newPosition.Y,
            sourceRect.Width,
            sourceRect.Height);
        }

        public void setSpriteEffect(SpriteEffects spriteEffect)
        {
            this.spriteEffect = spriteEffect;
        }
    }
}