using System.Collections.Generic;
using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.Components
{
    public class AnimatedSprite : Sprite
    {
        private readonly Dictionary<AnimationKey, Animation> animations;
        public           AnimationKey                        currentAnimation { get; set; }
        public           bool                                isAnimating      { get; set; }

        public int Width
        {
            get { return animations[currentAnimation].frameWidth; }
        }

        public int Height
        {
            get { return animations[currentAnimation].frameHeight; }
        }

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            this.sprite = sprite;
            animations  = new Dictionary<AnimationKey, Animation>();

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
