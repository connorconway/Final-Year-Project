using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_Year_Project.GameStates
{
    public abstract partial class GameState : DrawableGameComponent
    {
        #region Variables
        List<GameComponent> childComponents { get; set; }
        GameState tag { get; set; }
        protected GameStateManager stateManager;
        #endregion

        #region Constructor(s)
        protected GameState(Game game, GameStateManager stateManager) : base(game)
        {
            this.stateManager = stateManager;
            childComponents = new List<GameComponent>();
            tag = this;
        }
        #endregion

        #region Override Methods
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

        #endregion

        #region General Methods
        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (stateManager.CurrentState == tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }
        #endregion
    }
}
