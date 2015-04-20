using System;
using Multiplayer_Software_Game_Engineering.GameData;

namespace Multiplayer_Software_Game_Engineering.TileEngine
{
    public class MapLayer
    {
        private readonly Tile[,]        map;
        public           int            width    { get { return map.GetLength(1); } }
        public           int            height   { get { return map.GetLength(0); } }

        public MapLayer(int width, int height)
        {
            Random rand = new Random();
            map = new Tile[height, width];

            for (var y = 0; y < height; y++)
            {  
                for (var x = 0; x < width; x++)
                {
                    int smallGrass = rand.Next(1, 15);
                    int trees = rand.Next(1, 15);
                    int deadTrees = rand.Next(1, 100);
                    map[x, y] = new Tile(trees == 1 ? 201 : smallGrass == 1 ? 61 : deadTrees == 1 ? 229 : 89, 0, Constants.TileState.IMPASSABLE);

                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            return map[x, y];
        }

        public int isPassable(int x, int y)
        {
            Tile tile = GetTile(x, y);

            if (tile.tileState == Constants.TileState.PASSABLE)
                return 1;

            if (tile.tileState == Constants.TileState.STAIRS)
                return 2;

            return 0;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            try
            {
                map[x, y] = tile;
            }
            catch (Exception)
            {
                //Tile placed outside of map
            }
               
        }
    }
}
