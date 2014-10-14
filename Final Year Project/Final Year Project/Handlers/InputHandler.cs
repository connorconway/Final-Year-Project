using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Final_Year_Project.Handlers
{
    class InputHandler : GameComponent
    {
        #region Variables
        static KeyboardState currKeyboardState { get; set; }
        static KeyboardState prevKeyboardState { get; set; }
        static GamePadState[] currGamePadState { get; set; }
        static GamePadState[] prevGamePadState { get; set; }
        static int currScrollValue {get; set;}
        static int prevScrollValue {get; set;}
        #endregion

        #region Constructor(s)
        public InputHandler(Game game)
            : base(game)
        {
            currKeyboardState = Keyboard.GetState();
            currGamePadState = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                currGamePadState[(int)index] = GamePad.GetState(index);

            currScrollValue = Mouse.GetState().ScrollWheelValue;
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            prevKeyboardState = currKeyboardState;
            currKeyboardState = Keyboard.GetState();

            prevGamePadState = (GamePadState[])currGamePadState.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                currGamePadState[(int)index] = GamePad.GetState(index);

            prevScrollValue = currScrollValue;
            currScrollValue = Mouse.GetState().ScrollWheelValue;

            base.Update(gameTime);
        }
        #endregion

        #region General Methods
        public static void Flush()
        {
            prevKeyboardState = currKeyboardState;
        }

        public static int scrollUp(MouseState mouseState)
        {
            currScrollValue = mouseState.ScrollWheelValue;
            if (prevScrollValue < currScrollValue)
                return 1;
            if (prevScrollValue > currScrollValue)
                return -1;
            return 0;
        }

        public static bool KeyReleased(Keys key)
        {
            return currKeyboardState.IsKeyUp(key) &&
            prevKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return currKeyboardState.IsKeyDown(key) &&
            prevKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return currKeyboardState.IsKeyDown(key);
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonUp(button) &&
            prevGamePadState[(int)index].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonDown(button) &&
            prevGamePadState[(int)index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return currGamePadState[(int)index].IsButtonDown(button);
        }

        #endregion
    }
}
