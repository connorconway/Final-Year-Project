using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.Components
{
    public class TextBox : Sprite
    {
        public TextBoxExists textBoxExists;
        private const float AlphaTime = 1000;                                          // total animate time (at milliseconds)
        float AlphaTimeSubtract = 1000.0f;                                             // at milliseconds
        public bool decreaseAlpha;
        protected Color color = Color.White;
        protected Color fontColor = Color.Black;
        SpriteFont font;
        private string text;
        private float size;
        private float opacity;

        public enum TextBoxExists
        {
            Opaque,
            Transparent
        }

        public TextBox(Texture2D sprite, Vector2 position, SpriteFont font, String text, float size)
        {
            decreaseAlpha = false;
            this.sprite = sprite;
            textBoxExists = TextBoxExists.Opaque;
            this.position = position;
            this.font = font;
            this.text = text;
            this.size = size;
            opacity = 1.0f;
        }

        public void setTextBoxExists(TextBoxExists textBoxExists)
        {
            this.textBoxExists = textBoxExists;
        }

        public void setOpacity(float opacity)
        {
            this.opacity = opacity;
        }

        public void setText(String text)
        {
            this.text = text;
        }

        public void setAlphaTimeSubtract(float alpha)
        {
            this.AlphaTimeSubtract = alpha;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            if (decreaseAlpha == false && textBoxExists == TextBoxExists.Opaque)
            {
                AlphaTimeSubtract -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
                color = Color.White * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                fontColor = Color.Black * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                if (AlphaTimeSubtract <= 0)
                {
                    textBoxExists = TextBoxExists.Transparent;
                }
            }
            else if (decreaseAlpha && textBoxExists == TextBoxExists.Transparent)
            {
                AlphaTimeSubtract += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
                color = Color.White * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                fontColor = Color.Black* MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
                if (AlphaTimeSubtract >= 1000)
                {
                    textBoxExists = TextBoxExists.Opaque;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color * opacity, 0, new Vector2(0, 0), size, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, new Vector2(position.X + 20, position.Y + 20), fontColor * opacity);
        }

    
    }
}
