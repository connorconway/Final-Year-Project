using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Final_Year_Project.TileEngine
{
    public class TileMap
    {
        #region Variable(s)
        List<TileSet> tileSets;
        List<MapLayer> mapLayers;
        static int mapWidth;
        static int mapHeight;
        #endregion

        #region Getter(s) and Setter(s)
        public static int MapWidth
        {
            get { return mapWidth * Engine.tileWidth; }
        }

        public static int MapHeight
        {
            get { return mapHeight * Engine.tileHeight; }
        }
        #endregion

        #region Constructor(s)
        public TileMap(List<TileSet> tileSets, List<MapLayer> mapLayers)
        {
            this.tileSets = tileSets;
            this.mapLayers = mapLayers;

            mapWidth = mapLayers[0].width;
            mapHeight = mapLayers[0].height;
            for (int i = 1; i < mapLayers.Count; i++)
            {
                if (mapWidth != mapLayers[i].width || mapHeight != mapLayers[i].height)
                    throw new Exception("Map layer size exception");
            }

        }

        public TileMap(TileSet tileSet, MapLayer mapLayer)
        {
            tileSets = new List<TileSet> {tileSet};

            mapLayers = new List<MapLayer> {mapLayer};

            mapWidth = mapLayers[0].width;
            mapHeight = mapLayers[0].height;
        }
        #endregion

        #region General Methods
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Point cameraPoint = Engine.GetCellFromVector(camera.Position * (1 / camera.zoom));
            Point viewPoint = Engine.GetCellFromVector(
            new Vector2(
                (camera.Position.X + camera.ViewportRectangle.Width) * (1 / camera.zoom),
                (camera.Position.Y + camera.ViewportRectangle.Height) * (1 / camera.zoom)));
            Point min = new Point();
            Point max = new Point();
            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, mapWidth);
            max.Y = Math.Min(viewPoint.Y + 1, mapHeight);
            Rectangle destination = new Rectangle(0, 0, Engine.tileWidth, Engine.tileHeight);
            foreach (MapLayer layer in mapLayers)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    destination.Y = y * Engine.tileHeight;
                    for (int x = min.X; x < max.X; x++)
                    {
                        Tile tile = layer.GetTile(x, y);
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

        public void AddLayer(MapLayer layer)
        {
            if (layer.width != mapWidth && layer.height != mapHeight)
                throw new Exception("Map layer size exception");
            mapLayers.Add(layer);
        }
        #endregion
    }
}
