using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.GameEntities
{
    public class HUD : Sprite
    {
        protected Color color = Color.White;
        protected Color fontColor = Color.Black;
        SpriteFont font;
        public int level, exp, gold;
        public string type;
        private float size;
        private float opacity;

        public HUD(Texture2D sprite, Vector2 position, SpriteFont font, int level, int exp, int gold, String type, float size)
        {
            this.sprite = sprite;
            this.position = position;
            this.font = font;
            this.level = level;
            this.exp = exp;
            this.gold = gold;
            this.type = type;
            this.size = size;
            opacity = 1.0f;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
           
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D characterImage, HealthBar playerHealth)
        {
            spriteBatch.Draw(sprite, position, null, color * opacity, 0, new Vector2(0, 0), size, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, level.ToString(), new Vector2(position.X + 325, position.Y + 60), fontColor * opacity);
            spriteBatch.DrawString(font, type, new Vector2(position.X + 177, position.Y + 26), fontColor * opacity);
            spriteBatch.DrawString(font, gold.ToString(), new Vector2(position.X + 388, position.Y + 50), fontColor * opacity);
            spriteBatch.DrawString(font, "Gold", new Vector2(position.X + 370, position.Y + 27), fontColor * opacity);
            spriteBatch.Draw(characterImage, new Vector2(position.X + 40, position.Y + 35), new Rectangle(0, 0, 32, 32), Color.White);
            playerHealth.Draw(gameTime, spriteBatch, position);
        }
    }
}
