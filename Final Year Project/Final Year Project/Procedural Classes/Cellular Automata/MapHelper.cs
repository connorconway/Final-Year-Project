using System;

namespace Multiplayer_Software_Game_Engineering.Procedural_Classes.Cellular_Automata
{
    internal class MapHelper
    {
        private readonly Random rand = new Random();

        private readonly int mapWidth, mapHeight;
        private int walls { get; set; }                                                 // The percentage of the map that will contain impassable tiles
        public int[,] tileInts;                                                         // A 2D Array of ints to store the tile number for the map

        public MapHelper()
        {
            mapWidth = 3000;
            mapHeight = 3000;
            walls = 45;

            RandomFillMap();
        }

        private void RandomFillMap()
        {
            tileInts = new int[mapWidth, mapHeight];

            for (var i = 0; i < mapHeight; i++)
            {
                for (var j = 0; j < mapWidth; j++)
                {
                    if (j == 0 || i == 0 || j == mapWidth - 1 || i == mapHeight - 1)    // Impassable tiles for spaces that lie on the edge of the map
                        tileInts[j, i] = 1;
                    else
                    {
                        tileInts[j, i] = walls >= rand.Next(1, 101) ? 1 : 0;            // Chance of either a passable or impassable tile dependant on the percentage of walls defined
                    }
                }
            }
        }

        public void FillWithRules()
        {
            for (var i = 0; i <= mapHeight - 1; i++)
                for (var j = 0; j <= mapWidth - 1; j++)
                    tileInts[j, i] = PlaceWallLogic(j, i);
        }

        private int PlaceWallLogic(int x, int y)
        {
            var numWalls = GetAdjacentWalls(x, y, 1, 1);

            if (tileInts[x, y] == 1)
                return numWalls >= 4 ? 1 : 0;

            return numWalls >= 5 ? 1 : 0;
        }

        private int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
        {
            var startX = x - scopeX;
            var startY = y - scopeY;
            var endX   = x + scopeX;
            var endY   = y + scopeY;

            var wallCounter = 0;

            for (var i = startY; i <= endY; i++)
            {
                for (var j = startX; j <= endX; j++)
                {
                    if (j == x && i == y)
                        continue;
                    if (IsWall(j, i))
                        wallCounter += 1;
                }
            }

            return wallCounter;
        }

        private bool IsWall(int x, int y)
        {
            if (IsOutOfBounds(x, y))
                return true;

            switch (tileInts[x, y])
            {
                case 1:
                    return true;
                case 0:
                    return false;
            }

            return false;
        }

        private bool IsOutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0)
                return true;
            return x > mapWidth - 1 || y > mapHeight - 1;
        }
    }
}