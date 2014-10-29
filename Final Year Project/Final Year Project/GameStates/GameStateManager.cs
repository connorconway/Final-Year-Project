using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
   public class GameStateManager : GameComponent
    {
        #region Variables
        public event EventHandler onStateChange;
        Stack<GameState> gameStates = new Stack<GameState>();
        const int startDrawOrder = 5000;
        const int drawOrderIncrement = 100;
        int drawOrder;
        #endregion

        #region Getters/Setters
        public GameState CurrentState
        {
            get { return gameStates.Peek(); }
        }
        #endregion

        #region Constructor(s)
        public GameStateManager(Game game) : base(game)
        {
            drawOrder = startDrawOrder;
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region General Methods
        public void PopState()
        {
            if (gameStates.Count > 0)
            {
                RemoveState();
                drawOrder -= drawOrderIncrement;
                if (onStateChange != null)
                    onStateChange(this, null);
            }
        }

        private void RemoveState()
        {
            GameState State = gameStates.Peek();
            onStateChange -= State.StateChange;
            Game.Components.Remove(State);
            gameStates.Pop();
        }

        public void PushState(GameState newState)
        {
            drawOrder += drawOrderIncrement;
            newState.DrawOrder = drawOrder;
            AddState(newState);
            if (onStateChange != null)
                onStateChange(this, null);
        }

        private void AddState(GameState newState)
        {
            gameStates.Push(newState);
            Game.Components.Add(newState);
            onStateChange += newState.StateChange;
        }

        public void ChangeState(GameState newState)
        {
            while (gameStates.Count > 0)
                RemoveState();
            newState.DrawOrder = startDrawOrder;
            drawOrder = startDrawOrder;
            AddState(newState);
            if (onStateChange != null)
                onStateChange(this, null);
        }
        #endregion
    }
}
