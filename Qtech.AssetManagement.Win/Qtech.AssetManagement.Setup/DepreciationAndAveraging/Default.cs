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

namespace Qtech.AssetManagement.Setup.DepreciationAndAveraging
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



        #region Private Methods
        private void LoadDepreciationMethod()
        {
            
        }

        private int SaveDepreciationMethod()
        {
            return 0;
        }
      

        private void EndEditing()
        {
            depreciationMethodUserControl1.EndEditing();
            averagingMethodUserControl1.EndEditing();
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


            if (ultraTabControl1.ActiveTab.Index == 0) depreciationMethodUserControl1.NewRecord();
            if (ultraTabControl1.ActiveTab.Index == 1) averagingMethodUserControl1.NewRecord();
        }

        public int SaveRecords()
        {
            if (ultraTabControl1.ActiveTab.Index == 0) depreciationMethodUserControl1.SaveRecords();
            if (ultraTabControl1.ActiveTab.Index == 1) averagingMethodUserControl1.SaveRecords();

            return 0;
        }

        public void CancelTransaction()
        {
            if (ultraTabControl1.ActiveTab.Index == 0) depreciationMethodUserControl1.CancelTransaction();
            if (ultraTabControl1.ActiveTab.Index == 1) averagingMethodUserControl1.CancelTransaction();
        }

        public void DeleteRecords()
        {
            if (ultraTabControl1.ActiveTab.Index == 0) depreciationMethodUserControl1.DeleteRecords();
            if (ultraTabControl1.ActiveTab.Index == 1) averagingMethodUserControl1.DeleteRecords();
        }

        public void PrintRecords()
        {
            if (ultraTabControl1.ActiveTab.Index == 0) depreciationMethodUserControl1.PrintRecords();
            if (ultraTabControl1.ActiveTab.Index == 1) averagingMethodUserControl1.PrintRecords();
        }

        #endregion
        

        private void Default_Load(object sender, EventArgs e)
        {
            SessionUtil.UserValidate(ref allow_select, ref allow_insert,
              ref allow_update, ref allow_delete, ref allow_print,
              (int)Modules.DepreciationMethod);

            depreciationMethodUserControl1.allow_select = allow_select;
            depreciationMethodUserControl1.allow_insert = allow_insert;
            depreciationMethodUserControl1.allow_update = allow_update;
            depreciationMethodUserControl1.allow_delete = allow_delete;
            depreciationMethodUserControl1.allow_print = allow_print;
            depreciationMethodUserControl1.LoadDepreciationMethod();

            averagingMethodUserControl1.allow_select = allow_select;
            averagingMethodUserControl1.allow_insert = allow_insert;
            averagingMethodUserControl1.allow_update = allow_update;
            averagingMethodUserControl1.allow_delete = allow_delete;
            averagingMethodUserControl1.allow_print = allow_print;
            averagingMethodUserControl1.LoadAveragingMethod();

            EndEditing();
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
           
        }
    }
}
