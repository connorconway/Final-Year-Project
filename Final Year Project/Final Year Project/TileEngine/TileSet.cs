using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.TileEngine
{
    public class TileSet
    {
        #region Variables
        public Texture2D texture {get; private set;}
        private int tileWidth { get; set; }
        private int tileHeight { get; set; }
        private int xTilesWide { get; set; }
        private int xTilesHigh { get; set; }
        public Rectangle[] sourceRects { get; private set; }
        #endregion

        #region Constructor(s)
        public TileSet(Texture2D texture, int tileWidth, int tileHeight, int xTilesWide, int xTilesHigh)
        {
            this.texture = texture;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.xTilesWide = xTilesWide;
            this.xTilesHigh = xTilesHigh;

            int totalTiles = xTilesHigh * xTilesWide;

            sourceRects = new Rectangle[totalTiles];

            int tile = 0;

            for (int x = 0; x < xTilesWide; x++)
            {
                for (int y = 0; y < xTilesHigh; y++)
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
        #endregion
    }
}
