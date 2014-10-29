using Multiplayer_Software_Game_Engineering.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.TileEngine
{
    public class TileSet
    {
        public  Texture2D   texture     { get; private set; }
        private int         tileWidth   { [UsedImplicitly] get; set; }
        private int         tileHeight  { [UsedImplicitly] get; set; }
        private int         xTilesWide  { [UsedImplicitly] get; set; }
        private int         xTilesHigh  { [UsedImplicitly] get; set; }
        public  Rectangle[] sourceRects { get; private set; }

        public TileSet(Texture2D texture, int tileWidth, int tileHeight, int xTilesWide, int xTilesHigh)
        {
            this.texture    = texture;
            this.tileWidth  = tileWidth;
            this.tileHeight = tileHeight;
            this.xTilesWide = xTilesWide;
            this.xTilesHigh = xTilesHigh;
            var totalTiles  = xTilesHigh * xTilesWide;
            sourceRects     = new Rectangle[totalTiles];
            var tile        = 0;

            for (var x = 0; x < xTilesWide; x++)
            {
                for (var y = 0; y < xTilesHigh; y++)
                {
                    sourceRects[tile] =
                        new Rectangle(
                            x * tileWidth,
                            y * tileHeight,
                            tileWidth,
                            tileHeight);
                    tile++;
                }
            }
        }
    }
}