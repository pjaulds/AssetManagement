using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Controls
{
    public partial class ExpandPanelControl : UserControl
    {
        public ExpandPanelControl()
        {
            InitializeComponent();
        }

        public event EventHandler _ExpandPanel;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (_ExpandPanel != null)
                _ExpandPanel(this, new EventArgs());
        }
    }
}
