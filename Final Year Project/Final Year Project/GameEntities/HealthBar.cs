using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.GameEntities
{
    public class HealthBar : Sprite
    {
        private readonly Color color;
        public int currentHealth;

        public HealthBar(Texture2D sprite, Vector2 position, Color color)
        {
            this.sprite = sprite;
            this.position = position;
            this.color = color;
            currentHealth = 100;
        }

        public void Update(GameTime gameTime)
        {
            currentHealth = (int)MathHelper.Clamp(currentHealth, 0, 100);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 HUDpos)
        {

            spriteBatch.Draw(sprite,
               new Rectangle((int)HUDpos.X+120, (int)HUDpos.Y+55, sprite.Width / 3 + 19, 11),
               new Rectangle(0, 45, sprite.Width, 44),
               Color.Gray * 0.4f);

            spriteBatch.Draw(sprite,
                 new Rectangle((int)HUDpos.X+120, (int)HUDpos.Y+55, (int)(sprite.Width  * ((double)currentHealth / 100)) / 3 + 19, 11),
                 new Rectangle(0, 45, sprite.Width, 44),
                 color * 0.4f);

            spriteBatch.Draw(sprite,
                new Rectangle((int)HUDpos.X+120, (int)HUDpos.Y+55, sprite.Width  / 3 + 19, 11),
                new Rectangle(0, 0, sprite.Width, 44),
                Color.White * 0.7f);
        }
    }
}
