namespace Final_Year_Project.TileEngine
{
    public class MapLayer
    {
        private readonly Tile[,]        map;
        public           int            width  { get { return map.GetLength(1); } }
        public           int            height { get { return map.GetLength(0); } }

        public MapLayer(int width, int height)
        {
            map = new Tile[height, width];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (x < width / 1.2 && y < height / 1.7)
                        map[y, x] = new Tile(33, 0);
                    else
                        map[y, x] = new Tile(62, 0);
                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            return map[y, x];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            map[y, x] = tile;
        }
    }
}
