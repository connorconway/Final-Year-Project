namespace Final_Year_Project.CharacterClasses
{
    class AttributePair
    {
        #region Variables
        private int currentValue { get; set; }
        private int maximumValue { get; set; }
        public static AttributePair Zero
        {
            get { return new AttributePair(); }
        }
        #endregion

        #region Constructor(s)
        private AttributePair()
        {
            currentValue = 0;
            maximumValue = 0;
        }

        public AttributePair(int maximumValue)
        {
            currentValue = maximumValue;
            this.maximumValue = maximumValue;
        }
        #endregion

        #region General Methods
        public void Heal(int value)
        {
            currentValue += value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void Damage(int value)
        {
            currentValue -= value;
            if (currentValue < 0)
                currentValue = 0;
        }

        public void SetCurrent(int value)
        {
            currentValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }

        public void SetMaximum(int value)
        {
            maximumValue = value;
            if (currentValue > maximumValue)
                currentValue = maximumValue;
        }
        #endregion

    }
}
