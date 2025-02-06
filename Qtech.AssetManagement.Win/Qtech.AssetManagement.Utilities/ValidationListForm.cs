using Qtech.AssetManagement.Validation;
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
    public partial class ValidationListForm : Form
    {
        public ValidationListForm()
        {
            InitializeComponent();
        }

        public BrokenRulesCollection mBrokenRules { get; set; }
        private void ValidationListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ValidationListForm_Load(object sender, EventArgs e)
        {
            label1.DataBindings.Add("Text", mBrokenRules, "Message");
            dataRepeater1.DataSource = mBrokenRules;
        }
    }
}
