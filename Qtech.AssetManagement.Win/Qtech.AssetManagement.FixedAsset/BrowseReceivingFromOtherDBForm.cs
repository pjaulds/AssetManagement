using Qtech.AssetManagement.Bll;
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
    public partial class BrowseReceivingFromOtherDBForm : Form
    {
        public BrowseReceivingFromOtherDBForm()
        {
            InitializeComponent();
        }

        public DataRowView mRv { get; set; }
        private void BrowseReceivingFromOtherDBForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void BrowseReceivingFromOtherDBForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);

            ultraGrid1.SetDataBinding(ReportManager.BrowseReceivingFromOtherDB(new BusinessEntities.ReportCriteria()), null, true);
            ultraGrid1.Refresh();
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1) return;

            DataRowView rv = (DataRowView)e.Row.ListObject;

            BrowseReceivingDetailFromOtherDBForm detailForm = new BrowseReceivingDetailFromOtherDBForm();
            detailForm.mReceivingId = Convert.ToInt32(rv["ReceivingId"]);
            detailForm.FormClosing += DetailForm_FormClosing;
            detailForm.ShowDialog();
        }

        private void DetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            BrowseReceivingDetailFromOtherDBForm detailForm = (BrowseReceivingDetailFromOtherDBForm)sender;
            if (detailForm.mRv == null) return;

            mRv = detailForm.mRv;
            Close();
        }
    }
}
