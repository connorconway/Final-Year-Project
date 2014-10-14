using System;
using System.Windows.Forms;
using Final_Year_Project;

namespace Final_Year_Project_Editor
{
    public partial class FormNewGame : Form
    {
        #region Variables
        private RolePlayingGame rpg { get; set; }
        #endregion

        #region Constructor(s)
        public FormNewGame()
        {
            InitializeComponent();
            btnOK.Click += btnOK_Click_1;
        }
        #endregion

        #region Event Handlers
        void btnOK_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbDescription.Text))
            {
                MessageBox.Show("You must enter a name and a description.", "Error");
                return;
            }
            rpg = new RolePlayingGame(tbName.Text, tbDescription.Text);
            Close();
        }
        #endregion
    }
}
