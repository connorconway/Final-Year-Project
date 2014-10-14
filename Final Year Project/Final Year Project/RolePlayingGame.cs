namespace Final_Year_Project
{
    public class RolePlayingGame
    {
        #region Variables
        public string name { get; set; }
        public string description { get; set; }
        #endregion

        #region Constructor(s)
        public RolePlayingGame(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public RolePlayingGame()
        {
        }
        #endregion
    }
}
