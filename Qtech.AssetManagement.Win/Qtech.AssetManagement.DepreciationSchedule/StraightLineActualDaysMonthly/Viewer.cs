using Microsoft.Reporting.WinForms;
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

namespace Qtech.AssetManagement.DepreciationSchedule.StraightLineActualDaysMonthly
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fixed Asset Id
        /// </summary>
        public int mId { get; set; }

        public int mAssetTypeId { get; set; }

        public short mYear { get; set; }

        ReportParameter[] myReportParameter = new ReportParameter[4];
        private void CreateParameter(int index, string name, string value)
        {
            myReportParameter[index] = new ReportParameter();
            myReportParameter[index].Name = name;
            myReportParameter[index].Values.Add(value);
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mId;
            criteria.mAssetTypeId = mAssetTypeId;
            criteria.mYear = mYear;
            
            ReportDataSource rds = new ReportDataSource("Fields", ReportManager.DepreciationScheduleStraightLineActualDaysMonthly(criteria));
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            CreateParameter(0, "Year", mYear.ToString());
            CreateParameter(1, "Year2", mYear.ToString().Substring(2, 2));

            FixedAsset fa = FixedAssetManager.GetItem(mId);
            CreateParameter(2, "AssetType", fa == null ? AssetTypeManager.GetItem(mAssetTypeId).mName : fa.mAssetTypeName);

            try
            {
                if (mAssetTypeId > 0)
                    CreateParameter(3, "AccountCode", "");
                else
                {
                    FixedAssetSetting faType = FixedAssetSettingManager.GetList().Where(x => x.mAssetTypeId == fa.mAssetTypeId).First();
                    CreateParameter(3, "AccountCode", faType.mAssetAccountCode);
                }
            }
            catch { CreateParameter(3, "AccountCode", ""); }

            

            reportViewer1.LocalReport.SetParameters(myReportParameter);
            reportViewer1.SetDisplayMode(DisplayMode.Normal);
            reportViewer1.RefreshReport();
        }

        private void Viewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }
    }
}
