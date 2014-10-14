namespace Final_Year_Project.TileEngine
{
    public class MapLayer
    {
        #region Variables
        Tile[,] map;
        public int width { get { return map.GetLength(1); } }
        public int height { get { return map.GetLength(0); } }
        #endregion

        #region Constructor(s)
        public MapLayer(Tile[,] map)
        {
            this.map = map;
        }

        public MapLayer(int width, int height)
        {
            map = new Tile[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[y, x] = new Tile(33, 0);
                }
            }

        }
        #endregion

        #region Getter(s) and Setter(s)
        public Tile GetTile(int x, int y)
        {
            return map[y, x];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            map[y, x] = tile;
        }

        public void SetTile(int x, int y, int tileIndex, int tileSet)
        {
            map[y, x] = new Tile(tileIndex, tileSet);
        }
        #endregion

    }
}
