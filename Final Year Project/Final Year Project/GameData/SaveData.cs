using System;
using Final_Year_Project.Annotations;

namespace Final_Year_Project.GameData
{
    [Serializable]
    public class SaveData
    {
        private String playerGender { [UsedImplicitly] get; set; }
        private String playerClass  { [UsedImplicitly] get; set; }
        private String playerXPos   { [UsedImplicitly] get; set; }
        private String playerYPos   { [UsedImplicitly] get; set; }

        public SaveData(String playerGender, String playerClass, String playerXPos, String playerYPos)
        {
            this.playerGender = playerGender;
            this.playerClass  = playerClass;
            this.playerXPos   = playerXPos;
            this.playerYPos   = playerYPos;
        }
    }
}