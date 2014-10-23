using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.Components
{
    enum BulletDirection
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3
    }

    public class Bullet : Sprite
    {
        private Vector2 motion;
        private BulletDirection spriteFacing;

        public Bullet(Texture2D sprite, Vector2 position, string spriteFacing, Vector2 motion)
        {
            this.sprite = sprite;
            this.position = position;
            speed = 7.0f;
            rotation = 0.0f;
            this.motion = motion;
            this.spriteFacing = (BulletDirection) Enum.Parse(typeof(BulletDirection), spriteFacing, true);
            boundingBox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public void Update(GameTime gameTime)
        {
            boundingBox.X = (int) position.X;
            boundingBox.Y = (int) position.Y;

            if (motion == Vector2.Zero)
            {
                if (spriteFacing == BulletDirection.Down)
                {
                    motion.Y = 1;
                    rotation = 1.57f;
                }
                if (spriteFacing == BulletDirection.Left)
                {
                    motion.X = -1;
                    rotation = MathHelper.Pi;
                }
                if (spriteFacing == BulletDirection.Right)
                {
                    motion.X = 1;
                }
                if (spriteFacing == BulletDirection.Up)
                {
                    motion.Y = -1;
                    rotation = -1.57f;
                }
            }
            else
            {
                if (motion.X > 0 && motion.Y > 0)
                {
                    rotation = 0.79f;
                }
                if (motion.X > 0 && motion.Y < 0)
                {
                    rotation = -0.79f;
                }
                if (motion.X < 0 && motion.Y < 0)
                {
                    rotation = -2.36f;
                }
                if (motion.X < 0 && motion.Y > 0)
                {
                    rotation = 2.36f;
                }
                else if (motion.Y == 1)
                {
                    rotation = 1.57f;
                }
                else if (motion.Y == -1)
                {
                    rotation = -1.57f;
                }
                else if (motion.X == -1)
                {
                    rotation = MathHelper.Pi;
                }    
            }
            
            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                position += motion * Speed;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(0,0), 1.0f, SpriteEffects.None, 0 );
        }
    }
}
