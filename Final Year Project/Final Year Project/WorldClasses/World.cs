using System;
using System.Collections.Generic;
using Final_Year_Project.Items;
using Final_Year_Project.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Year_Project.WorldClasses
{
    public class World : DrawableGameComponent
    {
        #region Variables
        public Rectangle screenRectangle { get; set; }
        public ItemManager itemManager = new ItemManager();
        public static readonly List<Level> levels = new List<Level>();
        int currentLevel = -1;
        #endregion

        #region Getter(s) and Setter(s)
        public List<Level> Levels
        {
            get { return levels; }
        }


        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                if (value < 0 || value >= levels.Count)
                    throw new IndexOutOfRangeException();
                if (levels[value] == null)
                    throw new NullReferenceException();
                currentLevel = value;
            }
        }
        #endregion

        #region Contructor(s)
        public World(Game game, Rectangle screenRectangle)
            : base(game)
        {
            this.screenRectangle = screenRectangle;
        }
        #endregion

        #region Override Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        #region General Methods
        public void DrawLevel(SpriteBatch spriteBatch, Camera camera)
        {
            levels[currentLevel].Draw(spriteBatch, camera);
        }
        #endregion
    }
}
