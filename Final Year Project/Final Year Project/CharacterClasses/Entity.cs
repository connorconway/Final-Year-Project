namespace Final_Year_Project.CharacterClasses
{
    public enum EntityGender
    {
        Male,
        Female,
        Unknown
    }

    public enum EntityType
    {
        Character,
        NPC,
        Monster,
        Creature
    }

    class Entity
    {
        #region Variables

        private string name { get; set; }
        private string entityClass { get; set; }
        private EntityType type { get; set; }
        private EntityGender gender { get; set; }

        private int strength { get; set; }
        private int dexterity { get; set; }
        private int cunning { get; set; }
        private int willpower { get; set; }
        private int magic { get; set; }
        private int constitution { get; set; }

        public int strengthModifier { get; set; }
        public int dexterityModifier { get; set; }
        public int cunningModifier { get; set; }
        public int willpowerModifier { get; set; }
        public int magicModifier { get; set; }
        public int constitutionModifier { get; set; }

        private AttributePair health { get; set; }
        private AttributePair stamina { get; set; }
        private AttributePair mana { get; set; }

        public int attack { get; set; }
        public int damage { get; set; }
        public int defense { get; set; }
        public int level { get; set; }
        public long experience { get; set; }
        #endregion

        #region Constructor(s)
        private Entity()
        {
            strength = dexterity = cunning = willpower = magic = constitution = 0;
            health = stamina = mana = new AttributePair(0);
        }

        public Entity(string name, EntityData entityData, EntityGender gender, EntityType type)
        {
            this.name = name;
            entityClass = entityData.type;
            this.gender = gender;
            this.type = type;
            strength = entityData.strength;
            dexterity = entityData.dexterity;
            cunning = entityData.cunning;
            willpower = entityData.willpower;
            magic = entityData.magic;
            constitution = entityData.constitution;
            health = stamina = mana = new AttributePair(0);
        }
        #endregion
    }
}
