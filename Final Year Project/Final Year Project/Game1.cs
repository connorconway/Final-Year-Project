using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.GameStates;
using Multiplayer_Software_Game_Engineering.Handlers;
using Multiplayer_Software_Game_Engineering.Levels;

namespace Multiplayer_Software_Game_Engineering
{
    public class Game1 : Game
    {
        public static   SystemOptions           systemOptions                = new SystemOptions();
        public          SpriteBatch             spriteBatch;
        public readonly StartMenuScreen         startMenuScreen;
        public          Rectangle               screenRectangle              { get; private set; }
        public readonly Level1          Level1;
        public readonly CharacterCreationScreen characterCreationScreen;
        public readonly PauseScreen             pauseScreen;
        public readonly OptionsScreen           optionsScreen;
        public readonly LoadGameScreen          loadGameScreen;
        public readonly HighScoresScreen        highScoreScreen;
        public readonly GameLoseScreen          gameLoseScreen;
        public readonly LobbyScreen             lobbyScreen;
        private         GameStateManager        stateManager;

        public Game1()
        {
           
            FileHandler.writeToFile(systemOptions, @"Content\Data\system_variables.xml", new XmlSerializer(typeof(SystemOptions)));
            systemOptions = FileHandler.readFromFile(@"Content\Data\system_variables.xml", new XmlSerializer(typeof(SystemOptions)));

            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth    = systemOptions.resolutionWidth,
                PreferredBackBufferHeight   = systemOptions.resolutionHeight,
                IsFullScreen                = systemOptions.fullScreen
            };

            Components.Add(new InputHandler(this));
            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            screenRectangle         = new Rectangle(0, 0, systemOptions.resolutionWidth, systemOptions.resolutionHeight);
            startMenuScreen         = new StartMenuScreen(this, stateManager);
            Level1          = new Level1(this, stateManager);
            characterCreationScreen = new CharacterCreationScreen(this, stateManager);
            pauseScreen             = new PauseScreen(this, stateManager);
            optionsScreen           = new OptionsScreen(this, stateManager);
            loadGameScreen          = new LoadGameScreen(this, stateManager);
            gameLoseScreen          = new GameLoseScreen(this, stateManager);
            lobbyScreen             = new LobbyScreen(this, stateManager);
            highScoreScreen         = new HighScoresScreen(this, stateManager);

            stateManager.ChangeState(startMenuScreen);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);      
            base.LoadContent();                                                                                         
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
    }
}
