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

namespace Qtech.AssetManagement.Maintenance.AccountGroup
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
                    return ((BusinessEntities.AccountGroup)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadAccountGroup()
        {
            ultraGrid1.SetDataBinding(AccountGroupManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveAccountGroup()
        {
            BusinessEntities.AccountGroup item = new BusinessEntities.AccountGroup();
            LoadAccountGroupFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = AccountGroupManager.Save(item);
                EndEditing();
                LoadAccountGroup();

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

        private void LoadAccountGroupFromFormControls(BusinessEntities.AccountGroup myAccountGroup)
        {
            myAccountGroup.mId = int.Parse(Idlabel.Text);
            myAccountGroup.mCode = CodetextBox.Text;
            myAccountGroup.mName = NametextBox.Text;
            myAccountGroup.mPost = PostcheckBox.Checked;
            myAccountGroup.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.AccountGroup myAccountGroup)
        {
            Idlabel.Text = myAccountGroup.mId.ToString();
            CodetextBox.Text = myAccountGroup.mCode;
            NametextBox.Text = myAccountGroup.mName;
            PostcheckBox.Checked = myAccountGroup.mPost;
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
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

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            AccountGroupCriteria criteria = new AccountGroupCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (AccountGroupManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account group title already exists."));

            criteria = new AccountGroupCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (AccountGroupManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account group code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveAccountGroup();
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
                BusinessEntities.AccountGroup item = AccountGroupManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                AccountGroupManager.Delete(item);

                LoadAccountGroup();

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

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess();
                return;
            }

            BusinessEntities.AccountGroup item = AccountGroupManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            CodetextBox.Focus();
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
                (int)Modules.AccountGroup);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadAccountGroup();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveAccountGroup();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

    }
}
