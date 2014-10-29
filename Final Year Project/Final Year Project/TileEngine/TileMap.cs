using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Multiplayer_Software_Game_Engineering.TileEngine
{
    public class TileMap
    {
        private readonly List<TileSet>   tileSets;
        private readonly List<MapLayer>  mapLayers;
        private static   int             mapWidth;
        private static   int             mapHeight;

        public static int MapWidth
        {
            get { return mapWidth * Engine.tileWidth; }
        }

        public static int MapHeight
        {
            get { return mapHeight * Engine.tileHeight; }
        }

        public TileMap(List<TileSet> tileSets, List<MapLayer> mapLayers)
        {
            this.tileSets  = tileSets;
            this.mapLayers = mapLayers;
            mapWidth       = mapLayers[0].width;
            mapHeight      = mapLayers[0].height;

            for (var i = 1; i < mapLayers.Count; i++)
            {
                if (mapWidth != mapLayers[i].width || mapHeight != mapLayers[i].height)
                    throw new Exception("Map layer size incorrect");
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var cameraPoint = Engine.GetCellFromVector(camera.position * (1 / camera.zoom));
            var viewPoint   = Engine.GetCellFromVector(
                                new Vector2(
                                    (camera.position.X + camera.viewportRectangle.Width) * (1 / camera.zoom),
                                    (camera.position.Y + camera.viewportRectangle.Height) * (1 / camera.zoom)));
            var min         = new Point();
            var max         = new Point();
            min.X           = Math.Max(0, cameraPoint.X - 1);
            min.Y           = Math.Max(0, cameraPoint.Y - 1);
            max.X           = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y           = Math.Min(viewPoint.Y + 1, mapHeight);
            var destination = new Rectangle(0, 0, Engine.tileWidth, Engine.tileHeight);

            foreach (var layer in mapLayers)
            {
                for (var y = min.Y; y < max.Y; y++)
                {
                    destination.Y = y * Engine.tileHeight;
                    for (var x = min.X; x < max.X; x++)
                    {
                        var tile = layer.GetTile(x, y);
                        if (tile.tileIndex == -1 || tile.tileSet == -1)
                            continue;
                        destination.X = x * Engine.tileWidth;
                        spriteBatch.Draw(
                            tileSets[tile.tileSet].texture,
                            destination,
                            tileSets[tile.tileSet].sourceRects[tile.tileIndex],
                            Color.White);
                    }
                }
            }
        }
    }
}