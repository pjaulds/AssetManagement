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

namespace Qtech.AssetManagement.User
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
                    return ((BusinessEntities.Users)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadUsers()
        {
            ultraGrid1.SetDataBinding(UsersManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveUsers()
        {
            BusinessEntities.Users item = new BusinessEntities.Users();
            LoadUsersFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = UsersManager.Save(item);
                EndEditing();
                LoadUsers();

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

        private void LoadUsersFromFormControls(BusinessEntities.Users myUser)
        {
            myUser.mId = int.Parse(Idlabel.Text);
            myUser.mUsername = UsernametextBox.Text;
            myUser.mPassword = PasswordtextBox.Text;
            myUser.mPersonnelId = ControlUtil.UltraComboReturnValue(PersonnelultraCombo);

            //Create a salt  
            var salt = SaltHashManager.Get_SALT();

            //Create a hash  
            var hash = SaltHashManager.Get_HASH_SHA512(PasswordtextBox.Text, UsernametextBox.Text, salt);
            myUser.mHash = hash;
            myUser.mSalt = salt;

            myUser.mUserId = SessionUtil.mUser.mId;

            myUser.mUserAccessCollection = (UserAccessCollection)AccessultraGrid.DataSource;
        }

        private void LoadFormControlsFromUser(BusinessEntities.Users myUsers)
        {
            Idlabel.Text = myUsers.mId.ToString();
            UsernametextBox.Text = myUsers.mUsername;
            PersonnelultraCombo.Value = myUsers.mPersonnelId;

            LoadFormControlsFromUserAccess(myUsers);
        }

        private void LoadFormControlsFromUserAccess(BusinessEntities.Users myUsers)
        {
            UserAccessCriteria criteria = new UserAccessCriteria();
            criteria.mUserId = myUsers == null ? 0 : myUsers.mId;

            AccessultraGrid.SetDataBinding(UserAccessManager.GetList(criteria), null, true);
            AccessultraGrid.Refresh();
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
            LoadFormControlsFromUserAccess(null);
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

            UsernametextBox.Focus();

        }

        public int SaveRecords()
        {
            if (!allow_update && int.Parse(Idlabel.Text) > 0)//editing
            {
                MessageUtil.NotAllowedUpdateAccess();
                return 0;
            }

            BrokenRulesCollection rules = new BrokenRulesCollection();

            if (PasswordtextBox.Text != ConfirmtextBox.Text)
                rules.Add(new BrokenRule("", "Password do not match."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveUsers();
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
                BusinessEntities.Users item = UsersManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                UsersManager.Delete(item);

                LoadUsers();

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

            BusinessEntities.Users item = UsersManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            UsernametextBox.Focus();
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
                (int)Modules.User);

            EndEditing();

            ThemeUtil.ControlsWithoutUpperCase(splitContainer1.Panel2);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadUsers();
            UltraComboUtil.Personnel(PersonnelultraCombo);
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        private void ultraGrid2_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);

            e.Layout.Override.CellClickAction = CellClickAction.EditAndSelectText;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
        }

        private void AllcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool check = AllcheckBox.Checked;

            foreach (UltraGridRow row in AccessultraGrid.Rows)
            {
                row.Cells["mSelect"].Value = check;
                row.Cells["mInsert"].Value = check;
                row.Cells["mUpdate"].Value = check;
                row.Cells["mDelete"].Value = check;
                row.Cells["mPrint"].Value = check;
            }
        }
    }
}
