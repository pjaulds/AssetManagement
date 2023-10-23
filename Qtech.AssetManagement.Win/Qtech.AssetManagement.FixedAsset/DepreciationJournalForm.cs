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
            EndDepreciationDatetextBox.Text = fa.mPurchaseDate.AddYears(fa.mUsefulLifeYears).AddDays(-1).ToString("D");
            PurchaseCosttextBox.Text = fa.mPurchasePrice.ToString("N");
            ResidualValuetextBox.Text = fa.mResidualValue.ToString("N");
            UsefulLifetextBox.Text = fa.mUsefulLifeYears.ToString();
            DepreciationMethodtextBox.Text = fa.mDepreciationMethodName;
            AveragingMethodtextBox.Text = fa.mAveragingMethodName;

            LoadFormControlsFromAssetType(fa);
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();

            DepreciationExpenseAccountCodelabel.Text = item.mAssetAccountCode; 
            DepreciationExpenseAccountTitlelabel.Text = item.mAssetAccountName;

            AccumulatedDepreciationAccountCodelabel.Text = item.mAccumulatedDepreciationAccountCode;
            AccumulatedDepreciationAccountTitlelabel.Text = item.mAccumulatedDepreciationAccountName;

            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mId;
            criteria.mYear = mYear;
            decimal amount = 0;
            if (fa.mDepreciationMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if(fa.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
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

            if (fa.mDepreciationMethodId == (int)DepreciationMethodEnum.SYD)
            {
                if (fa.mAveragingMethodId == (int)AveragingMethodEnum.FullMonth)
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
            if(IsDepreciationJournalExists())
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
    }
}
