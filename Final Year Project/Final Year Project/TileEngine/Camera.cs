using Microsoft.Xna.Framework;
using Multiplayer_Software_Game_Engineering.GameEntities;

namespace Multiplayer_Software_Game_Engineering.TileEngine
{
    public class Camera
    {
        public      Vector2       position;
        public      float         zoom                  { get; private set; }
        public      Rectangle     viewportRectangle     { get; private set; }

        public Matrix Transformation
        {
            get { return Matrix.CreateScale(zoom) * Matrix.CreateTranslation(new Vector3(-position, 0f)); }
        }

        public Camera(Rectangle viewportRectangle)
        {
            zoom                    = 1f;
            this.viewportRectangle  = viewportRectangle;
        }

        public void Zoom(float amount)
        {
            zoom += amount;

            if (zoom > 2.5f)
                zoom = 2.5f;
            else if (zoom < 0.5f)
                zoom = 0.5f;

            var newPosition = position * zoom;
            SnapToPosition(newPosition);
        }

        private void SnapToPosition(Vector2 newPosition)
        {
            position.X = newPosition.X - viewportRectangle.Width / 2f;
            position.Y = newPosition.Y - viewportRectangle.Height / 2f;
            LockCamera();
        }

        private void LockCamera()
        {
            position.X = MathHelper.Clamp(position.X, 0, TileMap.MapWidth*zoom - viewportRectangle.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, TileMap.MapHeight*zoom - viewportRectangle.Height);
        }

        public void LockToSprite(AnimatedSprite sprite)
        {
            position.X = (sprite.Position.X + sprite.Width / 2f)*zoom - (viewportRectangle.Width / 2f);
            position.Y = (sprite.Position.Y + sprite.Height / 2f)*zoom - (viewportRectangle.Height / 2f);
            LockCamera();
        } 
    }
}