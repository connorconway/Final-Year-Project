using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Multiplayer_Software_Game_Engineering.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiplayer_Software_Game_Engineering.Controls
{
    public sealed class ListBox : Control
    {
        #region Variables
        public event EventHandler selectionChanged;
        public event EventHandler enter;
        public event EventHandler leave;
        List<string> items = new List<string>();
        int startItem;
        int lineCount;
        Texture2D background;
        Texture2D cursor;
        Color selectedColor = Color.White;
        int selectedItem;
        #endregion

        #region Getter(s) and Setter(s)
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return selectedItem; }
            set { selectedItem = (int)MathHelper.Clamp(value, 0f, items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[selectedItem]; }
        }

        public List<string> Items
        {
            get { return items; }
        }

        public override bool HasFocus
        {
            get { return hasFocus; }
            set
            {
                hasFocus = value;
                if (hasFocus)
                    OnEnter(null);
                else
                    OnLeave(null);
            }
        }
        #endregion

        #region Constructor(s)
        public ListBox(Texture2D background, Texture2D cursor)
            : base()
        {
            hasFocus = false;
            tabStop = false;
            this.background = background;
            this.cursor = cursor;
            size = new Vector2(background.Width, background.Height);
            lineCount = background.Height / spriteFont.LineSpacing;
            startItem = 0;
            color = Color.Black;
        }
        #endregion

        #region Override Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, Color.White);

            for (int i = 0; i < lineCount; i++)
            {
                if (startItem + i >= items.Count)
                    break;

                if (startItem + i == selectedItem)
                {
                    spriteBatch.DrawString(
                        spriteFont,
                        items[startItem + i],
                        new Vector2(
                            position.X,
                            position.Y + i*spriteFont.LineSpacing),
                            SelectedColor);

                    spriteBatch.Draw(
                        cursor,
                        new Vector2(
                            position.X - (cursor.Width + 2),
                            position.Y + i*spriteFont.LineSpacing + 5),
                            Color.White);
                }
                else
                {
                    spriteBatch.DrawString(
                        spriteFont,
                        items[startItem + i],
                        new Vector2(
                            position.X,
                            2 + position.Y + i*spriteFont.LineSpacing),
                            color);
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!HasFocus)
                return;

            if (InputHandler.KeyReleased(Keys.Down) || InputHandler.ButtonReleased(Buttons.LeftThumbstickDown, playerIndex))
            {
                if (selectedItem < items.Count - 1)
                {
                    selectedItem++;
                    if (selectedItem >= startItem + lineCount)
                        startItem = selectedItem - lineCount + 1;
                    OnSelectionChanged(null);
                }
            }

            else if (InputHandler.KeyReleased(Keys.Up) || InputHandler.ButtonReleased(Buttons.LeftThumbstickUp, playerIndex))
            {
                if (selectedItem > 0)
                {
                    selectedItem--;
                    if (selectedItem < startItem)
                        startItem = selectedItem;
                    OnSelectionChanged(null);
                }
            }

            if (InputHandler.KeyReleased(Keys.Enter) || InputHandler.ButtonReleased(Buttons.A, playerIndex))
            {
                HasFocus = false;
                OnSelected(null);
            }

            if (InputHandler.KeyReleased(Keys.Escape) || InputHandler.ButtonReleased(Buttons.B, playerIndex))
            {
                HasFocus = false;
            }
        }
        #endregion

        #region General Methods
        private void OnSelectionChanged(EventArgs e)
        {
            if (selectionChanged != null)
                selectionChanged(this, e);
        }

        private void OnEnter(EventArgs e)
        {
            if (enter != null)
                enter(this, e);
        }

        private void OnLeave(EventArgs e)
        {
            if (leave != null)
                leave(this, e);
        }
        #endregion
    }
}
