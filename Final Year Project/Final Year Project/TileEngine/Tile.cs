namespace Multiplayer_Software_Game_Engineering.TileEngine
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