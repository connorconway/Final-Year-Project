using Multiplayer_Software_Game_Engineering.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Multiplayer_Software_Game_Engineering.Controls
{
    public class ControlManager : List<Control>
    {
        #region Variables
        int selectedControl;
        public static SpriteFont spriteFont { get; private set; }
        public event EventHandler focusChanged;
        bool acceptInput = true;
        #endregion

        #region Getter(s) and Setter(s)
        public bool AcceptInput
        {
            get { return acceptInput; }
            set { acceptInput = value; }
        }
        #endregion

        #region Constructor(s)
        public ControlManager(SpriteFont spriteFont)
        {
            ControlManager.spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            ControlManager.spriteFont = spriteFont;
        }

        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection)
            : base(collection)
        {
            ControlManager.spriteFont = spriteFont;
        }
        #endregion

        #region General Methods
        public void Update(GameTime gameTime, PlayerIndex playerIndex)
        {
            if (Count == 0)
                return;

            foreach (Control c in this.Where(c => c.enabled))
            {
                c.Update(gameTime);
            }

            foreach (Control c in this.Where(c => c.HasFocus))
            {
                c.HandleInput(playerIndex);
                break;
            }

            if (!AcceptInput)
                return;

            if (InputHandler.ButtonPressed(Buttons.LeftThumbstickUp, playerIndex) ||
                InputHandler.ButtonPressed(Buttons.DPadUp, playerIndex) ||
                InputHandler.KeyPressed(Keys.Up))
                PreviousControl();

            if (InputHandler.ButtonPressed(Buttons.LeftThumbstickDown, playerIndex) ||
                InputHandler.ButtonPressed(Buttons.DPadDown, playerIndex) ||
                InputHandler.KeyPressed(Keys.Down))
                NextControl();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Control c in this.Where(c => c.visible))
            {
                c.Draw(spriteBatch);
            }
        }

        public void NextControl()
        {
            if (Count == 0)
                return;
            int currentControl = selectedControl;
            this[selectedControl].HasFocus = false;
            do
            {
                selectedControl++;
                if (selectedControl == Count)
                    selectedControl = 0;
                if (this[selectedControl].tabStop && this[selectedControl].enabled)
                {
                    if (focusChanged != null)
                        focusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);
            this[selectedControl].HasFocus = true;
        }

        public void PreviousControl()
        {
            if (Count == 0)
                return;
            int currentControl = selectedControl;
            this[selectedControl].HasFocus = false;
            do
            {
                selectedControl--;
                if (selectedControl < 0)
                    selectedControl = Count - 1;
                if (this[selectedControl].tabStop && this[selectedControl].enabled)
                {
                    if (focusChanged != null)
                        focusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);
            this[selectedControl].HasFocus = true;
        }
        #endregion
    }
}
