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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private FixedAssetCapitalizedCostCollection deleted_items = null;

        public int mId { get; set; }

        private void SaveFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);

            fa.mIsDraft = false;
            fa.mIsRegistered = true;
            fa.mUserId = SessionUtil.mUser.mId;
            LoadFixedAssetCapitalizedCostFromFormControls(fa);
            fa.mDeletedFixedAssetCapitalizedCostCollection = deleted_items;

            FixedAssetManager.Save(fa);

            MessageBox.Show("Asset save as registered successfully.", "Fixed Asset", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            UsefulLifetextBox.Text = fa.mUsefulLifeYears.ToString();
      //some comments
        }

        private void LoadFormControlsFromFixedAssetCapitalizedCost()
        {
            FixedAssetCapitalizedCostCriteria criteria = new FixedAssetCapitalizedCostCriteria();
            criteria.mFixedAssetId = mId;

            ItemsdataGridView.AutoGenerateColumns = false;
            ItemsdataGridView.DataSource = new SortableBindingList<FixedAssetCapitalizedCost>(FixedAssetCapitalizedCostManager.GetList(criteria));
            ItemsdataGridView.Refresh();
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();
            
            AssetAccounttextBox.Text = item.mChartOfAccountCode + " - " + item.mChartOfAccountName;
            AccumulatedDepreciationAccounttextBox.Text = item.mAccumulatedDepreciationAccountCode + " - " + item.mAccumulatedDepreciationAccountName;
            DepreciationExpenseAccounttextBox.Text = item.mDepreciationExpenseAccountCode + " - " + item.mDepreciationExpenseAccountName;
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

            DataGridViewComboBoxColumn comboColumn = (DataGridViewComboBoxColumn)(ItemsdataGridView.Columns["mCapitalizedCost"]);
            comboColumn.DataSource = CapitalizedCostManager.GetList();
            comboColumn.DisplayMember = "mName";
            comboColumn.ValueMember = "mId";
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
        }
    }
}
