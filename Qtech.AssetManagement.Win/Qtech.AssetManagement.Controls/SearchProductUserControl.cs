using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.Utilities;
using System.Text.RegularExpressions;

namespace Qtech.AssetManagement.Controls
{
    public partial class SearchProductUserControl : UserControl
    {
        public SearchProductUserControl()
        {
            InitializeComponent();
        }

        public Product mProduct { get; set; }
        public event EventHandler _GetProduct;
        public event EventHandler _SearchingProduct;
        public bool mWithQuantity { get; set; }
        public decimal mQuantity { get; set; }
        public UltraGrid mUltraGrid { get { return ultraGrid1; } }
        
        ProductCollection products;
        private void LoadProducts()
        {
            ProductCriteria criteria = new ProductCriteria();
            if (products == null) products = ProductManager.GetList(criteria);

            string pattern = @"\S+";
            Regex re = new Regex(pattern);
            System.Text.RegularExpressions.MatchCollection matches = re.Matches(ultraTextEditor1.Text.ToUpper());
            string[] words = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                words[i] = matches[i].Value;
            }

            if (ultraTextEditor1.Text != string.Empty)
            {
                ultraGrid1.SetDataBinding(
                    products.Where(x => x.mName.ToUpper().ContainsAny(words)).ToList(),
                    null,
                    true);
                ultraGrid1.Refresh();
            }
            else
            {
                ultraGrid1.SetDataBinding(
                    products,
                    null,
                    true);
                ultraGrid1.Refresh();
            }
            if (ultraGrid1.Rows.Count > 0)
            {
                UltraGridRow row = ultraGrid1.Rows[0];
                ultraGrid1.ActiveRow = row;
                ultraGrid1.Visible = true;
            }

            if (ultraTextEditor1.Text == string.Empty)
                ultraGrid1.Visible = false;

            if (_SearchingProduct != null)
                _SearchingProduct(this, new EventArgs());
        }

        private void GetProduct()
        {
            if (mWithQuantity)
            {
                QuantityForm qtyForm = new QuantityForm();
                qtyForm.FormClosing += QtyForm_FormClosing;
                qtyForm.ShowDialog();
                GetProduct2();
            }
            else GetProduct2();
        }

        private void GetProduct2()
        {
            mProduct = (Product)ultraGrid1.ActiveRow.ListObject;

            if (_GetProduct != null)
                _GetProduct(this, new EventArgs());

            ultraGrid1.Visible = false;
            ultraTextEditor1.Text = string.Empty;

            ultraTextEditor1.Focus();
        }

        private void QtyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            QuantityForm qtyForm = (QuantityForm)sender;
            if (!qtyForm.mAccept) return;
            mQuantity = qtyForm.mQuantity;
        }

        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);            
            e.Layout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Appearance.FontData.SizeInPoints = float.Parse("12");
        }

        private void ultraTextEditor1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                ultraGrid1.Focus();

            if (e.KeyCode == Keys.F2)
            {
                ultraGrid1.Visible = true;
                
                products = ProductManager.GetList();
                ultraGrid1.SetDataBinding(products, null, true);
                ultraGrid1.Refresh();
            }

            if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.Escape)
            {
                if (e.KeyCode == Keys.Escape)
                    ultraGrid1.Visible = false;

                if (_SearchingProduct != null)
                    _SearchingProduct(this, new EventArgs());
            }
        }

        private void ultraGrid1_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1)
                return;

            GetProduct();
        }

        private void ultraGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ultraGrid1.ActiveRow.Index == -1)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }

        private void ultraTextEditor1_ValueChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void ultraTextEditor1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                if (ultraGrid1.Rows.Count > 0)
                {
                    GetProduct();
                }
            }
        }
    }
}
