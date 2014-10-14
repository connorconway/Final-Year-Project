using System.Runtime.InteropServices;
using System.Threading;
using Final_Year_Project.Components;
using Final_Year_Project.Handlers;
using Final_Year_Project.Networking;
using Final_Year_Project.TileEngine;
using Final_Year_Project.WorldClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace Final_Year_Project.GameStates
{
    public class GamePlayScreen : BaseGameState
    {
        #region Variables
        private Engine engine = new Engine(32, 32);
        public static Player player { get; set; }
        public static World world { get; set; }
        private Client client;
        private const string hostname = "192.168.0.11";
        private const int port = 4444;
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();
        #endregion

        #region Constructor(s)
        public GamePlayScreen(Game game, GameStateManager stateManager)
            : base(game, stateManager)
        {
            AllocConsole();  
            world = new World(game, gameReference.screenRectangle);
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
                                                                                               // Open a console for ease of debugging

            try
            {
                client = new Client();
                if (!client.Connect(hostname, port))
                {
                    Console.WriteLine("Could not connect. Ensure the server is running");                              //TODO: Lobby, Connection GameState
                    stateManager.PopState();
                }
                else
                {
                    Thread clientThread = new Thread(Client.Run);
                    clientThread.Start();
                }  
            }
            catch (Exception)
            {
                Console.WriteLine("An error has occured.");
                stateManager.PopState();
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            //            Texture2D spriteSheet = Game.Content.Load<Texture2D>(@"Graphics\Sprites\" +
            //                gameReference.characterCreationScreen.SelectGender +
            //                gameReference.characterCreationScreen.SelectClass);
            //                
            //            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            //
            //            Animation animation = new Animation(3, 32, 32, 0, 0);
            //            animations.Add(AnimationKey.Down, animation);
            //            animation = new Animation(3, 32, 32, 0, 32);
            //            animations.Add(AnimationKey.Left, animation);
            //            animation = new Animation(3, 32, 32, 0, 64);
            //            animations.Add(AnimationKey.Right, animation);
            //            animation = new Animation(3, 32, 32, 0, 96);
            //            animations.Add(AnimationKey.Up, animation);
            //            AnimatedSprite animatedSprite = new AnimatedSprite(spriteSheet, animations);
            //            player = new Player(gameReference, animatedSprite);
            //            base.LoadContent();
            //
            //            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_1");
            //            TileSet tileSet1 = new TileSet(tilesetTexture, 16, 16, 10, 28);
            //
            //            tilesetTexture = Game.Content.Load<Texture2D>(@"Graphics\Tiles\tileSet_2");
            //            TileSet tileSet2 = new TileSet(tilesetTexture, 16, 16, 10, 28);
            //
            //            List<TileSet> tilesets = new List<TileSet> {tileSet1, tileSet2};
            //
            //            MapLayer mapLayer = new MapLayer(100, 100);
            //
            //            for (int y = 0; y < mapLayer.height; y++)
            //            {
            //                for (int x = 0; x < mapLayer.width; x++)
            //                {
            //                    Tile tile = new Tile(33, 0);
            //                    mapLayer.SetTile(x, y, tile);
            //                }
            //            }
            //
            //            MapLayer splatter = new MapLayer(100, 100);
            //            Random random = new Random();
            //            for (int i = 0; i < 100; i++)
            //            {
            //                int x = random.Next(0, 100);
            //                int y = random.Next(0, 100);
            //                int index = random.Next(229, 230);
            //                Tile tile = new Tile(index, 0);
            //                splatter.SetTile(x, y, tile);
            //            }
            //
            //            splatter.SetTile(1, 0, new Tile(0, 1));
            //            splatter.SetTile(2, 0, new Tile(2, 1));
            //            splatter.SetTile(3, 0, new Tile(0, 1));
            //
            //            List<MapLayer> mapLayers = new List<MapLayer> {mapLayer, splatter};
            //
            //            TileMap map = new TileMap(tilesets, mapLayers);
            //            Level level = new Level(map);
            //            world.levels.Add(level);
            //            world.CurrentLevel = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.P))
            {
                stateManager.PushState(gameReference.pauseScreen);
            }

            world.Update(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            gameReference.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                null, null, null, player.camera.Transformation);

            world.DrawLevel(gameReference.spriteBatch, player.camera);
            player.Draw(gameTime, gameReference.spriteBatch);
            base.Draw(gameTime);
            gameReference.spriteBatch.End();
        }
        #endregion
    }
}
