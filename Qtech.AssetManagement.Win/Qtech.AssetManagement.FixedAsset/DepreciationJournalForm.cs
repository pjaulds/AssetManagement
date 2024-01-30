using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.FixedAsset
{
    public partial class DepreciationJournalForm : Form
    {
        public DepreciationJournalForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// fixed asset id
        /// </summary>
        public int mId { get; set; }
        public byte mMonth { get; set; }
        public short mYear { get; set; }
        public int mDepreciationMethodId { get; set; }
        public int mAveragingMethodId { get; set; }

        private int SaveDepreciationJournal()
        {
            BusinessEntities.DepreciationJournal item = new BusinessEntities.DepreciationJournal();
            LoadDepreciationJournalFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = DepreciationJournalManager.Save(item);               
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
            myDepreciationJournal.mFixedAssetId = mId;
            myDepreciationJournal.mYear = mYear;
            myDepreciationJournal.mMonth = mMonth;

            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);
            FixedAssetSetting faType = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            myDepreciationJournal.mDepreciationExpenseAccountId = faType.mDepreciationExpenseAccountId;
            myDepreciationJournal.mDepreciationExpenseAccountName = faType.mDepreciationExpenseAccountName;
            myDepreciationJournal.mDepreciationExpenseAccountDebitCredit = DepreciationExpensecheckBox.Checked;

            myDepreciationJournal.mAccumulatedDepreciationAccountId = faType.mAccumulatedDepreciationAccountId;
            myDepreciationJournal.mAccumulatedDepreciationAccountName = faType.mAccumulatedDepreciationAccountName;
            myDepreciationJournal.mAccumulatedDepreciationAccountDebitCredit = AccumulatedDepreciationcheckBox.Checked;

            myDepreciationJournal.mAmount = Convert.ToDecimal(DepreciationExpenseAmountlabel.Text);
            myDepreciationJournal.mDescription = DescriptiontextBox.Text;
            myDepreciationJournal.mUserId = SessionUtil.mUser.mId;

        }

        private void LoadFormControlsFromFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);

            NametextBox.Text = fa.mProductName;
            
            PurchaseDatetextBox.Text = fa.mPurchaseDate.ToString("D");
            EndDepreciationDatetextBox.Text = fa.mPurchaseDate.AddYears((int)fa.mUsefulLifeYears).AddDays(-1).ToString("D");
            PurchaseCosttextBox.Text = fa.mPurchasePrice.ToString("N");
            ResidualValuetextBox.Text = fa.mResidualValue.ToString("N");
            UsefulLifetextBox.Text = fa.mUsefulLifeYears.ToString("N");
            DepreciationMethodtextBox.Text = fa.mDepreciationMethodName;
            AveragingMethodtextBox.Text = fa.mAveragingMethodName;

            LoadFormControlsFromAssetType(fa);

            DepreciationExpensecheckBox_CheckedChanged(this, new EventArgs());
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            DepreciationExpenseAccountCodelabel.Text = item.mChartOfAccountCode; 
            DepreciationExpenseAccountTitlelabel.Text = item.mChartOfAccountName;

            AccumulatedDepreciationAccountCodelabel.Text = item.mAccumulatedDepreciationAccountCode;
            AccumulatedDepreciationAccountTitlelabel.Text = item.mAccumulatedDepreciationAccountName;

            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mId;
            criteria.mYear = mYear;
            decimal amount = 0;
            if (mDepreciationMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if(mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                {                    
                    DataTable dt = ReportManager.DepreciationScheduleStraightLineFullMonthMonthly(criteria);
                    if (dt.Rows.Count > 0)
                    {
                        string monthName = new DateTime(2010, mMonth, 1).ToString("MMM", CultureInfo.InvariantCulture); //get month name base on number
                        
                        if (!dt.Rows[0].IsNull(monthName))
                            amount = (decimal)dt.Rows[0][monthName];
                        else
                            MessageBox.Show("Depreciation journal with selected period does not exists.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }

            if (mDepreciationMethodId == (int)DepreciationMethodEnum.SYD)
            {
                if (mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    DataTable dt = ReportManager.DepreciationScheduleSYDFullMonthMonthly(criteria);
                    if (dt.Rows.Count > 0)
                    {
                        string monthName = new DateTime(2010, mMonth, 1).ToString("MMM", CultureInfo.InvariantCulture); //get month name base on number

                        if (!dt.Rows[0].IsNull(monthName))
                            amount = (decimal)(double)dt.Rows[0][monthName];
                        else
                            MessageBox.Show("Depreciation journal with selected period does not exists.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            DepreciationExpenseAmountlabel.Text = amount.ToString("N");
            AccumulatedDepreciationAmountlabel.Text = amount.ToString("N");


            //criteria.mPurchaseDate = fa.mPurchaseDate;
            //criteria.mPurchaseCost = fa.mPurchasePrice;
            //criteria.mResidualValue = fa.mResidualValue;
            //criteria.mUsefulLifeYears = fa.mUsefulLifeYears;
            //criteria.mYear = mYear;

            //DataTable dt = ReportManager.DepreciationStraightLineFullMonth(criteria);
            //if (dt.Rows.Count == 0) return;

            //DataRow row = dt.Rows[0];
            //DepreciationExpenseAmountlabel.Text = (((decimal)row["depreciation_expense"]) / 12).ToString("N");
            //AccumulatedDepreciationAmountlabel.Text = DepreciationExpenseAmountlabel.Text;
        }

        private bool IsDepreciationJournalExists()
        {
            DepreciationJournalCriteria criteria = new DepreciationJournalCriteria();
            criteria.mFixedAssetId = mId;
            criteria.mYear = mYear;
            criteria.mMonth = mMonth;

            return DepreciationJournalManager.SelectCountForGetList(criteria) > 0;
        }

        private void DepreciationJournalForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DepreciationJournalForm_Load(object sender, EventArgs e)
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
            decimal difference = 0;
            decimal.TryParse(Differencelabel.Text, out difference);
            if (difference != 0)
            {
                MessageBox.Show("Depreciation journal has difference cannot continue.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (IsDepreciationJournalExists())
            {
                MessageBox.Show("Depreciation journal with same month and year was already exists.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (SaveDepreciationJournal() > 0)
            {
                MessageBox.Show("Depreciation journal save successfully.", "Depreciation Journal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void DepreciationExpensecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            decimal debit = 0;
            decimal credit = 0;
            if (DepreciationExpensecheckBox.Checked)
                decimal.TryParse(DepreciationExpenseAmountlabel.Text, out debit);
            else
                decimal.TryParse(DepreciationExpenseAmountlabel.Text, out credit);

            decimal debit2 = 0;
            decimal credit2 = 0;
            if (AccumulatedDepreciationcheckBox.Checked)
                decimal.TryParse(AccumulatedDepreciationAmountlabel.Text, out debit2);
            else
                decimal.TryParse(AccumulatedDepreciationAmountlabel.Text, out credit2);

            Debitlabel.Text = (debit + debit2).ToString("N");
            Creditlabel.Text = (credit + credit2).ToString("N");

            decimal difference = (debit + debit2) - (credit + credit2);
            Differencelabel.Text = difference.ToString("N");
            Differencelabel.ForeColor = difference == 0 ? Color.Black : Color.Red;
        }
    }
}
