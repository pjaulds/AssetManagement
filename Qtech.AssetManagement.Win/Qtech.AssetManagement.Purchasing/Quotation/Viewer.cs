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

namespace Qtech.AssetManagement.Purchasing.Quotation
{
    public partial class Viewer : Form
    {
        public Viewer()
        {
            InitializeComponent();
        }


        public int mId { get; set; }
        ReportParameter[] myReportParameter = new ReportParameter[9];
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
            QuotationDetailCriteria criteria = new QuotationDetailCriteria();
            criteria.mQuotationId = mId;

            ReportDataSource rds = new ReportDataSource("QuotationFields", QuotationDetailManager.GetList(criteria));
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            BusinessEntities.Quotation q = QuotationManager.GetItem(mId);
            CompanyProfile cp = CompanyProfileManager.GetList().First();
            CreateParameter(0, "ReportLogo", ImageToBase64(Image.FromStream(new System.IO.MemoryStream(cp.mReportLogo)), System.Drawing.Imaging.ImageFormat.Jpeg));
            CreateParameter(1, "CompanyName", cp.mName);
            CreateParameter(2, "CompanyAddress", cp.mAddress);
            
            CreateParameter(3, "Supplier1", q.mSupplier1Name);
            CreateParameter(4, "Supplier2", q.mSupplier2Name);
            CreateParameter(5, "Supplier3", q.mSupplier3Name);
            CreateParameter(6, "TransactionNo", q.mTransactionNo);
            CreateParameter(7, "PurchaseRequestNo", q.mPurchaseRequestNo);
            CreateParameter(8, "Date", q.mDate.ToString());

           

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
