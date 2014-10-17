using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Final_Year_Project.Components;
using Final_Year_Project.Handlers;
using Final_Year_Project.Networking;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.GameStates
{
    public class GamePlayScreen : BaseGameState
    {
        #region Variables
        private Engine engine = new Engine(32, 32);
        public static World world { get; set; }
        
        
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        
        #endregion

        #region Constructor(s)
        public GamePlayScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
            AllocConsole();  
            world = new World(game, gameReference.screenRectangle);
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.P))
            {
                stateManager.PushState(gameReference.pauseScreen);
            }

            
            world.Update(gameTime);
            foreach (var onlinePlayers in ServerMain.playersInServer.Where(onlinePlayers => onlinePlayers.Key == clientID))
            {
                onlinePlayers.Value.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var onlinePlayers in ServerMain.playersInServer)
            {
                if (onlinePlayers.Key == clientID)
                {
                    gameReference.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                    null, null, null, onlinePlayers.Value.camera.Transformation);
                }

                world.DrawLevel(gameReference.spriteBatch, onlinePlayers.Value.camera);
                    onlinePlayers.Value.Draw(gameTime, gameReference.spriteBatch);
            }
            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }
        #endregion
    }
}
