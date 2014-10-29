using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.Controls
{
    public abstract class Control
    {
        #region Variables
        public String name { get; set; }
        public String text { get; set; }
        public Vector2 size { get; set; }
        public Vector2 position { get; set; }
        public object value { get; set; }
        protected bool hasFocus;
        public bool enabled { get; set; }
        public bool visible { get; set; }
        public bool tabStop { get; set; }
        public SpriteFont spriteFont { get; set; }
        public Color color { get; set; }
        public String type { get;  set; }
        public event EventHandler selected;
        public SpriteEffects spriteEffect { get; protected set; }
        #endregion

        #region Getter(s) and Setter(s)
        public virtual bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }
        #endregion

        #region Constructor(s)
        public Control()
        {
            color = Color.White;
            enabled = true;
            visible = true;
            spriteFont = ControlManager.spriteFont;
            spriteEffect = SpriteEffects.None;
        }
        #endregion

        #region Abstract Methods
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(PlayerIndex playerIndex);
        #endregion

        #region Virtual Methods
        protected virtual void OnSelected(EventArgs e)
        {
            if (selected != null)
            {
                selected(this, e);
            }
        }
        #endregion
    }
}
