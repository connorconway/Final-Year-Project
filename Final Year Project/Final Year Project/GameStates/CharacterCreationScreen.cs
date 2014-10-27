using System;
using System.Collections.Generic;
using Final_Year_Project.Components;
using Final_Year_Project.Controls;
using Final_Year_Project.Handlers;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Label = Final_Year_Project.Controls.Label;
using LinkLabel = Final_Year_Project.Controls.LinkLabel;
using PictureBox = Final_Year_Project.Controls.PictureBox;

namespace Final_Year_Project.GameStates
{
    public class CharacterCreationScreen : BaseGameState
    {
        #region Varibales
        private LeftRightSelector selectGender;
        private LeftRightSelector selectClass;
        private PictureBox characterImage;
        private Texture2D[,] characterImages;
        private readonly String[] genderItems = {"Male", "Female"};
        private readonly String[] classItems = {"Fighter", "Wizard", "Rogue", "Priest"};
        #endregion

        #region Getter(s) and Setter(s)
        public string SelectGender { get { return selectGender.SelectedItem; } }

        public string SelectClass { get { return selectClass.SelectedItem; } }
        #endregion

        #region Contructor(s)
        public CharacterCreationScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {    
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            LoadImages();
            CreateControls();
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
        private void CreateControls()
        {
            Texture2D leftTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\leftarrowUp");
            Texture2D rightTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\rightarrowUp");
            Texture2D stopTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\StopBar");

            Label label1 = new Label {text = "Who will you fight as?"};
            label1.size = label1.spriteFont.MeasureString(label1.text);
            label1.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - label1.size.X) >> 1,
                Game1.systemOptions.resolutionHeight >> 1);

            selectGender = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectGender.SetItems(genderItems, 125);
            selectGender.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1,
                label1.position.Y + 50);
            selectGender.selectionChanged += selectionChanged;

            selectClass = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectClass.SetItems(classItems, 125);
            selectClass.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1,
                selectGender.position.Y + 50);
            selectClass.selectionChanged += selectionChanged;

            LinkLabel linkLabel1 = new LinkLabel {text = "Create New Lobby"};
            linkLabel1.size = linkLabel1.spriteFont.MeasureString(linkLabel1.text);
            linkLabel1.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - linkLabel1.size.X) >> 1,
                selectClass.position.Y + 75);
            linkLabel1.selected += linkLabel1_Selected;

            LinkLabel joinLobby = new LinkLabel { text = "Join a lobby" };
            joinLobby.size = linkLabel1.spriteFont.MeasureString(joinLobby.text);
            joinLobby.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - joinLobby.size.X) >> 1,
                linkLabel1.position.Y + 50);
            joinLobby.selected += joinLobby_Selected;

            LinkLabel linkLabel2 = new LinkLabel {text = "Back to menu"};
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int) (Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                joinLobby.position.Y + 75);
            linkLabel2.selected += linkLabel2_Selected;

            characterImage = new PictureBox(characterImages[0, 0],
                new Rectangle(((Game1.systemOptions.resolutionWidth - 96) >> 1),
                    ((Game1.systemOptions.resolutionHeight + 120 ) >> 2), 96, 96), new Rectangle(0, 0, 32, 32));

            controlManager.Add(label1);
            controlManager.Add(selectGender);
            controlManager.Add(selectClass);
            controlManager.Add(linkLabel1);
            controlManager.Add(joinLobby);
            controlManager.Add(linkLabel2);
            controlManager.Add(characterImage);
            controlManager.NextControl();
        }

        private void LoadImages()
        {
            characterImages = new Texture2D[genderItems.Length, classItems.Length];
            for (int i = 0; i < genderItems.Length; i++)
            {
                for (int j = 0; j < classItems.Length; j++)
                {
                    characterImages[i, j] =
                        Game.Content.Load<Texture2D>(@"Graphics\Sprites\" + genderItems[i] + classItems[j]);
                }
            }
        }

        private void CreatePlayer()
        {
            var animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);

            AnimatedSprite sprite =
                new AnimatedSprite(characterImages[selectGender.SelectedIndex, selectClass.SelectedIndex], animations);
            sprite.textTexture = selectGender.SelectedItem + selectClass.SelectedItem;
            
            Texture2D spriteToUse;
            if (selectClass.SelectedItem.Contains("Fighter"))
                spriteToUse = bulletSprite;
            else if (selectClass.SelectedItem.Contains("Rogue"))
                spriteToUse = shurikenBulletSprite;
            else if (selectClass.SelectedItem.Contains("Priest"))
                spriteToUse = healthBulletSprite;
            else
                spriteToUse = fireBallBulletSprite;

            player1 = new Player(gameReference, sprite, spriteToUse, healthBarSprite, Color.Green);
            player1.animatedSprite.textTexture = selectGender.SelectedItem + selectClass.SelectedItem;
        }

        public void CreateWorld()
        {
            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_1");
            TileSet tileSet1 = new TileSet(tilesetTexture, 16, 16, 10, 28);

            tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_2");
            TileSet tileSet2 = new TileSet(tilesetTexture, 16, 16, 10, 28);

            List<TileSet> tilesets = new List<TileSet> {tileSet1, tileSet2};

            MapLayer mapLayer = new MapLayer(100, 100);

            for (int y = 0; y < mapLayer.height; y++)
            {
                for (int x = 0; x < mapLayer.width; x++)
                {
                    Tile tile = new Tile(33, 0);
                    mapLayer.SetTile(x, y, tile);
                }
            }

            MapLayer splatter = new MapLayer(100, 100);
            Random random = new Random();
            for (int i = 0; i < 70; i++)
            {
                int x = random.Next(0, 90);
                int y = random.Next(0, 70);
                int[] choices = { 53, 34, 120, 229, 34, 34, 34, 229, 34, 34, 34, 34, 229, 34, 120, 53, 229, 229, 34, 229, 34, 34, 34, 34, 229, 229, 34 };
                int index = choices[random.Next(choices.Length)];
                Tile tile = new Tile(index, 0);
                splatter.SetTile(x, y, tile);
            }

            //TODO: 53  : Full Health
            //TODO: 34  : Random Rocks
            //TODO: 120 : Chest (Shoot to open)

//            splatter.SetTile(1, 0, new Tile(0, 1));
//            splatter.SetTile(2, 0, new Tile(2, 1));
//            splatter.SetTile(3, 0, new Tile(0, 1));

            List<MapLayer> mapLayers = new List<MapLayer> {mapLayer, splatter};

            TileMap map = new TileMap(tilesets, mapLayers);
            Level level = new Level(map);
            World world = new World(gameReference, gameReference.screenRectangle);
            World.levels.Add(level);
            world.currentLevel = 0;

            GamePlayScreen.world = world;
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            characterImage.texture = characterImages[selectGender.SelectedIndex, selectClass.SelectedIndex];
        }

        private void linkLabel1_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            CreatePlayer();
            CreateWorld();
            player1.setHost(true);
            stateManager.PushState(gameReference.gamePlayScreen);
        }

        private void joinLobby_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            CreatePlayer();
            stateManager.PushState(gameReference.lobbyScreen);
        }

        private void linkLabel2_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();
            stateManager.PopState();
        }
        #endregion
    }
}
