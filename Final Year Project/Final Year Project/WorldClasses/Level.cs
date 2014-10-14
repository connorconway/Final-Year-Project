using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.WorldClasses
{
    public class Level
    {
        #region Variables
        TileMap tileMap { get; set; }
        #endregion

        #region Constructor(s)
        public Level(TileMap tileMap)
        {
            this.tileMap = tileMap;
        }
        #endregion

        #region General Methods
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            tileMap.Draw(spriteBatch, camera);    
        }
        #endregion
    }
}
