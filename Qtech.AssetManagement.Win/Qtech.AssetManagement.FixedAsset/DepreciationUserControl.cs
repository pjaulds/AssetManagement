using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Bll;
using System.Globalization;
using Qtech.AssetManagement.Utilities;

namespace Qtech.AssetManagement.FixedAsset
{
    public partial class DepreciationUserControl : UserControl
    {
        public DepreciationUserControl()
        {
            InitializeComponent();
        }

        private int _mFixedAssetId { get; set; }
        public int mMonth { get; set; }
        public short mYear { get; set; }

        public void Save()
        {
            SaveDepreciationJournal(IsDepreciationJournalExists());
        }

        private int SaveDepreciationJournal(int id)
        {
            BusinessEntities.DepreciationJournal item = new BusinessEntities.DepreciationJournal();
            item.mId = id;
            LoadDepreciationJournalFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                id = DepreciationJournalManager.Save(item);
                return id;
            }
            else
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = item.BrokenRules;
                validationForm.ShowDialog();
                return 0;
            }
        }

        private void LoadDepreciationJournalFromFormControls(BusinessEntities.DepreciationJournal myDepreciationJournal)
        {
            myDepreciationJournal.mFixedAssetId = _mFixedAssetId;
            myDepreciationJournal.mYear = mYear;
            myDepreciationJournal.mMonth = (byte)mMonth;

            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(_mFixedAssetId);
            FixedAssetSetting faType = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            myDepreciationJournal.mDepreciationExpenseAccountId = faType.mDepreciationExpenseAccountId;
            myDepreciationJournal.mDepreciationExpenseAccountName = faType.mDepreciationExpenseAccountName;
            myDepreciationJournal.mDepreciationExpenseAccountDebitCredit = DepreciationExpensecheckBox.Checked;

            myDepreciationJournal.mAccumulatedDepreciationAccountId = faType.mAccumulatedDepreciationAccountId;
            myDepreciationJournal.mAccumulatedDepreciationAccountName = faType.mAccumulatedDepreciationAccountName;
            myDepreciationJournal.mAccumulatedDepreciationAccountDebitCredit = AccumulatedDepreciationcheckBox.Checked;

            myDepreciationJournal.mAmount = Convert.ToDecimal(DepreciationExpenseAmountlabel.Text);

            DateTime date = new DateTime(mYear, mMonth, 1);

            myDepreciationJournal.mDescription = "Depreciation for the month of " + date.ToString("MMMMM") + " " + mYear.ToString();
            myDepreciationJournal.mUserId = SessionUtil.mUser.mId;

        }

        private int IsDepreciationJournalExists()
        {
            DepreciationJournalCriteria criteria = new DepreciationJournalCriteria();
            criteria.mFixedAssetId = _mFixedAssetId;
            criteria.mYear = mYear;
            criteria.mMonth = (byte)mMonth;

            if (DepreciationJournalManager.SelectCountForGetList(criteria) > 0)
                return DepreciationJournalManager.GetList(criteria).First().mId;
            else return 0;
        }

        public void LoadFormControlsFromFixedAsset(int fixedAssetId)
        {
            _mFixedAssetId = fixedAssetId;
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(fixedAssetId);

            NametextBox.Text = fa.mProductName;

            PurchaseDatetextBox.Text = fa.mPurchaseDate.ToString("D");
            EndDepreciationDatetextBox.Text = fa.mPurchaseDate.AddYears((int)fa.mUsefulLifeYears).AddDays(-1).ToString("D");
            PurchaseCosttextBox.Text = fa.mPurchasePrice.ToString("N");
            ResidualValuetextBox.Text = fa.mResidualValue.ToString("N");
            UsefulLifetextBox.Text = fa.mUsefulLifeYears.ToString("N");
            DepreciationMethodtextBox.Text = fa.mDepreciationMethodName;
            AveragingMethodtextBox.Text = fa.mAveragingMethodName;

            LoadFormControlsFromAssetType(fa);
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            DepreciationExpenseAccountCodelabel.Text = item.mChartOfAccountCode;
            DepreciationExpenseAccountTitlelabel.Text = item.mChartOfAccountName;

            AccumulatedDepreciationAccountCodelabel.Text = item.mAccumulatedDepreciationAccountCode;
            AccumulatedDepreciationAccountTitlelabel.Text = item.mAccumulatedDepreciationAccountName;

            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = fa.mId;
            criteria.mYear = mYear;
            criteria.mAssetTypeId = item.mAssetTypeId;

            decimal amount = 0;
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
            {
                string monthName = new DateTime(2010, mMonth, 1).ToString("MMM", CultureInfo.InvariantCulture); //get month name base on number

                if (!dt.Rows[0].IsNull(monthName))
                    amount = Convert.ToDecimal(dt.Rows[0][monthName]);
                // else
                //   MessageBox.Show("Depreciation journal with selected period does not exists.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            DepreciationExpenseAmountlabel.Text = amount.ToString("N");
            AccumulatedDepreciationAmountlabel.Text = amount.ToString("N");
            
        }
    }
}
