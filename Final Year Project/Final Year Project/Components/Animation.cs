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

    public class Animation
    {
        private readonly Rectangle[] frames;
        private          int framesPerSecond;
        private          TimeSpan frameLength;
        private          TimeSpan frameTimer;
        private          int currentFrame;
        public           int frameWidth         { get; private set; }
        public           int frameHeight        { get; private set; }

        private int FramesPerSecond
        {
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

        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffSet, int yOffSet)
        {
            frames           = new Rectangle[frameCount];
            this.frameWidth  = frameWidth;
            this.frameHeight = frameHeight;

            for (var i = 0; i < frameCount; i++)
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
            frames          = animation.frames;
            FramesPerSecond = 5;
        }

        public void Update(GameTime gameTime)
        {
            frameTimer  += gameTime.ElapsedGameTime;
            if (frameTimer < frameLength)
                return;
            frameTimer   = TimeSpan.Zero;
            currentFrame = (currentFrame + 1) % frames.Length;
        }

        private void Reset()
        {
            currentFrame = 0;
            frameTimer   = TimeSpan.Zero;
        }

        public object Clone()
        {
            var animationClone = new Animation(this)
            {
                frameWidth  = frameWidth,
                frameHeight = frameHeight
            };
            animationClone.Reset();

            return animationClone;
        }
    }
}