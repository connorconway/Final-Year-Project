using System.Collections.Generic;
using Multiplayer_Software_Game_Engineering.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.WorldClasses
{
    public class World : DrawableGameComponent
    {
        private         Rectangle   screenRectangle     { get; set; }
        public static   List<Level> levels;
        public          int         currentLevel        { private get; set; }

        public World(Game game, Rectangle screenRectangle) : base(game)
        {
            levels               = new List<Level>();
            currentLevel         = -1;
            this.screenRectangle = screenRectangle;
        }

        public void DrawLevel(SpriteBatch spriteBatch, Camera camera)
        {
            levels[currentLevel].Draw(spriteBatch, camera);
        }
    }
}