﻿using System;
using System.Collections.Generic;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiplayer_Software_Game_Engineering.Components
{
    public class Player
    {
        #region Variables
        public Camera camera { get; private set; }
        Game1 gameReference;
        public AnimatedSprite animatedSprite { get; set; }
        public Vector2 motion;
        public List<Bullet> bullets;

        public Texture2D bulletSprite;
        private TimeSpan bulletTimer;
        private float shotSeconds;
        public Boolean createBullet;
        public Vector2 playerOrigin;
        public HealthBar playerHealth;
        private bool isHost;
        #endregion

        #region Constructor(s)
        public Player(Game gameReference, AnimatedSprite animatedSprite, Texture2D bulletSprite, Texture2D healthBarSprite, Color color)
        {
            Random rand = new Random();

            this.gameReference = (Game1)gameReference;
            camera = new Camera(this.gameReference.screenRectangle);
            this.animatedSprite = animatedSprite;
            bullets = new List<Bullet>(10);
            shotSeconds = 5;
            bulletTimer = TimeSpan.FromSeconds(shotSeconds);
            this.bulletSprite = bulletSprite;
            playerHealth = new HealthBar(healthBarSprite, animatedSprite.Position, color);
            animatedSprite.position.X = rand.Next(50, 400); 
            animatedSprite.position.Y = rand.Next(50, 400);
            isHost = false;
        }
        #endregion

        #region General Methods
        public void UpdateHealthBar(GameTime gameTime)
        {
            playerHealth.position.X = animatedSprite.Position.X - animatedSprite.Width / 2;
            playerHealth.position.Y = animatedSprite.Position.Y - 10;
        }
        public void Update(GameTime gameTime)
        {
            createBullet = false;
            playerOrigin = new Vector2(animatedSprite.Position.X + animatedSprite.Width / 2, animatedSprite.Position.Y + animatedSprite.Height / 2);
            animatedSprite.Update(gameTime);
            
            if (InputHandler.KeyReleased(Keys.Z) || InputHandler.Scroll(Mouse.GetState()) == 1 ||
                InputHandler.ButtonReleased(Buttons.LeftShoulder, PlayerIndex.One))
            {
                camera.Zoom(0.25f);
                camera.LockToSprite(animatedSprite);
            }

            else if (InputHandler.KeyReleased(Keys.X) || InputHandler.Scroll(Mouse.GetState()) == -1 ||
                     InputHandler.ButtonReleased(Buttons.RightShoulder, PlayerIndex.One))
            {
                camera.Zoom(-0.25f);
                camera.LockToSprite(animatedSprite);
            }
            motion = new Vector2();
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
                camera.LockToSprite(animatedSprite);
            }
            else
            {
                animatedSprite.isAnimating = false;
            }
            if (InputHandler.KeyPressed(Keys.Space) ||
                     InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
            {
                bullets.Add(new Bullet(bulletSprite, playerOrigin, animatedSprite.currentAnimation.ToString(), motion));
                createBullet = true;
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
            }

            UpdateHealthBar(gameTime);

            playerHealth.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animatedSprite.Draw(spriteBatch);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }
            playerHealth.Draw(gameTime, spriteBatch);
        }

        public bool getHost()
        {
            return isHost;
        }
        public void setHost(bool host)
        {
            isHost = host;
        }
        #endregion
    }
}
