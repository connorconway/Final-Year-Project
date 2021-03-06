﻿using System;
using System.Collections.Generic;
using Multiplayer_Software_Game_Engineering.Controls;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameEntities;
using Multiplayer_Software_Game_Engineering.TileEngine;
using Multiplayer_Software_Game_Engineering.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Multiplayer_Software_Game_Engineering.Levels;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class LoadGameScreen : BaseGameState
    {
        #region Variables
        private ListBox loadGameListBox;
        private LinkLabel loadGameLinkLabel;
        private LinkLabel mainMenuLinkLabel;
        #endregion

        #region Constructor(s)
        public LoadGameScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
        }
        #endregion

        #region Override Methods
        protected override void LoadContent()
        {
            base.LoadContent();
            ContentManager Content = Game.Content;

            loadGameLinkLabel = new LinkLabel { text = Constants.SELECT_CHARACTER };
            mainMenuLinkLabel = new LinkLabel { text = Constants.MAIN_MENU };

            loadGameLinkLabel.size = loadGameLinkLabel.spriteFont.MeasureString(loadGameLinkLabel.text);
            loadGameLinkLabel.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - loadGameLinkLabel.size.X) >> 1,
                (Game1.systemOptions.resolutionHeight >> 1));

            mainMenuLinkLabel.size = mainMenuLinkLabel.spriteFont.MeasureString(mainMenuLinkLabel.text);
            mainMenuLinkLabel.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - mainMenuLinkLabel.size.X) >> 1,
                (Game1.systemOptions.resolutionHeight >> 1) + 40);

            loadGameListBox = new ListBox(Content.Load<Texture2D>(@"Graphics\GUI\listBoxImage2"),
                Content.Load<Texture2D>(@"Graphics\GUI\rightarrowUp"));
            loadGameListBox.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - loadGameListBox.size.X) >> 1,
                (Game1.systemOptions.resolutionHeight >> 1) + 110);

            for (int i = 0; i < 10; i++)
                loadGameListBox.Items.Add(Constants.MAKE_CHOICE + (i + 1));
            loadGameListBox.Items.Add(Constants.BACK);

            loadGameLinkLabel.selected += loadGameLinkLabelSelected;
            mainMenuLinkLabel.selected += exitLinkLabel_Selected;
            loadGameListBox.selected += loadGameListBoxSelected;
            loadGameListBox.leave += loadGameListBoxLeave;

            controlManager.Add(loadGameLinkLabel);
            controlManager.Add(mainMenuLinkLabel);
            controlManager.Add(loadGameListBox);
            controlManager.NextControl();
        }

        public override void Update(GameTime gameTime)
        {
            controlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameReference.spriteBatch.Begin();
            base.Draw(gameTime);
            gameReference.spriteBatch.Draw(backgroundImage, gameReference.screenRectangle, color);
            gameReference.spriteBatch.Draw(backgroundBorder, gameReference.screenRectangle, Color.White);
            controlManager.Draw(gameReference.spriteBatch);
            gameReference.spriteBatch.End();
        }
        #endregion

        #region General Methods
        private void loadGameListBoxLeave(object sender, EventArgs e)
        {
            loadGameLinkLabel.HasFocus = true;
            controlManager.AcceptInput = true;
        }

        private void loadGameLinkLabelSelected(object sender, EventArgs e)
        {
            controlManager.AcceptInput = false;
            loadGameLinkLabel.HasFocus = false;
            loadGameListBox.HasFocus = true;
        }

        private void loadGameListBoxSelected(object sender, EventArgs e)
        {
            switch (loadGameListBox.SelectedItem)
            {
                case Constants.BACK:
                    loadGameListBox.HasFocus = false;
                    loadGameLinkLabel.HasFocus = true;
                    break;
                default:
                    loadGameLinkLabel.HasFocus = true;
                    loadGameListBox.HasFocus = false;
                    controlManager.AcceptInput = true;
                    stateManager.ChangeState(gameReference.Level1);
                    CreatePlayer();
                    CreateWorld();
                    break;
            }
        }

        private void exitLinkLabel_Selected(object sender, EventArgs e)
        {
            stateManager.PopState();
        }

        private void CreatePlayer()
        {
            Dictionary<Constants.Direction, Animation> animations = new Dictionary<Constants.Direction, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(Constants.Direction.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(Constants.Direction.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(Constants.Direction.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(Constants.Direction.Up, animation);
            AnimatedSprite sprite =
                new AnimatedSprite(gameReference.Content.Load<Texture2D>(@"Graphics\Sprites\malefighter"), animations);
            //Level1.player = new Player(gameReference, sprite);
        }

        private void CreateWorld()
        {
            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_1");
            TileSet tileset1 = new TileSet(tilesetTexture, 8, 8, 32, 32);
            tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_2");
            TileSet tileset2 = new TileSet(tilesetTexture, 8, 8, 32, 32);
            List<TileSet> tilesets = new List<TileSet> {tileset1, tileset2};
            MapLayer layer = new MapLayer(100, 100);

            for (int y = 0; y < layer.height; y++)
            {
                for (int x = 0; x < layer.width; x++)
                {
                    Tile tile = new Tile(0, 0, Constants.TileState.IMPASSABLE);
                    layer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(100, 100);
            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(0, 100);
                int y = random.Next(0, 100);
                int index = random.Next(2, 14);
                Tile tile = new Tile(index, 0, Constants.TileState.IMPASSABLE);
                splatter.SetTile(x, y, tile);
            }

            splatter.SetTile(1, 0, new Tile(0, 1, Constants.TileState.IMPASSABLE));
            splatter.SetTile(2, 0, new Tile(2, 1, Constants.TileState.IMPASSABLE));
            splatter.SetTile(3, 0, new Tile(0, 1, Constants.TileState.IMPASSABLE));
            List<MapLayer> mapLayers = new List<MapLayer> {layer, splatter};
            TileMap map = new TileMap(tilesets, mapLayers);
            Level level = new Level(map);
            World world = new World(gameReference, gameReference.screenRectangle);
            World.levels.Add(level);
            world.currentLevel = 0;
            Level1.world = world;
        }
        #endregion
    }
}