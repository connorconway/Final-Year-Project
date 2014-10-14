using System;
using System.Collections.Generic;
using Final_Year_Project.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.Controls
{
    public class LeftRightSelector : Control
    {
        #region Variables
        public event EventHandler selectionChanged;
        List<String> items = new List<string>();
        private Texture2D leftArrow;
        private Texture2D rightArrow;
        private Texture2D stopTexture;
        private Color selectedColor = Color.Red;
        private int maxItemWidth;
        public int selectedItem;
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
        #endregion

        #region Constructor(s)
        public LeftRightSelector(Texture2D leftArrow, Texture2D rightArrow, Texture2D stopTexture)
        {
            this.leftArrow = leftArrow;
            this.rightArrow = rightArrow;
            this.stopTexture = stopTexture;
            tabStop = true;
            color = Color.White;
        }
        #endregion

        #region General Methods
        public void SetItems(IEnumerable<string> theItems, int maxWidth)
        {
            items.Clear();
            foreach (string s in theItems)
                items.Add(s);
            maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged()
        {
            if (selectionChanged != null)
            {
                selectionChanged(this, null);
            }
        }
        #endregion

        #region Override Method(s)
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = position;
            spriteBatch.Draw(selectedItem != 0 ? leftArrow : stopTexture, pos, Color.White);
            pos.X += leftArrow.Width + 5f;
            float itemWidth = spriteFont.MeasureString(items[selectedItem]).X;
            float offset = (maxItemWidth - itemWidth) / 2;
            pos.X += offset;
            spriteBatch.DrawString(spriteFont, items[selectedItem], pos, hasFocus ? selectedColor : color);
            pos.X += -1 * offset + maxItemWidth + 5f;
            spriteBatch.Draw(selectedItem != items.Count - 1 ? rightArrow : stopTexture, pos, Color.White);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (items.Count == 0)
                return;

            if (InputHandler.ButtonReleased(Buttons.LeftThumbstickLeft, playerIndex) ||
            InputHandler.ButtonReleased(Buttons.DPadLeft, playerIndex) ||
            InputHandler.KeyReleased(Keys.Left))
            {
                selectedItem--;
                if (selectedItem < 0)
                    selectedItem = 0;
                OnSelectionChanged();
            }
            if (!InputHandler.ButtonReleased(Buttons.LeftThumbstickRight, playerIndex) &&
                !InputHandler.ButtonReleased(Buttons.DPadRight, playerIndex) && !InputHandler.KeyReleased(Keys.Right))
                return;
            selectedItem++;
            if (selectedItem >= items.Count)
                selectedItem = items.Count - 1;
            OnSelectionChanged();
        }
        #endregion
    }
}
