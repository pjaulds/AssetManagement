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
    public partial class DisposalJournalForm : Form
    {
        public DisposalJournalForm()
        {
            InitializeComponent();
        }

        public int mFixedAssetId { get; set; }
                
        public string mDateDisposed { set { DateDisposedtextBox.Text = value; } }
        public string mSalesProceeds { set { SalesProceedstextBox.Text = value; SalesProceeds2textBox.Text = value; CashAmountlabel.Text = value; } }

        public int mCashAccountId { get; set; }
        public string mCashAccount { set { CashAccounttextBox.Text = value; } }

        public int mGainLossAccountId { get; set; }
        public string mGainLossAccount { set { GainLossAccounttextBox.Text = value; } }
        private int SaveDisposal()
        {
            BusinessEntities.Disposal item = new BusinessEntities.Disposal();
            LoadDisposalFromFormControls(item);
            return DisposalManager.Save(item);
        }

        private void LoadDisposalFromFormControls(BusinessEntities.Disposal myDisposal)
        {
            myDisposal.mFixedAssetId = mFixedAssetId;
            myDisposal.mDateDisposed = Convert.ToDateTime(DateDisposedtextBox.Text);
            myDisposal.mSalesProceeds = ControlUtil.TextBoxDecimal(SalesProceedstextBox);
            myDisposal.mCashAccountId = mCashAccountId;
            myDisposal.mGainLossAccountId = mGainLossAccountId;
            myDisposal.mDate = DateTime.Now.Date;
            myDisposal.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mFixedAssetId);
            CosttextBox.Text = fa.mPurchasePrice.ToString("N");
            AssetAmountlabel.Text = CosttextBox.Text;

            LoadFormControlsFromAssetType(fa);
            LoadDepreciationFromFormControls(fa);

            BusinessEntities.ChartOfAccount ca = ChartOfAccountManager.GetItem(mCashAccountId);
            CashAccountCodelabel.Text = ca.mCode;
            CashAccountTitlelabel.Text = ca.mName;

            ca = ChartOfAccountManager.GetItem(mGainLossAccountId);
            GainLossCashAccountCodelabel.Text = ca.mCode;
            GainLossAccountTitlelabel.Text = ca.mName;
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            BusinessEntities.FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            AssetCashAccountCodelabel.Text = item.mChartOfAccountCode;
            AssetAccountTitlelabel.Text = item.mChartOfAccountName;
            
            AccumulatedDepreciationAccountCodelabel.Text = item.mAccumulatedDepreciationAccountCode; 
            AccumulatedDepreciationAccountTitlelabel.Text = item.mAccumulatedDepreciationAccountName;
        }

        private void LoadDepreciationFromFormControls(BusinessEntities.FixedAsset fa)
        {
            if(fa.mDepreciationMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if (fa.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    ReportCriteria criteria = new ReportCriteria();
                    DateTime dateEnd = Convert.ToDateTime(DateDisposedtextBox.Text);
                    criteria.mId = fa.mId;
                    criteria.mYear = (short)dateEnd.Year;
                    criteria.mEndDate = new DateTime(dateEnd.Year, dateEnd.Month, 1).AddDays(-1);

                    AccumulatedDepreciationAccounttextBox.Text = Convert.ToDecimal(ReportManager.DepreciationScheduleStraightLineFullMonthMonthly(criteria).Rows[0]["Ending"]).ToString("N");
                }
            }

            AccumulatedDepreciationAmountlabel.Text = AccumulatedDepreciationAccounttextBox.Text;
            
            decimal cost = Convert.ToDecimal(CosttextBox.Text);
            decimal depreciation = Convert.ToDecimal(AccumulatedDepreciationAccounttextBox.Text);
            decimal salesProceeds = Convert.ToDecimal(SalesProceedstextBox.Text);
            decimal gainLoss = salesProceeds - (cost - depreciation);
            GainLossDisposaltextBox.Text = gainLoss.ToString("N");
            GainLossAmountlabel.Text = GainLossDisposaltextBox.Text;
        }

        private void DisposalJournalForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisposalJournalForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            LoadFormControlsFromFixedAsset();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            //check if exists
            DisposalCriteria criteria = new DisposalCriteria();
            criteria.mFixedAssetId = mFixedAssetId;
            if(DisposalManager.SelectCountForGetList(criteria)>0)
            {
                MessageBox.Show("Asset were already disposed.\nCannot continue", "Disposal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (SaveDisposal() > 0)
            {
                MessageBox.Show("Disposal save successfully.", "Disposal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }
    }
}
