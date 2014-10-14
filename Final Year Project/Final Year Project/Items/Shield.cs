using System;
using System.Globalization;
using System.Linq;

namespace Final_Year_Project.Items
{
    public class Shield : BaseItem
    {
        #region Variables
        private int defenseValue { get; set; }
        private int defenseModifier { get; set; }
        #endregion

        #region Constructor(s)
        public Shield(string name, string type, int price, float weight, int defenseValue, int defenseModifier, params string[] allowableClasses)
            : base(name, type, price, weight, allowableClasses)
        {
            this.defenseModifier = defenseModifier;
            this.defenseValue = defenseValue;
        }
        #endregion

        #region Override Method(s)
        public override object Clone()
        {
            string[] allowedClasses = new string[allowableClasses.Count];

            for (int i = 0; i < allowableClasses.Count; i++)
                allowedClasses[i] = allowableClasses[i];

            Shield shield = new Shield(name, type, price, weight, defenseValue, defenseModifier, allowedClasses);

            return shield;
        }

        public override string ToString()
        {
            string shieldString = base.ToString() + ", ";
            shieldString += defenseValue.ToString(CultureInfo.InvariantCulture) + ", ";
            shieldString += defenseModifier.ToString(CultureInfo.InvariantCulture);

            return allowableClasses.Aggregate(shieldString, (current, t) => current + (", " + t));
        }
        #endregion
    }
}
