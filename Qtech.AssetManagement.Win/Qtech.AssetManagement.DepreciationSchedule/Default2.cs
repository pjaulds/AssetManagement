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
    public partial class Default2 : Form
    {
        public Default2()
        {
            InitializeComponent();
        }

        private void LoadDates()
        {
            if (MonthcomboBox.SelectedIndex == -1) return;

            DateTime startDate = new DateTime((int)YearnumericUpDown.Value, MonthcomboBox.SelectedIndex + 1, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            StartdateTimePicker.Value = startDate;
            EnddateTimePicker.Value = endDate;
        }

        private void Default2_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            UltraComboUtil.AssetType(AssetTypeutraCombo);
            YearnumericUpDown.Value = AuditManager.GetDateToday().Year;
            LoadDates();
        }

        private void MonthcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDates();
        }

        private void YearnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            LoadDates();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            FixedAssetCriteria faCriteria = new FixedAssetCriteria();
            faCriteria.mAssetTypeId = ControlUtil.UltraComboReturnValue(AssetTypeutraCombo);

            FixedAssetCollection items = new FixedAssetCollection();
            foreach (FixedAsset fa in FixedAssetManager.GetList(faCriteria))
            {
                AssetType type = AssetTypeManager.GetItem(fa.mAssetTypeId);
                FixedAssetSetting setting = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == type.mId).First();

                ReportCriteria criteria = new ReportCriteria();
                criteria.mAssetTypeId = type.mId;
                criteria.mId = fa.mId;
                criteria.mYear = (short)YearnumericUpDown.Value;
                criteria.mEndDate = EnddateTimePicker.Value;

                if (fa.mPurchaseDate > EnddateTimePicker.Value.Date) continue;

                DataTable dt = new DataTable();
                int depMethodId = setting.mDepreciationMethodId;
                int avgMethodId = setting.mAveragingMethodId;

                if (depMethodId == (int)DepreciationMethodEnum.StraightLine)
                {
                    if (avgMethodId == (int)AveragingMethodEnum.FullMonth)
                    {
                        if (MonthlyradioButton.Checked)
                            dt = ReportManager.DepreciationScheduleStraightLineFullMonthMonthly(criteria);
                        else//annual
                            dt = ReportManager.DepreciationScheduleStraightLineFullMonthAnnually(criteria);

                    }

                    if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                    {
                        if (MonthlyradioButton.Checked)
                            dt = ReportManager.DepreciationScheduleStraightLineActualDaysMonthly(criteria);
                        else//annual
                            dt = ReportManager.DepreciationScheduleStraightLineActualDaysAnnually(criteria);
                    }

                }

                if (depMethodId == (int)DepreciationMethodEnum.SYD)
                {
                    if (avgMethodId == (int)AveragingMethodEnum.FullMonth)
                    {
                        if (MonthlyradioButton.Checked)
                            dt = ReportManager.DepreciationScheduleSYDFullMonthMonthly(criteria);
                        else//annual
                            dt = ReportManager.DepreciationScheduleSYDFullMonthAnnually(criteria);
                    }

                    if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                    {
                        if (MonthlyradioButton.Checked)
                            dt = ReportManager.DepreciationScheduleSYDActualDaysAnnually(criteria);
                        else //annual
                            dt = ReportManager.DepreciationScheduleSYDActualDaysAnnually(criteria);
                    }
                }

                if (dt == null) continue;
                if (dt.Rows.Count == 0) continue;

                fa.mAccumulatedDepreciation = Convert.ToDecimal(dt.Rows[0]["Ending"].ToString());
                fa.mBookValue = Convert.ToDecimal(dt.Rows[0]["BookValueEnd"].ToString());
                fa.mMonthlyDepreciation = Convert.ToDecimal(dt.Rows[0]["MonthlyDepreciation"].ToString());
                items.Add(fa);
            }

            ultraGrid1.SetDataBinding(items, null, true);
            ultraGrid1.Refresh();

        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1) return;

            BusinessEntities.FixedAsset fa = (BusinessEntities.FixedAsset)e.Row.ListObject;
            int depMethodId = fa.mDepreciationMethodId;
            int avgMethodId = fa.mAveragingMethodId;

            if (depMethodId == (int)DepreciationMethodEnum.StraightLine)
            {
                if (avgMethodId == (int)AveragingMethodEnum.FullMonth)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineFullMonthMonthly.Viewer viewer = new StraightLineFullMonthMonthly.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.mEndDateCriteria = EnddateTimePicker.Value.Date;
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineFullMonthAnnually.Viewer viewer = new StraightLineFullMonthAnnually.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

                if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        StraightLineActualDaysMonthly.Viewer viewer = new StraightLineActualDaysMonthly.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        StraightLineActualDaysAnnually.Viewer viewer = new StraightLineActualDaysAnnually.Viewer();
                        viewer.mId = fa.mId;
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
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.mEndDateCriteria = EnddateTimePicker.Value.Date;
                        viewer.ShowDialog();
                    }
                    else//annual
                    {
                        SYDFullMonthAnnually.Viewer viewer = new SYDFullMonthAnnually.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }

                }

                if (avgMethodId == (int)AveragingMethodEnum.ActualDays)
                {
                    if (MonthlyradioButton.Checked)
                    {
                        SYDActualDaysMonthly.Viewer viewer = new SYDActualDaysMonthly.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                    else //annual
                    {
                        SYDActualDaysAnnually.Viewer viewer = new SYDActualDaysAnnually.Viewer();
                        viewer.mId = fa.mId;
                        viewer.mYear = Convert.ToInt16(YearnumericUpDown.Value);
                        viewer.ShowDialog();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FixedAssetReportViewer viewer = new FixedAssetReportViewer();
            viewer.mMonth = MonthlyradioButton.Text;
            viewer.mYear = YearnumericUpDown.Value.ToString();
            viewer.mFixedAssetCollection = (FixedAssetCollection)ultraGrid1.DataSource;
            viewer.ShowDialog();
        }
    }
}
