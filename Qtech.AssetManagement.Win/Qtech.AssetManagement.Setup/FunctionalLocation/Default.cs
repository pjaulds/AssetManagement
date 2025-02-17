﻿using Infragistics.Win.UltraWinGrid;
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

namespace Qtech.AssetManagement.Setup.FunctionalLocation
{
    public partial class Default : Form, ICRUD, IComboSelection
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
                    return ((BusinessEntities.FunctionalLocation)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadFunctionalLocation()
        {
            ultraGrid1.SetDataBinding(FunctionalLocationManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveFunctionalLocation()
        {
            BusinessEntities.FunctionalLocation item = new BusinessEntities.FunctionalLocation();
            LoadFunctionalLocationFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = FunctionalLocationManager.Save(item);
                EndEditing();
                LoadFunctionalLocation();

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

        private void LoadFunctionalLocationFromFormControls(BusinessEntities.FunctionalLocation myFunctionalLocation)
        {
            myFunctionalLocation.mId = int.Parse(Idlabel.Text);
            myFunctionalLocation.mCode = CodetextBox.Text;
            myFunctionalLocation.mName = NametextBox.Text;
            myFunctionalLocation.mParentFlId = ControlUtil.UltraComboReturnValue(FunctionalLocationultraCombo);
            myFunctionalLocation.mParentFlName = FunctionalLocationultraCombo.Text;
            myFunctionalLocation.mFlStatus = StatuscomboBox.Text;
            myFunctionalLocation.mAddressName = AddressNametextBox.Text;
            myFunctionalLocation.mStreet = StreettextBox.Text;
            myFunctionalLocation.mCity = CitytextBox.Text;
            myFunctionalLocation.mProvince = ProvincetextBox.Text;
            myFunctionalLocation.mCountry = CountrytextBox.Text;
            myFunctionalLocation.mZipCode = ZipCodetextBox.Text;
            myFunctionalLocation.mActive = ActivecheckBox.Checked;
            myFunctionalLocation.mRemarks = RemarkstextBox.Text;
            myFunctionalLocation.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.FunctionalLocation myFunctionalLocation)
        {
            Idlabel.Text = myFunctionalLocation.mId.ToString();
            CodetextBox.Text = myFunctionalLocation.mCode;
            NametextBox.Text = myFunctionalLocation.mName;
            FunctionalLocationultraCombo.Value = myFunctionalLocation.mParentFlId;
            StatuscomboBox.Text = myFunctionalLocation.mFlStatus;
            AddressNametextBox.Text = myFunctionalLocation.mAddressName;
            StreettextBox.Text = myFunctionalLocation.mStreet;
            CitytextBox.Text = myFunctionalLocation.mCity;
            ProvincetextBox.Text = myFunctionalLocation.mProvince;
            CountrytextBox.Text = myFunctionalLocation.mCountry;
            ZipCodetextBox.Text = myFunctionalLocation.mZipCode;
            RemarkstextBox.Text = myFunctionalLocation.mRemarks;
            ActivecheckBox.Checked = myFunctionalLocation.mActive;
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
                MessageUtil.NotAllowedInsertAccess(" functional location");
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            FunctionalLocationCriteria criteria = new FunctionalLocationCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (FunctionalLocationManager.SelectCountForGetList(criteria) > 0)
            {
                MessageUtil.Message(criteria.mCode + " already exists. Please use a different, unique code.");
                return 0;
            }
            
            criteria = new FunctionalLocationCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (FunctionalLocationManager.SelectCountForGetList(criteria) > 0)
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

            if (!MessageUtil.SaveConfirm(" functional location")) return 0;

            bool isNew = Idlabel.Text == "0";

            int id = SaveFunctionalLocation();

            if (id > 0)
            {
                BusinessEntities.FunctionalLocation item = FunctionalLocationManager.GetItem(id);

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
            BusinessEntities.FunctionalLocation item = FunctionalLocationManager.GetItem(_mId);

            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess(" functional location " + item.mCode);
                return;
            }
            
            if (MessageUtil.DeleteConfirm(item.mCode))
            {   
                item.mUserId = SessionUtil.mUser.mId;
                FunctionalLocationManager.Delete(item);
                MessageUtil.DeletedSuccessfully(item.mCode);
                LoadFunctionalLocation();
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
                MessageUtil.NotAllowedUpdateAccess(" functional location");
                return;
            }

            BusinessEntities.FunctionalLocation item = FunctionalLocationManager.GetItem(_mId);
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
                (int)Modules.FunctionalLocation);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadFunctionalLocation();
            RefreshAllSelection();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveRecords();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.FunctionalLocation(FunctionalLocationultraCombo);
        }

        private void FunctionalLocationultraCombo_RowSelected(object sender, RowSelectedEventArgs e)
        {
            ParentNametextBox.Text = string.Empty;
            if (e.Row == null) return;

            BusinessEntities.FunctionalLocation fl = (BusinessEntities.FunctionalLocation)e.Row.ListObject;
            ParentNametextBox.Text = fl.mName;
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
