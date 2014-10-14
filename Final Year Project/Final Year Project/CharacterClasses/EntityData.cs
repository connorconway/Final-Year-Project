using System.Globalization;

namespace Final_Year_Project.CharacterClasses
{
    public class EntityData
    {
        #region Variables
        public string type;                             // Type of entity
        public int strength;                            // How strong the entity is
        public int dexterity;                           // Measure of the entities agility
        public int cunning;                             // Measure of the entities mental reasoning and perception
        public int willpower;                           // The entities stamina and mana / How long before the entity tires in battle
        public int magic;                               // Effectiveness of magic / healing spells
        public int constitution;                        // Determines how healthy an entity is
        public string healthFormula;                    // Determine the entities health
        public string staminaFormula;                   // Determine the entities stamina
        public string magicFormula;                     // Determine the entities magic
        #endregion

        #region Constructor(s)
        private EntityData()
        {
        }

        public EntityData(string type, int strength, int dexterity, int cunning, int willpower, int magic, int constitution,
            string healthFormula, string staminaFormula, string magicFormula)
        {
            this.type = type;
            this.strength = strength;
            this.dexterity = dexterity;
            this.cunning = cunning;
            this.willpower = willpower;
            this.magic = magic;
            this.constitution = constitution;
            this.healthFormula = healthFormula;
            this.staminaFormula = staminaFormula;
            this.magicFormula = magicFormula;
        }
        #endregion

        #region Override Methods
        public override string ToString()
        {
            string toString = "";
            toString += "Name = " + type + ", ";
            toString += "Strength = " + strength.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Dexterity = " + dexterity.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Cunning = " + cunning.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Willpower = " + willpower.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Magic = " + magic.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Constitution = " + constitution.ToString(CultureInfo.InvariantCulture) + ", ";
            toString += "Health Formula = " + healthFormula + ", ";
            toString += "Stamina Formula = " + staminaFormula + ", ";
            toString += "Magic Formula = " + magicFormula;
            return toString;
        }
        #endregion

        #region General Methods
        public object Clone()
        {
            return new EntityData
            {
                type = type,
                strength = strength,
                dexterity = dexterity,
                cunning = cunning,
                willpower = willpower,
                magic = magic,
                constitution = constitution,
                healthFormula = healthFormula,
                staminaFormula = staminaFormula,
                magicFormula = magicFormula
            };
        }
        #endregion
    }
}
