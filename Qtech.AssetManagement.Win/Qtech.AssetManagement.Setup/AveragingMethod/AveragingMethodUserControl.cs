using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.Utilities;
using Qtech.AssetManagement.Validation;
using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Setup.AveragingMethod
{
    public partial class AveragingMethodUserControl : UserControl
    {
        public AveragingMethodUserControl()
        {
            InitializeComponent();
        }

        #region Private Variables
        public bool allow_select;
        public bool allow_insert;
        public bool allow_update;
        public bool allow_delete;
        public bool allow_print;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.AveragingMethod)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        public void LoadAveragingMethod()
        {
            ultraGrid1.SetDataBinding(AveragingMethodManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        public int SaveAveragingMethod()
        {
            BusinessEntities.AveragingMethod item = new BusinessEntities.AveragingMethod();
            LoadAveragingMethodFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = AveragingMethodManager.Save(item);
                EndEditing();
                LoadAveragingMethod();

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

        private void LoadAveragingMethodFromFormControls(BusinessEntities.AveragingMethod myAveragingMethod)
        {
            myAveragingMethod.mId = int.Parse(Idlabel.Text);
            myAveragingMethod.mCode = CodetextBox.Text;
            myAveragingMethod.mName = NametextBox.Text;
            myAveragingMethod.mActive = ActivecheckBox.Checked;
            myAveragingMethod.mRemarks = RemarkstextBox.Text;
            myAveragingMethod.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsAveragingMethod(BusinessEntities.AveragingMethod myAveragingMethod)
        {
            Idlabel.Text = myAveragingMethod.mId.ToString();
            CodetextBox.Text = myAveragingMethod.mCode;
            NametextBox.Text = myAveragingMethod.mName;
            ActivecheckBox.Checked = myAveragingMethod.mActive;
            RemarkstextBox.Text = myAveragingMethod.mRemarks;
        }

        public void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            ThemeUtil.Controls(this);
            Idlabel.Text = "0";
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess(" averaging method");
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            AveragingMethodCriteria criteria = new AveragingMethodCriteria();

            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (AveragingMethodManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mCode + " already exists. Please use a different, unique code.");
                return 0;
            }

            criteria = new AveragingMethodCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (AveragingMethodManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mName + " already exists. Please use a different, unique name.");
                return 0;
            }

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }

            if (!MessageUtil.SaveConfirm("averaging method")) return 0;

            bool isNew = Idlabel.Text == "0";

            int id = SaveAveragingMethod();

            if (id > 0)
            {
                BusinessEntities.AveragingMethod item = AveragingMethodManager.GetItem(id);

                if (isNew)
                    MessageUtil.SaveSuccessfully(item.mCode);
                else
                    MessageUtil.UpdatedSuccessfully(item.mCode);
            }

            return id;
        }

        public void CancelTransaction()
        {
            if (MessageUtil.CancelUpdateConfirm())
                EndEditing();
        }

        public void DeleteRecords()
        {
            BusinessEntities.AveragingMethod item = AveragingMethodManager.GetItem(_mId);

            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess(" averaging method " + item.mCode);
                return;
            }
            
            if (MessageUtil.DeleteConfirm(item.mCode))
            {
                item.mUserId = SessionUtil.mUser.mId;
                AveragingMethodManager.Delete(item);
                MessageUtil.DeletedSuccessfully(item.mCode);
                LoadAveragingMethod();
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

            BusinessEntities.AveragingMethod item = AveragingMethodManager.GetItem(_mId);

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess(" averaging method " + item.mCode);
                return;
            }
            
            LoadFormControlsAveragingMethod(item);

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

        public void AveragingLoad()
        {
            SessionUtil.UserValidate(ref allow_select, ref allow_insert,
              ref allow_update, ref allow_delete, ref allow_print,
              (int)Modules.AveragingMethod);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadAveragingMethod();
        }
       
        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
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

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }
    }
}
