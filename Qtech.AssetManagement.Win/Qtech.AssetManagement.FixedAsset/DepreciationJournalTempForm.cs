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
    public partial class DepreciationJournalTempForm : Form
    {
        public DepreciationJournalTempForm()
        {
            InitializeComponent();
        }

        public byte mMonth { get; set; }
        public short mYear { get; set; }

        private void DepreciationJournalTempForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void DepreciationJournalTempForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            FixedAssetCriteria criteria = new FixedAssetCriteria();
            criteria.mIsRegistered = true;
            foreach(BusinessEntities.FixedAsset fa in FixedAssetManager.GetList(criteria))
            {
                //check if depreciable
                FixedAssetSettingCriteria settingCriteria = new FixedAssetSettingCriteria();
                settingCriteria.mAssetTypeId = fa.mAssetTypeId;
                if (FixedAssetSettingManager.SelectCountForGetList(settingCriteria) == 0) continue;
                if (!FixedAssetSettingManager.GetList(settingCriteria).First().mDepreciable) continue;

                DepreciationUserControl dep = new DepreciationUserControl();
                dep.mMonth = (int)mMonth;
                dep.mYear = mYear;
                dep.LoadFormControlsFromFixedAsset(fa.mId);
                dep.Dock = DockStyle.Top;                
                panel2.Controls.Add(dep);
            }

            foreach(Control ctrl in panel2.Controls)
            {
                ctrl.BringToFront();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            foreach (Control ctrl in panel2.Controls)
            {
                if(ctrl is DepreciationUserControl)
                {
                    DepreciationUserControl dep = (DepreciationUserControl)ctrl;
                    dep.Save();

                    cnt += 1;
                }                
            }

            MessageBox.Show("Total of " + cnt.ToString() + " fixed asset(s) processed.", "Depreciation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
