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
    public partial class DisposedForm : Form
    {
        public DisposedForm()
        {
            InitializeComponent();
        }

        private void LoadDisposal()
        {
            DisposalCriteria criteria = new DisposalCriteria();
            if (dateTimePicker1.Checked && dateTimePicker2.Checked)
            {
                criteria.mStartDate = dateTimePicker1.Value.Date;
                criteria.mEndDate = dateTimePicker2.Value.Date;
            }

            ultraGrid1.SetDataBinding(DisposalManager.GetList(criteria), null, true);
            ultraGrid1.Refresh();
        }

        private void DisposedForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);
            LoadDisposal();
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            LoadDisposal();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
