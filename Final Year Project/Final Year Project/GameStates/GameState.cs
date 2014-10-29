using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public abstract class GameState : DrawableGameComponent
    {
        protected readonly GameStateManager    stateManager;
        private            List<GameComponent> childComponents { get; set; }
        private            GameState           gameState       { get; set; }

        protected GameState(Game game, GameStateManager stateManager) : base(game)
        {
            this.stateManager = stateManager;
            childComponents   = new List<GameComponent>();
            gameState = this;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents.Where(component => component.Enabled))
            {
                component.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (DrawableGameComponent component in childComponents.OfType<DrawableGameComponent>().Where(component => component.Visible))
            {
                component.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        internal protected void StateChange(object sender, EventArgs e)
        {
            if (stateManager.CurrentState == gameState)
                Show();
            else
                Hide();
        }

        private void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;
                var gameComponent = component as DrawableGameComponent;
                if (gameComponent != null)
                    gameComponent.Visible = true;
            }
        }

        private void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;
                var gameComponent = component as DrawableGameComponent;
                if (gameComponent != null)
                    gameComponent.Visible = false;
            }
        }
    }
}