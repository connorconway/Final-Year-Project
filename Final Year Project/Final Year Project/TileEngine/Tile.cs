namespace Final_Year_Project.TileEngine
{
    public class Tile
    {
        #region Variables
        public int tileIndex { get; private set; }
        public int tileSet { get; private set; }
        #endregion

        #region Constructor(s)
        public Tile(int tileIndex, int tileSet)
        {
            this.tileIndex = tileIndex;
            this.tileSet = tileSet;
        }
        #endregion
    }
}
