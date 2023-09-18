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
    public partial class QuantityForm : Form
    {
        public QuantityForm()
        {
            InitializeComponent();
        }

        public decimal mQuantity { get; set; }
        public bool mAccept { get; set; }

        private void QuantityForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mAccept = false;
                Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ControlUtil.TextBoxNumericDecimal_KeyPress(sender, e);

            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                mQuantity = ControlUtil.TextBoxDecimal(textBox1);

                mAccept = true;
                Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
