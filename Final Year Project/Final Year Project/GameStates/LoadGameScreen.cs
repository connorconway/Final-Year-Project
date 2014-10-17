using System;
using System.Collections.Generic;
using Final_Year_Project.Components;
using Final_Year_Project.Controls;
using Final_Year_Project.GameData;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.GameStates
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

            loadGameLinkLabel = new LinkLabel { text = Constants._SELECTGAME };
            mainMenuLinkLabel = new LinkLabel { text = Constants._MAINMENU };

            loadGameLinkLabel.size = loadGameLinkLabel.spriteFont.MeasureString(loadGameLinkLabel.text);
            loadGameLinkLabel.position = new Vector2((int) (Game1._systemOptions.resolutionWidth - loadGameLinkLabel.size.X) >> 1,
                (Game1._systemOptions.resolutionHeight >> 1));

            mainMenuLinkLabel.size = mainMenuLinkLabel.spriteFont.MeasureString(mainMenuLinkLabel.text);
            mainMenuLinkLabel.position = new Vector2((int) (Game1._systemOptions.resolutionWidth - mainMenuLinkLabel.size.X) >> 1,
                (Game1._systemOptions.resolutionHeight >> 1) + 40);

            loadGameListBox = new ListBox(Content.Load<Texture2D>(@"Graphics\GUI\listBoxImage2"),
                Content.Load<Texture2D>(@"Graphics\GUI\rightarrowUp"));
            loadGameListBox.position = new Vector2((int) (Game1._systemOptions.resolutionWidth - loadGameListBox.size.X) >> 1,
                (Game1._systemOptions.resolutionHeight >> 1) + 110);

            for (int i = 0; i < 10; i++)
                loadGameListBox.Items.Add("Game number: " + (i + 1));
            loadGameListBox.Items.Add("Back");

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
                case "Back":
                    loadGameListBox.HasFocus = false;
                    loadGameLinkLabel.HasFocus = true;
                    break;
                default:
                    loadGameLinkLabel.HasFocus = true;
                    loadGameListBox.HasFocus = false;
                    controlManager.AcceptInput = true;
                    stateManager.ChangeState(gameReference.gamePlayScreen);
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
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);
            AnimatedSprite sprite =
                new AnimatedSprite(gameReference.Content.Load<Texture2D>(@"Graphics\Sprites\malefighter"), animations);
            //GamePlayScreen.player = new Player(gameReference, sprite);
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
                    Tile tile = new Tile(0, 0);
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
                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            splatter.SetTile(1, 0, new Tile(0, 1));
            splatter.SetTile(2, 0, new Tile(2, 1));
            splatter.SetTile(3, 0, new Tile(0, 1));
            List<MapLayer> mapLayers = new List<MapLayer> {layer, splatter};
            TileMap map = new TileMap(tilesets, mapLayers);
            Level level = new Level(map);
            World world = new World(gameReference, gameReference.screenRectangle);
            world.Levels.Add(level);
            world.CurrentLevel = 0;
            GamePlayScreen.world = world;
        }
        #endregion
    }
}