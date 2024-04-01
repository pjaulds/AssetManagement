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
    public partial class CapitalizeJournalizedForm : Form
    {
        public CapitalizeJournalizedForm()
        {
            InitializeComponent();
        }

        public bool mAcceptInput { get; set; }
        public int mAssetAccountId {
            get { return ControlUtil.UltraComboReturnValue(AssetAccountutraCombo); }
            set { AssetAccountutraCombo.Value = value; }
        }

        public int mCashPayableAccountId
        {
            get { return ControlUtil.UltraComboReturnValue(CashPayableAccountultraCombo); }
            set { CashPayableAccountultraCombo.Value = value; }
        }

        private void CapitalizeJournalizedForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            mAcceptInput = true;
            Close();
        }

        private void CapitalizeJournalizedForm_Load(object sender, EventArgs e)
        {
            UltraComboUtil.ChartOfAccount(AssetAccountutraCombo);
            UltraComboUtil.ChartOfAccount(CashPayableAccountultraCombo);
        }
    }
}
