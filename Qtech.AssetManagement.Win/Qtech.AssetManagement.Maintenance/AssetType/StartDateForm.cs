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

namespace Qtech.AssetManagement.Maintenance.AssetType
{
    public partial class StartDateForm : Form
    {
        public StartDateForm()
        {
            InitializeComponent();
        }

        private void StartDateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void StartDateForm_Load(object sender, EventArgs e)
        {
            ThemeUtil.Controls(this);

            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) > 0)
                FixedAssetSettingdateTimePicker.Value = FixedAssetSettingDateManager.GetList().First().mDate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            FixedAssetSettingDate date;
            if (FixedAssetSettingDateManager.SelectCountForGetList(new FixedAssetSettingDateCriteria()) == 0) date = new FixedAssetSettingDate();
            else date = FixedAssetSettingDateManager.GetList().First();

            date.mDate = FixedAssetSettingdateTimePicker.Value.Date;
            date.mUserId = SessionUtil.mUser.mId;
            FixedAssetSettingDateManager.Save(date);

            MessageBox.Show("Start date save successfully.", "Start Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
