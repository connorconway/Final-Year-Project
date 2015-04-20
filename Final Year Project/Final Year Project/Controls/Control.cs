using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.Controls
{
    public abstract class Control
    {
        public String name { get; set; }
        public String text { get; set; }
        public Vector2 size { get; set; }
        public Vector2 position { get; set; }
        public object value { get; set; }
        protected bool hasFocus;
        public bool enabled { get; private set; }
        public bool visible { get; private set; }
        public bool tabStop { get; protected set; }
        public SpriteFont spriteFont { get; private set; }
        protected Color color { get; set; }
        public String type { get;  set; }
        public event EventHandler selected;
        protected SpriteEffects spriteEffect { get; set; }

        public virtual bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }

        protected Control()
        {
            color = Color.White;
            enabled = true;
            visible = true;
            spriteFont = ControlManager.spriteFont;
            spriteEffect = SpriteEffects.None;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(PlayerIndex playerIndex);

        protected void OnSelected(EventArgs e)
        {
            if (selected != null)
            {
                selected(this, e);
            }
        }
    }
}
