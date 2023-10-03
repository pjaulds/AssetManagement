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

        public int mId { get; set; }

        private void LoadFormControlsFromFixedAsset()
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);
            
            TypetextBox.Text = fa.mAssetTypeName;

            LoadFormControlsFromAssetType(fa);

            WarrantyExpirytextBox.Text = fa.mWarrantyExpiry.ToString("D");
            SerialNotextBox.Text = fa.mSerialNo;
            DescriptiontextBox.Text = fa.mDescription;
            DepreciationStartDatelabel.Text = fa.mDepreciationStartDate.ToString("D");
            DepreciationMethodlabel.Text = fa.mDepreciationMethodName;
            AveragingMethodlabel.Text = fa.mAveragingMethodName;
            PurchasePricelabel.Text = fa.mPurchasePrice.ToString("N");
            PurchaseDatelabel.Text = fa.mPurchaseDate.ToString("D");
            ResidualValuelabel.Text = fa.mResidualValue.ToString();
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();
            
            AssetAccounttextBox.Text = item.mAssetAccountCode + " - " + item.mAssetAccountName;
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
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            BusinessEntities.FixedAsset fa = FixedAssetManager.GetItem(mId);

            fa.mIsRegistered = true;
            fa.mUserId = SessionUtil.mUser.mId;

            FixedAssetManager.Save(fa);

            MessageBox.Show("Asset save as registered successfully.", "Fixed Asset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
