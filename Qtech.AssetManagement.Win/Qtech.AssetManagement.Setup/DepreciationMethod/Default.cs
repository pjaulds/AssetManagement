using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using Qtech.AssetManagement.Validation;
using Qtech.Qasa.PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Setup.DepreciationMethod
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

        public IPluginHost PluginHost { get; set; }
        public IPlugin Plugin { get; set; }


        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.DepreciationMethod)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadDepreciationMethod()
        {
            ultraGrid1.SetDataBinding(DepreciationMethodManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveDepreciationMethod()
        {
            BusinessEntities.DepreciationMethod item = new BusinessEntities.DepreciationMethod();
            LoadDepreciationMethodFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = DepreciationMethodManager.Save(item);
                EndEditing();
                LoadDepreciationMethod();

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

        private void LoadDepreciationMethodFromFormControls(BusinessEntities.DepreciationMethod myDepreciationMethod)
        {
            myDepreciationMethod.mId = int.Parse(Idlabel.Text);
            myDepreciationMethod.mCode = CodetextBox.Text;
            myDepreciationMethod.mName = NametextBox.Text;
            myDepreciationMethod.mActive = ActivecheckBox.Checked;
            myDepreciationMethod.mRemarks = RemarkstextBox.Text;
            myDepreciationMethod.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.DepreciationMethod myDepreciationMethod)
        {
            Idlabel.Text = myDepreciationMethod.mId.ToString();
            CodetextBox.Text = myDepreciationMethod.mCode;
            NametextBox.Text = myDepreciationMethod.mName;
            ActivecheckBox.Checked = myDepreciationMethod.mActive;
            RemarkstextBox.Text = myDepreciationMethod.mRemarks;
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
                MessageUtil.NotAllowedInsertAccess("Depreciation Method");
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            DepreciationMethodCriteria criteria = new DepreciationMethodCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (DepreciationMethodManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mName + " already exists. Please use a different, unique name.");
                return 0;
            }

            criteria = new DepreciationMethodCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (DepreciationMethodManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mCode + " already exists. Please use a different, unique code.");
                return 0;
            }

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }

            if (!MessageUtil.SaveConfirm("depreciation method")) return 0;

            return SaveDepreciationMethod();
        }

        public void CancelTransaction()
        {
            if (MessageUtil.CancelUpdateConfirm())
                EndEditing();
        }

        public void DeleteRecords()
        {
            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess("depreciation method");
                return;
            }

            BusinessEntities.DepreciationMethod item = DepreciationMethodManager.GetItem(_mId);
            if (MessageUtil.DeleteConfirm(item.mCode))
            {
                item.mUserId = SessionUtil.mUser.mId;
                DepreciationMethodManager.Delete(item);
                LoadDepreciationMethod();
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
                MessageUtil.NotAllowedUpdateAccess(" depreciation method");
                return;
            }

            BusinessEntities.DepreciationMethod item = DepreciationMethodManager.GetItem(_mId);
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
                (int)Modules.DepreciationMethod);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadDepreciationMethod();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SaveRecords() > 0) NewRecord();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = Idlabel.Text;
            if (MessageUtil.ResetConfirm()) ControlUtil.ClearConent(splitContainer1.Panel2);

            Idlabel.Text = id;//reassigned
        }
    }
}
