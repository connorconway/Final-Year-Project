using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.Components
{
    public class Sprite
    {
        protected Texture2D sprite;
        protected Vector2 position;
        protected Vector2 velocity;
        protected float speed = 2.0f;
        public string textTexture { get; set; }
        protected Rectangle spriteRectangle;
        public Vector2 motion;
        public float rotation;

        public Rectangle boundingBox;


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

    }
}
