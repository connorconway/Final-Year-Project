using Microsoft.Xna.Framework;

namespace Final_Year_Project.TileEngine
{
    class Engine
    {
        #region Variables
        public static int tileWidth { get; private set; }
        public static int tileHeight { get; private set; }
        #endregion

        #region Constructor(s)
        public Engine(int tileWidth, int tileHeight)
        {
            Engine.tileHeight = tileHeight;
            Engine.tileWidth = tileWidth;
        }
        #endregion

        #region General Methods
        public static Point GetCellFromVector(Vector2 position)
        {
            return new Point( (int)position.X / tileWidth, (int)position.Y / tileWidth );
        }
        #endregion
    }
}
