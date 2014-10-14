using Final_Year_Project.Components;
using Final_Year_Project.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.TileEngine
{
    public enum CameraMode
    {
        Free,
        Follow
    }

    public class Camera
    {
        #region Variables
        Vector2 position;
        float speed;
        public float zoom { get; set; }
        Rectangle viewportRectangle;
        public CameraMode cameraMode { get; set; }
        #endregion

        #region Getter(s) and Setter(s)
        public Vector2 Position
        {
            get { return position; }
            private set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = MathHelper.Clamp(speed, 1f, 16f);
            }
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(zoom) *
                    Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }
        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                    viewportRectangle.X,
                    viewportRectangle.Y,
                    viewportRectangle.Width,
                    viewportRectangle.Height);
            }
        }
        #endregion

        #region Constructor(s)
        public Camera(Rectangle viewportRectangle)
        {
            speed = 4f;
            zoom = 1f;
            this.viewportRectangle = viewportRectangle;
            cameraMode = CameraMode.Follow;
        }

        public Camera(Rectangle viewportRectangle, Vector2 position)
        {
            speed = 4f;
            zoom = 1f;
            this.viewportRectangle = viewportRectangle;
            this.position = position;
            cameraMode = CameraMode.Follow;
        }
        #endregion

        #region General Methods
        public void Update(GameTime gameTime)
        {
            if (cameraMode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputHandler.KeyDown(Keys.Left) || InputHandler.ButtonDown(Buttons.RightThumbstickLeft, PlayerIndex.One))
                motion.X -= speed;
            else if (InputHandler.KeyDown(Keys.Right) || InputHandler.ButtonDown(Buttons.RightThumbstickRight, PlayerIndex.One))
                motion.X += speed;

            if (InputHandler.KeyDown(Keys.Up) || InputHandler.ButtonDown(Buttons.RightThumbstickUp, PlayerIndex.One))
                motion.Y -= speed;
            else if (InputHandler.KeyDown(Keys.Down) || InputHandler.ButtonDown(Buttons.RightThumbstickDown, PlayerIndex.One))
                motion.Y += speed;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();                                                                                                  //Turns the motion into a unit vector. The result is a vector one unit in length pointing in the same direction as the motion
                position += motion*speed;
                LockCamera();
            }
        }

        public void ZoomIn()
        {
            zoom += .25f;
            if (zoom > 2.5f)
                zoom = 2.5f;

            Vector2 newPosition = Position*zoom;
            SnapToPosition(newPosition);
        }

        public void ZoomOut()
        {
            zoom -= .25f;
            if (zoom < .5f)
                zoom = .5f;

            Vector2 newPosition = Position * zoom;
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

        public void ToggleCameraMode()
        {
            switch (cameraMode)
            {
                case CameraMode.Follow:
                    cameraMode = CameraMode.Free;
                    break;
                case CameraMode.Free:
                    cameraMode = CameraMode.Follow;
                    break;
            }
        }
        #endregion
    }
}