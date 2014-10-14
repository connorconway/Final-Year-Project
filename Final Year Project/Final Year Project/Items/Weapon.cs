using System;
using System.Globalization;
using System.Linq;

namespace Final_Year_Project.Items
{
    public class Weapon : BaseItem
    {
        #region Variables
        public Hands hands { get; set; }
        public int attackValue { get; set; }
        public int attackModifier { get; set; }
        public int damageValue { get; set; }
        public int damageModifier { get; set; }
        #endregion

        #region Constructor(s)
        public Weapon(string name, string type, int price, float weight, Hands hands, int attackValue, int attackModifier, int damageValue, int damageModifier, string[] allowableClasses)
            : base(name, type, price, weight, allowableClasses)
        {
            this.hands = hands;
            this.attackValue = attackValue;
            this.attackModifier = attackModifier;
            this.damageValue = damageValue;
            this.damageModifier = damageModifier;
        }
        #endregion

        #region Override Method(s)
        public override object Clone()
        {
            string[] allowedClasses = new string[allowableClasses.Count];
            for (int i = 0; i < allowableClasses.Count; i++)
                allowedClasses[i] = allowableClasses[i];
            Weapon weapon = new Weapon(name, type, price, weight, hands, attackValue, attackModifier, damageValue, damageModifier, allowedClasses);
            return weapon;
        }

        public override string ToString()
        {
            string weaponString = base.ToString() + ", ";
            weaponString += hands + ", ";
            weaponString += attackValue.ToString(CultureInfo.InvariantCulture) + ", ";
            weaponString += attackModifier.ToString(CultureInfo.InvariantCulture) + ", ";
            weaponString += damageValue.ToString(CultureInfo.InvariantCulture) + ", ";
            weaponString += damageModifier.ToString(CultureInfo.InvariantCulture);

            return allowableClasses.Aggregate(weaponString, (current, t) => current + (", " + t));
        }
        #endregion
    }
}
