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

namespace Qtech.AssetManagement.DepreciationSchedule.StraightLineFullMonthAnnually
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
        }

        public int mId { get; set; }
        public short mYear { get; set; }

        ReportParameter[] myReportParameter = new ReportParameter[1];
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
            criteria.mYear = mYear;
            ReportDataSource rds = new ReportDataSource("Fields", ReportManager.DepreciationScheduleStraightLineFullMonthAnnually(criteria));
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            CreateParameter(0, "Year", mYear.ToString());
            

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
