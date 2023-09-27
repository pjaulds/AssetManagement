using Microsoft.Reporting.WinForms;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Purchasing.PurchaseRequest
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
        }


        public int mId { get; set; }
        ReportParameter[] myReportParameter = new ReportParameter[12];
        private void CreateParameter(int index, string name, string value)
        {
            myReportParameter[index] = new ReportParameter();
            myReportParameter[index].Name = name;
            myReportParameter[index].Values.Add(value);
        }

        private void Viewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            PurchaseRequestDetailCriteria criteria = new PurchaseRequestDetailCriteria();
            criteria.mPurchaseRequestId = mId;

            ReportDataSource rds = new ReportDataSource("PurchaseRequestFields", PurchaseRequestDetailManager.GetList(criteria));
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            BusinessEntities.PurchaseRequest pr = PurchaseRequestManager.GetItem(mId);
            CreateParameter(0, "RequestedBy", pr.mRequestedByName);
            CreateParameter(1, "Department", "");
            CreateParameter(2, "Supplier1", pr.mSupplier1Name);
            CreateParameter(3, "Supplier2", pr.mSupplier2Name);
            CreateParameter(4, "Supplier3", pr.mSupplier3Name);
            CreateParameter(5, "DateRequired", pr.mDateRequired.ToString());
            CreateParameter(6, "Remarks", pr.mRemarks);
            CreateParameter(7, "TransactionNo", pr.mTransactionNo);
            CreateParameter(8, "Date", pr.mDate.ToString());

            CompanyProfile cp = CompanyProfileManager.GetList().First();
            CreateParameter(9, "ReportLogo", ImageToBase64(Image.FromStream(new System.IO.MemoryStream(cp.mReportLogo)), System.Drawing.Imaging.ImageFormat.Jpeg));
            CreateParameter(10, "CompanyName", cp.mName);
            CreateParameter(11, "CompanyAddress", cp.mAddress);

            reportViewer1.LocalReport.SetParameters(myReportParameter);
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer1.RefreshReport();
        }

        public string ImageToBase64(Image image,
 System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}
