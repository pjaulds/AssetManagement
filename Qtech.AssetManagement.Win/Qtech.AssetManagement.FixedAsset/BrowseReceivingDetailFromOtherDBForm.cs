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
    public partial class BrowseReceivingDetailFromOtherDBForm : Form
    {
        public BrowseReceivingDetailFromOtherDBForm()
        {
            InitializeComponent();
        }

        public int mReceivingId { get; set; }
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
            ReportCriteria criteria = new ReportCriteria();
            criteria.mId = mReceivingId;            

            ultraGrid1.SetDataBinding(ReportManager.BrowseReceivingDetailFromOtherDB(criteria), null, true);
            ultraGrid1.Refresh();

            ThemeUtil.Controls(this);
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1) return;

            mRv = (DataRowView)e.Row.ListObject;
            Close();
        }
    }
}
