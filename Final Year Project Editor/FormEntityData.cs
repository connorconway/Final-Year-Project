using System;
using System.Globalization;
using System.Windows.Forms;
using Final_Year_Project.CharacterClasses;

namespace Final_Year_Project_Editor
{
    public partial class FormEntityData : Form
    {
        #region Variables
        public EntityData entityData { get; private set; }
        #endregion

        #region Constructor(s)
        public FormEntityData()
        {
            InitializeComponent();
            Load += FormEntityData_Load;
            btnOK.Click += btnOK_Click;
            btnCancel.Click += btnCancel_Click;
        }
        #endregion

        #region Event Handlers
        void FormEntityData_Load(object sender, EventArgs e)
        {
            if (entityData == null) return;

            tbName.Text = entityData.type;
            mtbStrength.Text = entityData.strength.ToString(CultureInfo.InvariantCulture);
            mtbDexterity.Text = entityData.dexterity.ToString(CultureInfo.InvariantCulture);
            mtbCunning.Text = entityData.cunning.ToString(CultureInfo.InvariantCulture);
            mtbWillpower.Text = entityData.willpower.ToString(CultureInfo.InvariantCulture);
            mtbConstitution.Text = entityData.constitution.ToString(CultureInfo.InvariantCulture);
            tbHealth.Text = entityData.healthFormula;
            tbStamina.Text = entityData.staminaFormula;
            tbMana.Text = entityData.magicFormula;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbHealth.Text) ||
                string.IsNullOrEmpty(tbStamina.Text) || string.IsNullOrEmpty(tbMana.Text))
            {
                MessageBox.Show("Name, Health Formula, Stamina Formula, and Mana Formula must have values.");
                return;
            }

            int strength, dexterity, cunning, willpower, magic, constitution = 0;

            if (!int.TryParse(mtbStrength.Text, out strength))
            {
                MessageBox.Show("Strength must be numeric.");
                return;
            }

            if (!int.TryParse(mtbDexterity.Text, out dexterity))
            {
                MessageBox.Show("Dexterity must be numeric.");
                return;
            }

            if (!int.TryParse(mtbCunning.Text, out cunning))
            {
                MessageBox.Show("Cunning must be numeric.");
                return;
            }

            if (!int.TryParse(mtbWillpower.Text, out willpower))
            {
                MessageBox.Show("Willpower must be numeric.");
                return;
            }

            if (!int.TryParse(mtbMagic.Text, out magic))
            {
                MessageBox.Show("Magic must be numeric.");
                return;
            }

            if (!int.TryParse(mtbConstitution.Text, out constitution))
            {
                MessageBox.Show("Constitution must be numeric.");
                return;
            }

            entityData = new EntityData(
                tbName.Text, strength, dexterity, cunning, willpower, magic,
                constitution, tbHealth.Text, tbStamina.Text, tbHealth.Text);

            Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            entityData = null;
            Close();
        }
        #endregion
    }
}
