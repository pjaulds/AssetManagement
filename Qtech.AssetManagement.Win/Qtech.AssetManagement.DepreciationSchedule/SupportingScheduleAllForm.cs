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

namespace Qtech.AssetManagement.DepreciationSchedule
{
    public partial class SupportingScheduleAllForm : Form
    {
        public SupportingScheduleAllForm()
        {
            InitializeComponent();
        }

        private void SupportingScheduleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void SupportingScheduleForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            UltraComboUtil.AssetType(AssetTypeutraCombo);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMethodultraCombo);
            
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            if (ControlUtil.IsUltraComboEmpty(AssetTypeutraCombo))
            {
                MessageBox.Show("Please select asset type", "Depreciation Schedule", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (ControlUtil.IsUltraComboEmpty(DepreciationMethodultraCombo))
            {
                MessageBox.Show("Please select depreciation method", "Depreciation Schedule", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (ControlUtil.IsUltraComboEmpty(AveragingMethodultraCombo))
            {
                MessageBox.Show("Please select averaging method", "Depreciation Schedule", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int depMethodId = ControlUtil.UltraComboReturnValue(DepreciationMethodultraCombo);
            int avgMethodId = ControlUtil.UltraComboReturnValue(AveragingMethodultraCombo);
            int assetTypeId  = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);

            if (depMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if(avgMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineFullMonthMonthly.Viewer viewer = new StraightLineFullMonthMonthly.Viewer();
                        viewer.mAssetTypeId = assetTypeId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineFullMonthAnnually.Viewer viewer = new StraightLineFullMonthAnnually.Viewer();
                        viewer.mAssetTypeId = assetTypeId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

                if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineActualDaysMonthly.Viewer viewer = new StraightLineActualDaysMonthly.Viewer();
                        viewer.mAssetTypeId = assetTypeId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineActualDaysAnnually.Viewer viewer = new StraightLineActualDaysAnnually.Viewer();
                        viewer.mAssetTypeId = assetTypeId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
