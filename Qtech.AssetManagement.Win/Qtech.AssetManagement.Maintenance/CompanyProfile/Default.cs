using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
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

namespace Qtech.AssetManagement.Maintenance.CompanyProfile
{
    public partial class Default : Form, ICRUD
    {
        public Default()
        {
            InitializeComponent();
        }

        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.CompanyProfile)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadCompanyProfile()
        {
            ultraGrid1.SetDataBinding(CompanyProfileManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveCompanyProfile()
        {
            BusinessEntities.CompanyProfile item = new BusinessEntities.CompanyProfile();
            LoadCompanyProfileFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = CompanyProfileManager.Save(item);
                EndEditing();
                LoadCompanyProfile();

                return id;

            }
            else
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = item.BrokenRules;
                validationForm.ShowDialog();
                return 0;
            }
        }

        private void LoadCompanyProfileFromFormControls(BusinessEntities.CompanyProfile myUser)
        {
            myUser.mId = int.Parse(Idlabel.Text);
            myUser.mName = NametextBox.Text;
            myUser.mAddress = AddresstextBox.Text;
            myUser.mReportLogo = FileUtil.ImageToByte(pictureBox1.Image);

            myUser.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.CompanyProfile myCompanyProfile)
        {
            Idlabel.Text = myCompanyProfile.mId.ToString();
            NametextBox.Text = myCompanyProfile.mName;
            AddresstextBox.Text = myCompanyProfile.mAddress;

            if (myCompanyProfile.mReportLogo != null)
                pictureBox1.Image = Image.FromStream(new System.IO.MemoryStream(myCompanyProfile.mReportLogo));
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            pictureBox1.Image = null;
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess();
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            NametextBox.Focus();

        }

        public int SaveRecords()
        {
            if (int.Parse(Idlabel.Text) != 0)
            {
                if (!allow_update)
                {
                    MessageUtil.NotAllowedUpdateAccess();
                    return 0;
                }
            }

            return SaveCompanyProfile();
        }

        public void CancelTransaction()
        {
            if (MessageUtil.AskCancelEdit())
                EndEditing();
        }

        public void DeleteRecords()
        {
            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess();
                return;
            }

            if (MessageUtil.AskDelete())
            {
                BusinessEntities.CompanyProfile item = CompanyProfileManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                CompanyProfileManager.Delete(item);

                LoadCompanyProfile();

            }
        }

        public void PrintRecords()
        {
            if (!allow_print)
            {
                MessageUtil.NotAllowedPrintAccess();
                return;
            }
        }

        #endregion

        #region Ultragrid 
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1)
                return;

            BusinessEntities.CompanyProfile item = CompanyProfileManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            NametextBox.Focus();
        }

        private void ultraGrid1_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (!allow_delete)
                MessageUtil.NotAllowedDeleteAccess();
            else
                DeleteRecords();

            e.Cancel = true;
        }
        #endregion

        private void expandPanelControl1__ExpandPanel(object sender, EventArgs e)
        {
            if (Idlabel.Text == "0" && allow_insert)
                ControlUtil.ExpandPanel(splitContainer1);
            else if (Idlabel.Text != "0" && allow_update)
                ControlUtil.ExpandPanel(splitContainer1);
        }

        private void collapsePanelControl1__HidePanel(object sender, EventArgs e)
        {
            ControlUtil.HidePanel(splitContainer1);
        }

        private void Default_Load(object sender, EventArgs e)
        {
            SessionUtil.UserValidate(ref allow_select, ref allow_insert,
                ref allow_update, ref allow_delete, ref allow_print,
                (int)Modules.CompanyProfile);

            EndEditing();

            ThemeUtil.Controls(this);
            //ControlUtil.TextBoxEnterLeaveNoKeyDownEventHandler(splitContainer1.Panel2);
            LoadCompanyProfile();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveCompanyProfile();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileUtil.BrowseImage(pictureBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }
    }
}
