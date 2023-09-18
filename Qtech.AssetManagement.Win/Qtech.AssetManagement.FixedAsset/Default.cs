﻿using Infragistics.Win;
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

namespace Qtech.AssetManagement.FixedAsset
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

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.FixedAsset)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadFixedAsset()
        {
            ultraGrid1.SetDataBinding(FixedAssetManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveFixedAsset()
        {
            BusinessEntities.FixedAsset item = new BusinessEntities.FixedAsset();
            LoadFixedAssetFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = FixedAssetManager.Save(item);
                EndEditing();
                LoadFixedAsset();

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

        private void LoadFixedAssetFromFormControls(BusinessEntities.FixedAsset myFixedAsset)
        {
            myFixedAsset.mId = int.Parse(Idlabel.Text);
            myFixedAsset.mCode = CodetextBox.Text;
            myFixedAsset.mName = NametextBox.Text;
            myFixedAsset.mTypeId = ControlUtil.UltraComboReturnValue(TypeutraCombo);
            myFixedAsset.mTypeName = TypeutraCombo.Text;
            myFixedAsset.mFunctionalLocationId = ControlUtil.UltraComboReturnValue(FunctionalLocationultraCombo);
            myFixedAsset.mFunctionalLocationName = FunctionalLocationultraCombo.Text;
            myFixedAsset.mDescription = DescriptiontextBox.Text;
            myFixedAsset.mPurchaseDate = PurchasedateTimePicker.Value.Date;
            myFixedAsset.mPurchasePrice = ControlUtil.TextBoxDecimal(PurchasePricetextBox);
            myFixedAsset.mWarrantyExpiry = WarrantydateTimePicker.Value.Date;
            myFixedAsset.mSerialNo = SerialNotextBox.Text;
            myFixedAsset.mModel = ModeltextBox.Text;
            myFixedAsset.mDepreciationStartDate = DepreciationStartdateTimePicker.Value.Date;
            myFixedAsset.mDepreciationMethodId = ControlUtil.UltraComboReturnValue(DepreciationMethodultraCombo);
            myFixedAsset.mDepreciationMethodName = DepreciationMethodultraCombo.Text;
            myFixedAsset.mAveragingMethodId = ControlUtil.UltraComboReturnValue(AveragingMehodultraCombo);
            myFixedAsset.mAveragingMethodName = AveragingMehodultraCombo.Text;
            myFixedAsset.mResidualValue = ControlUtil.TextBoxDecimal(ResidualValuetextBox);
            myFixedAsset.mUsefulLifeYears = ControlUtil.TextBoxShort(UsefulLifetextBox);
            myFixedAsset.mIsDraft = DraftcheckBox.Checked;
            myFixedAsset.mIsRegistered = RegisteredcheckBox.Checked;
            myFixedAsset.mIsDisposed = DisposedcheckBox.Checked;
            myFixedAsset.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.FixedAsset myFixedAsset)
        {
            Idlabel.Text = myFixedAsset.mId.ToString();
            CodetextBox.Text = myFixedAsset.mCode;
            NametextBox.Text = myFixedAsset.mName;
            TypeutraCombo.Value = myFixedAsset.mTypeId;
            FunctionalLocationultraCombo.Value = myFixedAsset.mFunctionalLocationId;
            DescriptiontextBox.Text = myFixedAsset.mDescription;
            PurchasedateTimePicker.Value = myFixedAsset.mPurchaseDate;
            PurchasePricetextBox.Text = myFixedAsset.mPurchasePrice.ToString();
            WarrantydateTimePicker.Value = myFixedAsset.mWarrantyExpiry;
            SerialNotextBox.Text = myFixedAsset.mSerialNo;
            ModeltextBox.Text = myFixedAsset.mModel;
            DepreciationStartdateTimePicker.Value = myFixedAsset.mDepreciationStartDate;
            DepreciationMethodultraCombo.Value = myFixedAsset.mDepreciationMethodId;
            AveragingMehodultraCombo.Value = myFixedAsset.mAveragingMethodId;
            ResidualValuetextBox.Text = myFixedAsset.mResidualValue.ToString();
            UsefulLifetextBox.Text = myFixedAsset.mUsefulLifeYears.ToString();
            DraftcheckBox.Checked = myFixedAsset.mIsDraft;
            RegisteredcheckBox.Checked = myFixedAsset.mIsRegistered;
            DisposedcheckBox.Checked = myFixedAsset.mIsDisposed;
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

            NametextBox.Focus();
            DraftcheckBox.Checked = true;
        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            FixedAssetCriteria criteria = new FixedAssetCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (FixedAssetManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account title already exists."));

            criteria = new FixedAssetCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (FixedAssetManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Account code already exists."));

            decimal purchasePrice = 0;
            if (!decimal.TryParse(PurchasePricetextBox.Text, out purchasePrice))
                rules.Add(new BrokenRule("", "Invalid purchase price."));

            short usefulLife = 0;
            if (!short.TryParse(UsefulLifetextBox.Text, out usefulLife))
                rules.Add(new BrokenRule("", "Invalid useful life (years)."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveFixedAsset();
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
                BusinessEntities.FixedAsset item = FixedAssetManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                FixedAssetManager.Delete(item);

                LoadFixedAsset();

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

            BusinessEntities.FixedAsset item = FixedAssetManager.GetItem(_mId);
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
                (int)Modules.FixedAsset);

            EndEditing();

            ThemeUtil.Controls(splitContainer1.Panel2);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadFixedAsset();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveFixedAsset();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        public void RefreshAllSelection()
        {
            UltraComboUtil.AssetType(TypeutraCombo);
            UltraComboUtil.FunctionalLocation(FunctionalLocationultraCombo);
            UltraComboUtil.DepreciationMethod(DepreciationMethodultraCombo);
            UltraComboUtil.AveragingMethod(AveragingMehodultraCombo);

        }

        private void ultraGrid1_MouseDown(object sender, MouseEventArgs e)
        {
            UltraGrid ug = ((UltraGrid)sender);

            UIElement clicked_element = ug.DisplayLayout.UIElement.ElementFromPoint(ug.PointToClient(Default.MousePosition));
            if (clicked_element == null) return;

            CellUIElement cell_element = (CellUIElement)clicked_element.GetAncestor(typeof(CellUIElement));
            if (cell_element == null) return;

            UltraGridCell cell = (UltraGridCell)cell_element.GetContext(typeof(UltraGridCell));
            if (cell == null) return;

            if (cell.Column.Key == "mName")
            {
                BusinessEntities.FixedAsset item = (BusinessEntities.FixedAsset)cell.Row.ListObject;
                RegisterForm frm = new RegisterForm();
                frm.mId = item.mId;
                frm.ShowDialog();
            }
        }
    }
}