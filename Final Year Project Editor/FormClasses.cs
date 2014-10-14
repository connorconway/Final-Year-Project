using System;
using System.Windows.Forms;
using Final_Year_Project.CharacterClasses;

namespace Final_Year_Project_Editor
{
    public partial class FormClasses : Form
    {
        #region Variables
        EntityDataManager entityDataManager = new EntityDataManager();
        #endregion

        #region Constructor(s)
        public FormClasses()
        {
            InitializeComponent();
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
        }
        #endregion

        #region Event Handlers
        void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            using (FormEntityData frmEntityData = new FormEntityData())
            {
                frmEntityData.ShowDialog();
                if (frmEntityData.entityData != null)
                {
                    lbClasses.Items.Add(frmEntityData.entityData.ToString());
                }
            }
        }

        void btnEdit_Click(object sender, EventArgs e)
        {
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
