using Final_Year_Project.Handlers;
using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.Components
{
    public class Player
    {
        #region Variables
        public Camera camera { get; private set; }
        Game1 gameReference;
        AnimatedSprite animatedSprite { get; set; }
        #endregion

        #region Constructor(s)
        public Player(Game gameReference, AnimatedSprite animatedSprite)
        {
            this.gameReference = (Game1)gameReference;
            camera = new Camera(this.gameReference.screenRectangle);
            this.animatedSprite = animatedSprite;
        }
        #endregion

        #region General Methods

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            animatedSprite.Update(gameTime);

            if (InputHandler.KeyReleased(Keys.Z) || InputHandler.scrollUp(Mouse.GetState()) == 1 ||
                InputHandler.ButtonReleased(Buttons.LeftShoulder, PlayerIndex.One))
            {
                camera.ZoomIn();
                if (camera.cameraMode == CameraMode.Follow)
                    camera.LockToSprite(animatedSprite);
            }

            else if (InputHandler.KeyReleased(Keys.X) || InputHandler.scrollUp(Mouse.GetState()) == -1 ||
                     InputHandler.ButtonReleased(Buttons.RightShoulder, PlayerIndex.One))
            {
                camera.ZoomOut();
                if (camera.cameraMode == CameraMode.Follow)
                    camera.LockToSprite(animatedSprite);
            }
            Vector2 motion = new Vector2();
            if (InputHandler.KeyDown(Keys.W) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickUp, PlayerIndex.One))
            {
                animatedSprite.currentAnimation = AnimationKey.Up;
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S) ||
                     InputHandler.ButtonDown(Buttons.LeftThumbstickDown, PlayerIndex.One))
            {
                animatedSprite.currentAnimation = AnimationKey.Down;
                motion.Y = 1;
            }
            if (InputHandler.KeyDown(Keys.A) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, PlayerIndex.One))
            {
                animatedSprite.currentAnimation = AnimationKey.Left;
                motion.X = -1;
            }
            if (InputHandler.KeyDown(Keys.D) ||
                     InputHandler.ButtonDown(Buttons.LeftThumbstickRight, PlayerIndex.One))
            {
                animatedSprite.currentAnimation = AnimationKey.Right;
                motion.X = 1;
            }
            if (motion != Vector2.Zero)
            {
                animatedSprite.isAnimating = true;
                motion.Normalize();
                animatedSprite.Position += motion * animatedSprite.Speed;
                animatedSprite.LockToMap();
                if (camera.cameraMode == CameraMode.Follow)
                    camera.LockToSprite(animatedSprite);
            }
            else
            {
                animatedSprite.isAnimating = false;
            }
        }
  

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animatedSprite.Draw(gameTime, spriteBatch, camera);
        }
        #endregion
    }
}
