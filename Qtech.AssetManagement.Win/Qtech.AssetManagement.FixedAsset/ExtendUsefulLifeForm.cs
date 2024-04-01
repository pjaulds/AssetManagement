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
    public partial class ExtendUsefulLifeForm : Form
    {
        public ExtendUsefulLifeForm()
        {
            InitializeComponent();
        }

        public bool mAcceptInput { get; set; }
        
        public decimal mYears {
            get { return ControlUtil.TextBoxDecimal(YeartextBox); }
        }

        private void ExtendUsefulLifeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void ExtendUsefulLifeForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MonthstextBox_TextChanged(object sender, EventArgs e)
        {
            decimal months = ControlUtil.TextBoxDecimal(MonthstextBox);
            if (months == 0) YeartextBox.Text = string.Empty;
            else YeartextBox.Text = (months / 12).ToString("N2");
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            mAcceptInput = true;
            Close();
        }
    }
}
