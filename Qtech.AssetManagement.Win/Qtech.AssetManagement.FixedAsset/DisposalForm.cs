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
    public partial class DisposalForm : Form
    {
        public DisposalForm()
        {
            InitializeComponent();
        }

        public int mFixedAssetId { get; set; }

        public string mFixedAssetName {
            set { Titlelabel.Text = "Dispose " + value; }
        }

        private void DisposalForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisposalForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            UltraComboUtil.ChartOfAccount(CashAccountultraCombo);
            UltraComboUtil.ChartOfAccount(GainLossAccountultraCombo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal salesProceeds;
            decimal.TryParse(SaleProceedstextBox.Text, out salesProceeds);

            DisposalJournalForm journalForm = new DisposalJournalForm();
            journalForm.mFixedAssetId = mFixedAssetId;
            journalForm.mDateDisposed = DateDisposeddateTimePicker.Value.ToString("D");
            journalForm.mSalesProceeds = salesProceeds.ToString("N");
            journalForm.mCashAccountId = ControlUtil.UltraComboReturnValue(CashAccountultraCombo);
            journalForm.mCashAccount = CashAccountultraCombo.Text;
            journalForm.mGainLossAccountId = ControlUtil.UltraComboReturnValue(GainLossAccountultraCombo);
            journalForm.mGainLossAccount = GainLossAccountultraCombo.Text;
            journalForm.ShowDialog();
        }
    }
}
