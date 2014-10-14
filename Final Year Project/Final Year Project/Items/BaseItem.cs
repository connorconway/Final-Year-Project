using System;
using System.Collections.Generic;
using System.Globalization;

namespace Final_Year_Project.Items
{
    public enum Hands
    {
        One,
        Two
    }

    public enum ArmorLocation
    {
        Body,
        Head,
        Hands,
        Feet
    }

    public abstract class BaseItem
    {
        #region Variables
        protected List<string> allowableClasses = new List<string>();
        public String name { get; set; }
        public String type { get; set; }
        public int price { get; set; }
        public float weight { get; set; }
        public bool equipped { get; set; }
        #endregion

        #region Getter(s) and Setter(s)
        public List<string> AllowableClasses
        {
            get { return allowableClasses; }
            protected set { allowableClasses = value; }
        }

        #endregion

        #region Constructor(s)
        public BaseItem(string name, String type, int price, float weight, params string[] allowableClasses)
        {
            foreach (string allowableClass in allowableClasses)
            {
                AllowableClasses.Add(allowableClass);
            }

            this.name = name;
            this.type = type;
            this.price = price;
            this.weight = weight;
            equipped = false;
        }
        #endregion

        #region General Method(s)
        public abstract object Clone();

        public virtual bool CanEquip(string characterType)
        {
            return allowableClasses.Contains(characterType);
        }

        public override string ToString()
        {
            string itemString = "";
            itemString += name + ", ";
            itemString += type + ", ";
            itemString += price.ToString(CultureInfo.InvariantCulture) + ", ";
            itemString += weight.ToString(CultureInfo.InvariantCulture);
            return itemString;
        }
        #endregion
    }
}
