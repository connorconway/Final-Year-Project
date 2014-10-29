using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
   public class GameStateManager : GameComponent
    {
        public event EventHandler onStateChange;
        private readonly Stack<GameState> gameStates = new Stack<GameState>();
        private const int startDrawOrder = 5000;
        private const int drawOrderIncrement = 100;
        private int drawOrder;

        public GameState CurrentState
        {
            get { return gameStates.Peek(); }
        }

        public GameStateManager(Game game) : base(game)
        {
            drawOrder = startDrawOrder;
        }

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
    }
}