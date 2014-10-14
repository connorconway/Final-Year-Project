using System.Collections.Generic;
using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.Components
{
    public class AnimatedSprite
    {
        #region Variables
        private Dictionary<AnimationKey, Animation> animations;
        public AnimationKey currentAnimation { get; set; }
        public bool isAnimating { get; set; }
        private Texture2D sprite;
        Vector2 position;
        private Vector2 velocity;
        private float speed = 2.0f;
        #endregion

        #region Getter(s) and Setter(s)
        public int Width
        {
            get { return animations[currentAnimation].frameWidth; }
        }

        public int Height
        {
            get { return animations[currentAnimation].frameHeight; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = MathHelper.Clamp(speed, 1.0f, 16.0f); }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                if (velocity != Vector2.Zero)
                    velocity.Normalize();
            }
        }
        #endregion

        #region Constructor(s)
        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            this.sprite = sprite;
            animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }
        #endregion

        #region General Method(s)
        public void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
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

        #endregion
    }
}
