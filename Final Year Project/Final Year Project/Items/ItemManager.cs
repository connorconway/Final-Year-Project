using System.Collections.Generic;

namespace Final_Year_Project.Items
{
    public class ItemManager
    {
        #region Variables
        readonly Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
        readonly Dictionary<string, Armor> armors = new Dictionary<string, Armor>();
        readonly Dictionary<string, Shield> shields = new Dictionary<string, Shield>();

        public Dictionary<string, Weapon>.KeyCollection WeaponKeys
        {
            get { return weapons.Keys; }
        }
        public Dictionary<string, Armor>.KeyCollection ArmorKeys
        {
            get { return armors.Keys; }
        }
        public Dictionary<string, Shield>.KeyCollection ShieldKeys
        {
            get { return shields.Keys; }
        }
        #endregion

        #region Getter(s) and Setter(s)
        public void AddWeapon(Weapon weapon)
        {
            if (!weapons.ContainsKey(weapon.name))
            {
                weapons.Add(weapon.name, weapon);
            }
        }

        public Weapon GetWeapon(string name)
        {
            if (weapons.ContainsKey(name))
            {
                return (Weapon)weapons[name].Clone();
            }
            return null;
        }

        public bool ContainsWeapon(string name)
        {
            return weapons.ContainsKey(name);
        }

        public void AddArmor(Armor armor)
        {
            if (!armors.ContainsKey(armor.name))
            {
                armors.Add(armor.name, armor);
            }
        }

        public Armor GetArmor(string name)
        {
            if (armors.ContainsKey(name))
            {
                return (Armor)armors[name].Clone();
            }
            return null;
        }

        public bool ContainsArmor(string name)
        {
            return armors.ContainsKey(name);
        }

        public void AddShield(Shield shield)
        {
            if (!shields.ContainsKey(shield.name))
            {
                shields.Add(shield.name, shield);
            }
        }

        public Shield GetShield(string name)
        {
            if (shields.ContainsKey(name))
            {
                return (Shield)shields[name].Clone();
            }
            return null;
        }

        public bool ContainsShield(string name)
        {
            return shields.ContainsKey(name);
        }
        #endregion

        #region Constructor(s)
        public ItemManager()
        {
            
        }
        #endregion
    }
}
