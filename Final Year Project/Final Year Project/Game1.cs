using System;
using System.IO;
using System.Xml.Serialization;
using Final_Year_Project.GameData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Final_Year_Project.Handlers;
using Final_Year_Project.GameStates;

namespace Final_Year_Project
{
    public class Game1 : Game
    {
        #region Variables
        public static SystemOptions _systemOptions = new SystemOptions();
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        GameStateManager stateManager;
        public StartMenuScreen startMenuScreen;
        public Rectangle screenRectangle { get; private set; }
        public GamePlayScreen gamePlayScreen;
        public CharacterCreationScreen characterCreationScreen;
        public PauseScreen pauseScreen;
        public OptionsScreen optionsScreen;
        public LoadGameScreen loadGameScreen;
        public GameLoseScreen gameLoseScreen;
        public GameWinScreen gameWinScreen;
        #endregion

        #region Constructor(s)
        public Game1()
        {
           //TODO: Use relative path and not absolute
            FileHandler.writeToFile(_systemOptions, Constants._serviceOptionsPath, new XmlSerializer(typeof(SystemOptions)));
            _systemOptions = FileHandler.readFromFile(Constants._serviceOptionsPath, new XmlSerializer(typeof(SystemOptions)));

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _systemOptions.resolutionWidth,
                PreferredBackBufferHeight = _systemOptions.resolutionHeight,
                IsFullScreen = _systemOptions.fullScreen
            };

            screenRectangle = new Rectangle(0, 0, _systemOptions.resolutionWidth, _systemOptions.resolutionHeight);

            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            startMenuScreen = new StartMenuScreen(this, stateManager);
            gamePlayScreen = new GamePlayScreen(this, stateManager);
            characterCreationScreen = new CharacterCreationScreen(this, stateManager);
            pauseScreen = new PauseScreen(this, stateManager);
            optionsScreen = new OptionsScreen(this, stateManager);
            loadGameScreen = new LoadGameScreen(this, stateManager);
            gameLoseScreen = new GameLoseScreen(this, stateManager);
            gameWinScreen = new GameWinScreen(this, stateManager);

            stateManager.ChangeState(startMenuScreen);
        }
        #endregion

        #region Override Methods
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);                                                                                                           
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)                                                                               
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
        #endregion
    }
}
