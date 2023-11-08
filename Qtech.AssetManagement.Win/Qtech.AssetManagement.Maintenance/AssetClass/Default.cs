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

namespace Qtech.AssetManagement.Maintenance.AssetClass
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
                    return ((BusinessEntities.AssetClass)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadAssetClass()
        {
            ultraGrid1.SetDataBinding(AssetClassManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveAssetClass()
        {
            BusinessEntities.AssetClass item = new BusinessEntities.AssetClass();
            LoadAssetClassFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = AssetClassManager.Save(item);
                EndEditing();
                LoadAssetClass();

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

        private void LoadAssetClassFromFormControls(BusinessEntities.AssetClass myAssetClass)
        {
            myAssetClass.mId = int.Parse(Idlabel.Text);
            myAssetClass.mCode = CodetextBox.Text;
            myAssetClass.mName = NametextBox.Text;
            myAssetClass.mPost = PostcheckBox.Checked;
            myAssetClass.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.AssetClass myAssetClass)
        {
            Idlabel.Text = myAssetClass.mId.ToString();
            CodetextBox.Text = myAssetClass.mCode;
            NametextBox.Text = myAssetClass.mName;
            PostcheckBox.Checked = myAssetClass.mPost;
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

            AssetClassCriteria criteria = new AssetClassCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (AssetClassManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Asset class title already exists."));

            criteria = new AssetClassCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (AssetClassManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Asset class code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveAssetClass();
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
                BusinessEntities.AssetClass item = AssetClassManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                AssetClassManager.Delete(item);

                LoadAssetClass();

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

            BusinessEntities.AssetClass item = AssetClassManager.GetItem(_mId);
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
                (int)Modules.AssetClass);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadAssetClass();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveAssetClass();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

    }
}
