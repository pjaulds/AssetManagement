using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Utilities
{
    public static class UltraComboUtil
    {

        public static void AssetType(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AssetTypeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ChartOfAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(ChartOfAccountManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }


        public static void AssetAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AssetAccountManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AccumulatedDepreciationAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AccumulatedDepreciationAccountManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void DepreciationExpenseAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(DepreciationExpenseAccountManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void DepreciationMethod(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(DepreciationMethodManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AveragingMethod(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AveragingMethodManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void FunctionalLocation(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(FunctionalLocationManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }
        
        public static void Personnel(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(PersonnelManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void Supplier(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(SupplierManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void Unit(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(UnitManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void PaymentMode(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(PaymentModeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void MaintenanceRequestType(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(MaintenanceRequestTypeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ServiceLevel(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(ServiceLevelManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void FaultSymptoms(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(FaultSymptomsManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void FaultArea(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(FaultAreaManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }
    }
}
