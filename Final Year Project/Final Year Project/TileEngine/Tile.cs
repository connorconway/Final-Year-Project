using Multiplayer_Software_Game_Engineering.GameData;

namespace Multiplayer_Software_Game_Engineering.TileEngine
{
    public class Tile
    {
        public int tileIndex { get; private set; }
        public int tileSet   { get; private set; }
        public Constants.TileState tileState { get; private set; }

        public Tile(int tileIndex, int tileSet, object tileState)
        {
            this.tileIndex = tileIndex;
            this.tileSet   = tileSet;
            this.tileState = (Constants.TileState) tileState;
        }
    }
}