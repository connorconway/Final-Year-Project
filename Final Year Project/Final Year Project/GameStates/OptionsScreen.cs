using System;
using System.Xml.Serialization;
using Multiplayer_Software_Game_Engineering.Controls;
using Multiplayer_Software_Game_Engineering.GameData;
using Multiplayer_Software_Game_Engineering.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiplayer_Software_Game_Engineering.GameStates
{
    public class OptionsScreen : BaseGameState
    {
        private LeftRightSelector selectFullScreen;
        private LeftRightSelector selectDifficulty;
        private LeftRightSelector selectSoundLevel;
        private LeftRightSelector selectMusicLevel;
        private LeftRightSelector selectResolution;
        readonly String[] fullScreenItems = { "True", "False" };
        readonly String[] difficultyItems = { "Easy", "Medium", "Hard" };
        readonly String[] soundLevelItems = { "On", "Quiet", "Off" };
        readonly String[] musicLevelItems = { "On", "Quiet", "Off" };
        readonly String[] resolutionItems = { "1280x900", "1366x876", "1920x1080", "2560x1080" };

        public string SelectFullScreen
        {
            get { return selectFullScreen.SelectedItem; }
        }

        public string SelectDifficulty
        {
            get { return selectDifficulty.SelectedItem; }
        }

        public string SelectSoundLevel
        {
            get { return selectSoundLevel.SelectedItem; }
        }

        public string SelectMusicLevel
        {
            get { return selectMusicLevel.SelectedItem; }
        }

        public string SelectResolution
        {
            get { return selectResolution.SelectedItem; }
        }

        public OptionsScreen(Game game, GameStateManager stateManager) : base(game, stateManager)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
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

        private void CreateControls()
        {
            Texture2D leftTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\leftarrowUp");
            Texture2D rightTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\rightarrowUp");
            Texture2D stopTexture = Game.Content.Load<Texture2D>(@"Graphics\GUI\StopBar");

            Label label1 = new Label { text = "Changes will take effect after restarting the game" };
            label1.size = label1.spriteFont.MeasureString(label1.text);
            label1.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - label1.size.X) >> 1, Game1.systemOptions.resolutionHeight >> 1);

            selectFullScreen = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectFullScreen.SetItems(fullScreenItems, 125);
            selectFullScreen.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1, label1.position.Y + 50);
            selectFullScreen.SelectedIndex = Game1.systemOptions.fullScreen ? 0 : 1;

            selectDifficulty = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectDifficulty.SetItems(difficultyItems, 125);
            selectDifficulty.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1, selectFullScreen.position.Y + 50);
            selectDifficulty.SelectedIndex = (int)Game1.systemOptions.difficultyLevel ;

            selectSoundLevel = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectSoundLevel.SetItems(soundLevelItems, 125);
            selectSoundLevel.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1, selectDifficulty.position.Y + 50);
            selectSoundLevel.SelectedIndex = (int)Game1.systemOptions.soundLevel;

            selectMusicLevel = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectMusicLevel.SetItems(musicLevelItems, 125);
            selectMusicLevel.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1, selectSoundLevel.position.Y + 50);
            selectMusicLevel.SelectedIndex = (int)Game1.systemOptions.musicLevel;

            selectResolution = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            selectResolution.SetItems(resolutionItems, 125);
            selectResolution.position = new Vector2((Game1.systemOptions.resolutionWidth - 125 - 96) >> 1, selectMusicLevel.position.Y + 50);
            selectResolution.SelectedIndex = Array.IndexOf(resolutionItems, Game1.systemOptions.resolutionWidth + "x" + Game1.systemOptions.resolutionHeight);

            LinkLabel linkLabel1 = new LinkLabel
            {
                text = Constants.ACCEPT_CHANGES
            };
            linkLabel1.size = linkLabel1.spriteFont.MeasureString(linkLabel1.text);
            linkLabel1.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel1.size.X) >> 1,
                selectResolution.position.Y + 75);
            linkLabel1.selected += linkLabel1_Selected;

            LinkLabel linkLabel2 = new LinkLabel
            {
                text = Constants.DISMISS_CHANGES
            };
            linkLabel2.size = linkLabel2.spriteFont.MeasureString(linkLabel2.text);
            linkLabel2.position = new Vector2((int)(Game1.systemOptions.resolutionWidth - linkLabel2.size.X) >> 1,
                linkLabel1.position.Y + 50);
            linkLabel2.selected += linkLabel2_Selected;

            controlManager.Add(label1);
            controlManager.Add(selectFullScreen);
            controlManager.Add(selectDifficulty);
            controlManager.Add(selectSoundLevel);
            controlManager.Add(selectMusicLevel);
            controlManager.Add(selectResolution);
            controlManager.Add(linkLabel1);
            controlManager.Add(linkLabel2);
            controlManager.NextControl();
        }

        void linkLabel1_Selected(object sender, EventArgs e)
        {
            Game1.systemOptions.soundLevel = (Sound) selectSoundLevel.SelectedIndex;
            Game1.systemOptions.musicLevel = (Music) selectMusicLevel.SelectedIndex;
            Game1.systemOptions.difficultyLevel = (Difficulty) selectDifficulty.SelectedIndex;
            try
            {
                Game1.systemOptions.resolutionHeight = Convert.ToInt32(resolutionItems[selectResolution.SelectedIndex].Substring(5, 4));

            }
            catch (Exception)
            {
                Game1.systemOptions.resolutionHeight = Convert.ToInt32(resolutionItems[selectResolution.SelectedIndex].Substring(5, 3));
            }

            Game1.systemOptions.resolutionWidth = Convert.ToInt32(resolutionItems[selectResolution.SelectedIndex].Substring(0, 4));
            Game1.systemOptions.fullScreen = Boolean.Parse(fullScreenItems[selectFullScreen.SelectedIndex]);

            FileHandler.writeToFile(Game1.systemOptions, Constants._serviceOptionsPath, new XmlSerializer(typeof(SystemOptions)), true);
            stateManager.PopState();
        }

        void linkLabel2_Selected(object sender, EventArgs e)
        {
            stateManager.PopState();
        }
    }
}