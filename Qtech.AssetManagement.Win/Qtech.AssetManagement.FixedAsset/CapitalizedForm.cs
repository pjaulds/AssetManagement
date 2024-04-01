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
    public partial class CapitalizedForm : Form
    {
        public CapitalizedForm()
        {
            InitializeComponent();
        }

        private FixedAssetCapitalizedCostCollection deleted_items = null;

        public int mId { get; set; }

        private void SaveFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);
            
            fa.mUserId = SessionUtil.mUser.mId;
            LoadFixedAssetCapitalizedCostFromFormControls(fa);
            fa.mDeletedFixedAssetCapitalizedCostCollection = deleted_items;

            FixedAssetManager.Save(fa);

            MessageBox.Show("Caplitelization suggested journal entry has been successfully processed.", "Fixed Asset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void LoadFixedAssetCapitalizedCostFromFormControls(BusinessEntities.FixedAsset myFixedAsset)
        {
            FixedAssetCapitalizedCostCollection items = new FixedAssetCapitalizedCostCollection();
            foreach(DataGridViewRow row in ItemsdataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                items.Add((FixedAssetCapitalizedCost)row.DataBoundItem);
            }
            myFixedAsset.mFixedAssetCapitalizedCostCollection = items;
        }

        private void LoadFormControlsFromFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);

            NametextBox.Text = fa.mProductName;
            AssetNotextBox.Text = fa.mAssetNo;
            TypetextBox.Text = fa.mAssetTypeName;
            FunctionalLocationtextBox.Text = fa.mFunctionalLocationName;

            LoadFormControlsFromAssetType(fa);
            
            WarrantyExpirytextBox.Text = fa.mWarrantyExpiry.ToString("D");
            SerialNotextBox.Text = fa.mSerialNo;
            ModeltextBox.Text = fa.mModel;
            PurchaseDatetextBox.Text = fa.mPurchaseDate.ToString("D");
            PurchaseCosttextBox.Text = fa.mPurchasePrice.ToString("N");
            ResidualValuetextBox.Text = fa.mResidualValue.ToString("N");
            DepreciationMethodtextBox.Text = fa.mDepreciationMethodName;
            AveragingMethodtextBox.Text = fa.mAveragingMethodName;

          

        }

        private void LoadFormControlsFromFixedAssetCapitalizedCost()
        {
            FixedAssetCapitalizedCostCriteria criteria = new FixedAssetCapitalizedCostCriteria();
            criteria.mFixedAssetId = mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<FixedAssetCapitalizedCost>(FixedAssetCapitalizedCostManager.GetList(criteria));
            ItemsdataGridView.Refresh();

            TotalCapitalizedCost();
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();
            
            AssetAccounttextBox.Text = item.mChartOfAccountCode + " - " + item.mChartOfAccountName;
            AccumulatedDepreciationAccounttextBox.Text = item.mAccumulatedDepreciationAccountCode + " - " + item.mAccumulatedDepreciationAccountName;
            DepreciationExpenseAccounttextBox.Text = item.mDepreciationExpenseAccountCode + " - " + item.mDepreciationExpenseAccountName;            
        }

        private void LoadFormControlsFromBookValue()
        {
            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mId;
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);

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
                BookValuelabel.Text = Convert.ToDecimal(dt.Rows[0]["BookValueEnd"]).ToString("N");
        }
        
        private void TotalCapitalizedCost()
        {
            SortableBindingList<FixedAssetCapitalizedCost> costs = (SortableBindingList<FixedAssetCapitalizedCost>)ItemsdataGridView.DataSource;
            TotalCostlabel.Text = costs.Sum(x => x.mAmount).ToString("N");
            TotalUsefullabel.Text = costs.Sum(x => x.mUsefulLife * 12).ToString("N");
            //CapitalizedCosttextBox.Text = TotalCostlabel.Text;

            //decimal purchaseCost = Convert.ToDecimal(PurchaseCosttextBox.Text);
            //decimal accDep = Convert.ToDecimal(AccumulatedDepreciationtextBox.Text);
            //decimal capCost = Convert.ToDecimal(CapitalizedCosttextBox.Text);

            //TotalAmounttextBox.Text = (purchaseCost - accDep + capCost).ToString("N");

            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);
            DateTime date1 = fa.mPurchaseDate.AddYears((int)fa.mUsefulLifeYears);
            date1 = date1.AddMonths((int)costs.Sum(x => x.mUsefulLife * 12));
            DateTime date2 = AuditManager.GetDateToday();

            double months = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
            Monthslabel.Text = months.ToString("N0");
            Yearslabel.Text = (months / 12).ToString("N");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RegisterForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            LoadFormControlsFromFixedAsset();
            LoadFormControlsFromFixedAssetCapitalizedCost();
            LoadFormControlsFromBookValue();

            DataGridViewComboBoxColumn comboColumn = (DataGridViewComboBoxColumn)(ItemsdataGridView.Columns["mCapitalizedCost"]);
            comboColumn.DataSource = CapitalizedCostManager.GetList();
            comboColumn.DisplayMember = "mName";
            comboColumn.ValueMember = "mId";

            BookValueAsOflabel.Text = "As of " + AuditManager.GetDateToday().ToString("MMM dd, yyyy");
            AsOfLabel2label.Text = BookValueAsOflabel.Text;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveFixedAsset();
        }

        private void Depreciatebutton_Click(object sender, EventArgs e)
        {
            RunDepreciationForm dep = new RunDepreciationForm();
            dep.mFaId = mId;
            dep.FormClosing += Dep_FormClosing;
            dep.ShowDialog();
        }

        private void Dep_FormClosing(object sender, FormClosingEventArgs e)
        {
            RunDepreciationForm dep = (RunDepreciationForm)sender;

            //if (dep.mMonth == 0) return;//no selected month
        }

        private void ItemsdataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void ItemsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            if (e.RowIndex == ItemsdataGridView.Rows.Count - 1) return;//new row

            string colName = ItemsdataGridView.Columns[e.ColumnIndex].Name;
            FixedAssetCapitalizedCost item = (FixedAssetCapitalizedCost)ItemsdataGridView.CurrentRow.DataBoundItem;

            if (colName == "mDelete")
            {               
                if (deleted_items == null)
                    deleted_items = new  FixedAssetCapitalizedCostCollection();

                deleted_items.Add(item);

                ItemsdataGridView.Rows.Remove(ItemsdataGridView.CurrentRow);
            }
            else if(colName == "mUsefulLife")
            {
                ExtendUsefulLifeForm ext = new ExtendUsefulLifeForm();
                ext.FormClosing += Ext_FormClosing;
                ext.ShowDialog();
            }
            else if(colName == "mIsJournalized")
            {
                CapitalizeJournalizedForm journal = new CapitalizeJournalizedForm();
                journal.mAssetAccountId = item.mAssetAccountId;
                journal.mCashPayableAccountId = item.mCashPayableAccountId;
                journal.FormClosing += Journal_FormClosing;
                journal.ShowDialog();
            }
        }

        private void Journal_FormClosing(object sender, FormClosingEventArgs e)
        {
            CapitalizeJournalizedForm journal = (CapitalizeJournalizedForm)sender;
            if (!journal.mAcceptInput) return;
            FixedAssetCapitalizedCost item = (FixedAssetCapitalizedCost)ItemsdataGridView.CurrentRow.DataBoundItem;
            item.mAssetAccountId = journal.mAssetAccountId;
            item.mCashPayableAccountId = journal.mCashPayableAccountId;
        }

        private void Ext_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExtendUsefulLifeForm ext = (ExtendUsefulLifeForm)sender;
            if (!ext.mAcceptInput) return;

            ItemsdataGridView.CurrentCell.Value = ext.mYears;
            TotalCapitalizedCost();
        }

        private void Disposebutton_Click(object sender, EventArgs e)
        {
            DisposalForm dispose = new DisposalForm();
            dispose.mFixedAssetId = mId;
            dispose.mFixedAssetName = NametextBox.Text;
            dispose.ShowDialog();
        }

        private void ItemsdataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2) TotalCapitalizedCost();
        }
    }
}
