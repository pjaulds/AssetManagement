using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
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
            LoadFormControlsFromBookValue();
        }

        private void LoadFormControlsFromBookValue()
        {
            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mFixedAssetId;
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mFixedAssetId);

            Titlelabel.Text= "Dispose " + fa.mAssetNo + " - " + fa.mProductName;

            criteria.mYear = (short)AuditManager.GetDateToday().Year;
            criteria.mAssetTypeId = fa.mAssetTypeId;

            DataTable dt = new DataTable();
            if (fa.mDepreciationMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if (fa.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                    dt = ReportManager.DepreciationScheduleStraightLineFullMonthMonthly(criteria);
                else if (fa.mAveragingMethodId == (int)AveragingMethodEnum.ActualDays)
                    dt = ReportManager.DepreciationScheduleStraightLineActualDaysMonthly(criteria);

            }

            if (fa.mDepreciationMethodId == (int)DepreciationMethodEnum.SYD)
            {
                if (fa.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                    dt = ReportManager.DepreciationScheduleSYDFullMonthMonthly(criteria);
                else if (fa.mAveragingMethodId == (int)AveragingMethodEnum.ActualDays)
                    dt = ReportManager.DepreciationScheduleSYDActualDaysMonthly(criteria);
            }
            

            if (dt.Rows.Count > 0)
                BookValuetextBox.Text = Convert.ToDecimal(dt.Rows[0]["BookValueEnd"]).ToString("N");
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
