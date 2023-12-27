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

namespace Qtech.AssetManagement.User
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// override transactions for approval or checked by
        /// </summary>
        public bool mForOverride { get; set; }
        public BusinessEntities.Users mUser { get; set; }
        private void LogInForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void UserNametextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter))
                SendKeys.Send("{TAB}");            
        }

        private void PasswordtextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == Convert.ToInt32(Keys.Enter))
                LogInButton_Click(sender, e);
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            UsersCriteria criteria = new UsersCriteria();
            criteria.mUsername = UserNametextBox.Text;

            if (UsersManager.SelectCountForGetList(criteria) > 0)
            {
                Users user = UsersManager.GetList(criteria).First();
                string OldHASHValue = string.Empty;
                byte[] SALT = new byte[512];

                OldHASHValue = user.mHash;
                SALT = user.mSalt;

                if (SaltHashManager.CompareHashValue(PasswordtextBox.Text, UserNametextBox.Text, OldHASHValue, SALT))
                {
                    if (!mForOverride)
                        SessionUtil.mUser = user;

                    mUser = user;
                    Close();
                }
                else
                {
                    MessageBox.Show("User name and password is invalid", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("User name and password is invalid", "Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void LogInForm_Load(object sender, EventArgs e)
        {
            if (Environment.MachineName == "PHDS")
            {
                UserNametextBox.Text = "PAUL";
                PasswordtextBox.Text = "GECSERVICES";
                //LogInButton_Click(sender, e);
            }
        }
    }
}
