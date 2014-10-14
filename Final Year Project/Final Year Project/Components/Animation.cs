using System;
using Microsoft.Xna.Framework;

namespace Final_Year_Project.Components
{
    public enum AnimationKey
    {
        Down,
        Left,
        Right,
        Up
    }

    public class Animation : ICloneable
    {
        #region Variables
        private Rectangle[] frames;
        private int framesPerSecond;
        private TimeSpan frameLength;
        private TimeSpan frameTimer;
        private int currentFrame;
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        #endregion

        #region Getter(s) and Setter(s)
        private int FramesPerSecond
        {
            get { return framesPerSecond; }
            set
            {
                if (value < 1)
                    framesPerSecond = 1;
                else if (value > 60)
                    framesPerSecond = 60;
                else
                    framesPerSecond = value;
                frameLength = TimeSpan.FromSeconds(1 / (double)framesPerSecond);
            }
        }

        public Rectangle CurrentFrameRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
        }
        #endregion

        #region Constructor(s)
        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffSet, int yOffSet)
        {
            frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                frames[i] = new Rectangle(
                    xOffSet + (frameWidth * i),
                    yOffSet,
                    frameWidth,
                    frameHeight);
            }
            FramesPerSecond = 5;
            Reset();
        }

        private Animation(Animation animation)
        {
            frames = animation.frames;
            FramesPerSecond = 5;
        }
        #endregion

        #region General Method(s)
        public void Update(GameTime gameTime)
        {
            frameTimer += gameTime.ElapsedGameTime;
            if (frameTimer >= frameLength)
            {
                frameTimer = TimeSpan.Zero;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        private void Reset()
        {
            currentFrame = 0;
            frameTimer = TimeSpan.Zero;
        }

        public object Clone()
        {
            Animation animationClone = new Animation(this)
            {
                frameWidth = frameWidth,
                frameHeight = frameHeight
            };
            animationClone.Reset();

            return animationClone;
        }
        #endregion 
    }
}
