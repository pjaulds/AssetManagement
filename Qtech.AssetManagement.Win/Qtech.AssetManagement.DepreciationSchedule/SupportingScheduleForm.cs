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
    public partial class SupportingScheduleForm : Form
    {
        public SupportingScheduleForm()
        {
            InitializeComponent();
        }

        public int mFaId { get; set; }

        public void LoadFormControlsFromFixedAsset()
        {
            FixedAsset fa = FixedAssetManager.GetItem(mFaId);
            AssetTypetextBox.Text = fa.mAssetTypeName;
            DepreciationMethodultraCombo.Value = fa.mDepreciationMethodId;
            AveragingMethodultraCombo.Value = fa.mAveragingMethodId;
            LoadFormControlsFromAssetType(fa);
        }

        private void LoadFormControlsFromAssetType(BusinessEntities.FixedAsset fa)
        {
            //try
            //{
            //    FixedAssetSetting item = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();
            //    DepreciationMethodultraCombo.Value = item.mDepreciationMethodId;
            //    AveragingMethodultraCombo.Value = item.mAveragingMethodId;
            //}
            //catch { }
        }

        private void SupportingScheduleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void SupportingScheduleForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMethodultraCombo);

            LoadFormControlsFromFixedAsset();
            EndMonthdateTimePicker.Value = new DateTime(Convert.ToInt32(YearnumericUpDown.Value), EndMonthdateTimePicker.Value.Month, EndMonthdateTimePicker.Value.Day);
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
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

            if(depMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if(avgMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineFullMonthMonthly.Viewer viewer = new StraightLineFullMonthMonthly.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.mEndDateCriteria = EndMonthdateTimePicker.Value.Date;
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineFullMonthAnnually.Viewer viewer = new StraightLineFullMonthAnnually.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

                if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineActualDaysMonthly.Viewer viewer = new StraightLineActualDaysMonthly.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineActualDaysAnnually.Viewer viewer = new StraightLineActualDaysAnnually.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

            }

            if (depMethodId == (int)DepreciationMethodEnum.SYD)
            {
                if (avgMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        SYDFullMonthMonthly.Viewer viewer = new SYDFullMonthMonthly.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.mEndDateCriteria = EndMonthdateTimePicker.Value.Date;
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        SYDFullMonthAnnually.Viewer viewer = new SYDFullMonthAnnually.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

                if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        SYDActualDaysMonthly.Viewer viewer = new SYDActualDaysMonthly.Viewer();
                        viewer.mId = mFaId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else //annual
                    {
                        SYDActualDaysAnnually.Viewer viewer = new SYDActualDaysAnnually.Viewer();
                        viewer.mId = mFaId;
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

        private void YearnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            EndMonthdateTimePicker.Value = new DateTime(Convert.ToInt32(YearnumericUpDown.Value), EndMonthdateTimePicker.Value.Month, EndMonthdateTimePicker.Value.Day);
        }
    }
}
