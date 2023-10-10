using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.FixedAsset
{
    public partial class RunDepreciationForm : Form
    {
        public RunDepreciationForm()
        {
            InitializeComponent();
        }

        public int mFaId { get; set; }

        public byte mMonth {
            get { return (byte)(MonthcomboBox.SelectedIndex + 1); }
        }
        public short mYear {
            get { return (short)YearnumericUpDown.Value; }
        }

        private void RunDepreciationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void RunDepreciationForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mFaId);
            NametextBox.Text = fa.mAssetNo + " " + fa.mProductName;
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (MonthcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select month", "Depreciation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DepreciationJournalForm dep = new DepreciationJournalForm();
            dep.mId = mFaId;
            dep.mMonth = (byte)(MonthcomboBox.SelectedIndex + 1);
            dep.mYear  = (short)YearnumericUpDown.Value;
            dep.ShowDialog();

            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
