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
    public partial class RunDepreciationAllForm : Form
    {
        public RunDepreciationAllForm()
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
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (MonthcomboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select month", "Depreciation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DepreciationJournalTempForm dep = new DepreciationJournalTempForm();
            dep.mMonth = mMonth;
            dep.mYear = mYear;
            dep.ShowDialog();

            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
