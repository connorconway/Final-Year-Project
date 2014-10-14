using System.Collections.Generic;

namespace Final_Year_Project.CharacterClasses
{
    public class EntityDataManager
    {
        #region Variables
        readonly Dictionary<string, EntityData> entityDatas = new Dictionary<string, EntityData>();

        public Dictionary<string, EntityData> EntityDatas
        {
            get { return entityDatas; }
        }
        #endregion

        #region Constructor(s)
        #endregion
    }
}