using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.WorldClasses
{
    public class Level
    {
        private TileMap tileMap { get; set; }

        public Level(TileMap tileMap)
        {
            this.tileMap = tileMap;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            tileMap.Draw(spriteBatch, camera);    
        }
    }
}
