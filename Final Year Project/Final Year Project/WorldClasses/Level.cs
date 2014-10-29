using Multiplayer_Software_Game_Engineering.TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.WorldClasses
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
