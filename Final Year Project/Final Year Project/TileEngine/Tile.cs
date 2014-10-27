namespace Final_Year_Project.TileEngine
{
    public class Tile
    {
        public int tileIndex { get; private set; }
        public int tileSet   { get; private set; }

        public Tile(int tileIndex, int tileSet)
        {
            this.tileIndex = tileIndex;
            this.tileSet   = tileSet;
        }
    }
}