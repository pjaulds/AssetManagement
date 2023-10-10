using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
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
    public partial class DepreciationJournalForm : Form
    {
        public DepreciationJournalForm()
        {
            InitializeComponent();
        }

        public int mId { get; set; }
        public byte mMonth { get; set; }
        public short mYear { get; set; }

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
            LoadFormControlsFromFixedAsset();
        }
    }
}
