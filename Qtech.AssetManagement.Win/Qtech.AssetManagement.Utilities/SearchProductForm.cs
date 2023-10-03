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

namespace Qtech.AssetManagement.Utilities
{
    public partial class SearchProductForm : Form
    {
        public SearchProductForm()
        {
            InitializeComponent();
        }

        private void SearchProductForm_KeyDown(object sender, KeyEventArgs e)
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

        private void SearchProductForm_Load(object sender, EventArgs e)
        {
            ultraGrid1.SetDataBinding(ProductManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        public Product mProduct { get; set; }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row == null) return;
            if (e.Row.Index == -1) return;

            mProduct = (Product)e.Row.ListObject;
            Close();
        }
    }
}
