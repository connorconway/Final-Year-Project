using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.TileEngine;

namespace Multiplayer_Software_Game_Engineering.GameEntities
{
    public class AnimatedSprite : Sprite
    {
        private readonly Dictionary<Constants.Direction, Animation> animations;
        public           Constants.Direction                        currentAnimation { get; set; }
        public           bool                                       isAnimating      { get; set; }

        public int Width
        {
            get { return animations[currentAnimation].frameWidth; }
        }

        public int Height
        {
            get { return animations[currentAnimation].frameHeight; }
        }

        public AnimatedSprite(Texture2D sprite, Dictionary<Constants.Direction, Animation> animation)
        {
            this.sprite = sprite;
            animations  = new Dictionary<Constants.Direction, Animation>();

            foreach (var key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());

            boundingBox = new Rectangle((int)position.X, (int)position.Y, sprite.Width/3, sprite.Height/4);
        }

        public void Update(GameTime gameTime)
        {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;

            if (isAnimating)
                animations[currentAnimation].Update(gameTime); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                sprite,
                position,
                animations[currentAnimation].CurrentFrameRect,
                Color.White);
        }

        public void LockToMap()
        {
            position.X = MathHelper.Clamp(position.X, 0, TileMap.MapWidth - Width);
            position.Y = MathHelper.Clamp(position.Y, 0, TileMap.MapHeight - Height);
        }
    }
}
