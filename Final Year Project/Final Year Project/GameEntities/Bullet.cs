using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.TileEngine;

namespace Multiplayer_Software_Game_Engineering.GameEntities
{
    public enum BulletLife
    {
        Alive,
        Dead
    }

    public class Bullet : Sprite
    {
        private new Vector2 motion;
        private readonly Constants.Direction spriteFacing;
        public BulletLife bulletLife;

        public Bullet(Texture2D sprite, Vector2 position, string spriteFacing, Vector2 motion)
        {
            this.sprite = sprite;
            this.position = position;
            speed = 7.0f;
            rotation = 0.0f;
            this.motion = motion;
            this.spriteFacing = (Constants.Direction)Enum.Parse(typeof(Constants.Direction), spriteFacing, true);
            bulletLife = BulletLife.Alive;
            boundingBox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public void Update(GameTime gameTime, MapLayer tileMap)
        {
            if (bulletLife != BulletLife.Alive)
                return;

            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;

            if (motion == Vector2.Zero)
            {
                if (spriteFacing == Constants.Direction.Down)
                {
                    motion.Y = 1;
                    rotation = 1.57f;
                }
                if (spriteFacing == Constants.Direction.Left)
                {
                    motion.X = -1;
                    rotation = MathHelper.Pi;
                }
                if (spriteFacing == Constants.Direction.Right)
                {
                    motion.X = 1;
                }
                if (spriteFacing == Constants.Direction.Up)
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

            checkIfHitObject(tileMap, (int)position.X , (int)position.Y );

        }

        public void checkIfHitObject(MapLayer tilemap, int xDirection, int yDirection)
        {
            int result = tilemap.isPassable((xDirection) / 32, (yDirection+(sprite.Height)) / 32);
            if (result == 0)
                bulletLife = BulletLife.Dead;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (bulletLife != BulletLife.Alive)
                return;
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(0,0), 1.0f, SpriteEffects.None, 0 );
        }
    }
}