using System;

namespace Final_Year_Project.GameData
{
    [Serializable()]
    public class SaveData
    {
        #region Variables
        public String playerGender { get; set; }
        public String playerClass { get; set; }
        public String playerXPos { get; set; }
        public String playerYPos { get; set; }
        #endregion

        #region Constructor(s)
        public SaveData(String playerGender, String playerClass, String playerXPos, String playerYPos)
        {
            this.playerGender = playerGender;
            this.playerClass = playerClass;
            this.playerXPos = playerXPos;
            this.playerYPos = playerYPos;
        }
        #endregion
    }
}
