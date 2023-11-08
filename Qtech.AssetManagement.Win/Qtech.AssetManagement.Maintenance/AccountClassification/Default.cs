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

namespace Qtech.AssetManagement.Maintenance.AccountClassification
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
                    return ((BusinessEntities.AccountClassification)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadAccountClassification()
        {
            ultraGrid1.SetDataBinding(AccountClassificationManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveAccountClassification()
        {
            BusinessEntities.AccountClassification item = new BusinessEntities.AccountClassification();
            LoadAccountClassificationFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = AccountClassificationManager.Save(item);
                EndEditing();
                LoadAccountClassification();

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

        private void LoadAccountClassificationFromFormControls(BusinessEntities.AccountClassification myAccountClassification)
        {
            myAccountClassification.mId = int.Parse(Idlabel.Text);
            myAccountClassification.mCode = CodetextBox.Text;
            myAccountClassification.mName = NametextBox.Text;
            myAccountClassification.mPost = PostcheckBox.Checked;
            myAccountClassification.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.AccountClassification myAccountClassification)
        {
            Idlabel.Text = myAccountClassification.mId.ToString();
            CodetextBox.Text = myAccountClassification.mCode;
            NametextBox.Text = myAccountClassification.mName;
            PostcheckBox.Checked = myAccountClassification.mPost;
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

            AccountClassificationCriteria criteria = new AccountClassificationCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (AccountClassificationManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account classification title already exists."));

            criteria = new AccountClassificationCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (AccountClassificationManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account classification code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveAccountClassification();
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
                BusinessEntities.AccountClassification item = AccountClassificationManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                AccountClassificationManager.Delete(item);

                LoadAccountClassification();

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

            BusinessEntities.AccountClassification item = AccountClassificationManager.GetItem(_mId);
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
                (int)Modules.AccountClassification);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadAccountClassification();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveAccountClassification();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

    }
}
