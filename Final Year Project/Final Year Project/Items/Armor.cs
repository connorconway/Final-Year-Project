using System;
using System.Globalization;
using System.Linq;

namespace Final_Year_Project.Items
{
    public class Armor : BaseItem
    {
        #region Variables
        private ArmorLocation location { get; set; }
        private int defenseValue { get; set; }
        private int defenseModifier { get; set; }
        #endregion

        #region Constructor(s)
        public Armor(string name, string type, int price, float weight, ArmorLocation location, int defenseValue, int defenseModifier, params string[] allowableClasses)
            : base(name, type, price, weight, allowableClasses)
        {
            this.location = location;
            this.defenseValue = defenseValue;
            this.defenseModifier = defenseModifier;
        }
        #endregion

        #region Override Methods
        public override object Clone()
        {
            string[] allowedClasses = new string[allowableClasses.Count];

            for (int i = 0; i < allowableClasses.Count; i++)
                allowedClasses[i] = allowableClasses[i];

            Armor armor = new Armor(name, type, price, weight, location, defenseValue, defenseModifier, allowedClasses);

            return armor;
        }

        public override string ToString()
        {
            string armorString = base.ToString() + ", ";
            armorString += location + ", ";
            armorString += defenseValue.ToString(CultureInfo.InvariantCulture) + ", ";
            armorString += defenseModifier.ToString(CultureInfo.InvariantCulture);

            return allowableClasses.Aggregate(armorString, (current, t) => current + (", " + t));
        }
        #endregion
    }
}
