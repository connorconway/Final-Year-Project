using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Multiplayer_Software_Game_Engineering.Handlers
{
    class InputHandler : GameComponent
    {
        private static KeyboardState  currKeyboardState { get; set; }
        private static KeyboardState  prevKeyboardState { get; set; }
        private static GamePadState[] currGamePadState  { get; set; }
        private static GamePadState[] prevGamePadState  { get; set; }
        private static int            currScrollValue   { get; set; }
        private static int            prevScrollValue   { get; set; }

        public InputHandler(Game game) : base(game)
        {
            currKeyboardState = Keyboard.GetState();
            currGamePadState  = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                currGamePadState[(int)index] = GamePad.GetState(index);

            currScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyboardState = currKeyboardState;
            currKeyboardState = Keyboard.GetState();
            prevGamePadState  = (GamePadState[])currGamePadState.Clone();

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                currGamePadState[(int)index] = GamePad.GetState(index);

            prevScrollValue = currScrollValue;
            currScrollValue = Mouse.GetState().ScrollWheelValue;

            base.Update(gameTime);
        }

        public static void Flush()
        {
            prevKeyboardState = currKeyboardState;
        }

        public static int Scroll(MouseState mouseState)
        {
            currScrollValue = mouseState.ScrollWheelValue;
            if (prevScrollValue < currScrollValue)
                return 1;                                               // Scroll Up
            if (prevScrollValue > currScrollValue)
                return -1;                                              // Scroll Down
            return 0;                                                   // Do Not Scroll
        }

        public static bool KeyReleased(Keys key)
        {
            return currKeyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return currKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return currKeyboardState.IsKeyDown(key);
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonUp(button) && prevGamePadState[(int)index].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonDown(button) && prevGamePadState[(int)index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonDown(button);
        }
    }
}