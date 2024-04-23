using Microsoft.Reporting.WinForms;
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

namespace Qtech.AssetManagement.DepreciationSchedule
{
    public partial class FixedAssetReportViewer : Form
    {
        public FixedAssetReportViewer()
        {
            InitializeComponent();
        }

        public string mMonth { get; set; }
        public string mYear { get; set; }
        public FixedAssetCollection mFixedAssetCollection { get; set; }

        ReportParameter[] myReportParameter = new ReportParameter[2];
        private void CreateParameter(int index, string name, string value)
        {
            myReportParameter[index] = new ReportParameter();
            myReportParameter[index].Name = name;
            myReportParameter[index].Values.Add(value);
        }

        private void FixedAssetReportViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void FixedAssetReportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource rds = new ReportDataSource("FieldsFixedAssetReport", mFixedAssetCollection);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            CreateParameter(0, "Month", mMonth);
            CreateParameter(1, "Year", mYear);         

            reportViewer1.LocalReport.SetParameters(myReportParameter);
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.RefreshReport();
        }
    }
}
