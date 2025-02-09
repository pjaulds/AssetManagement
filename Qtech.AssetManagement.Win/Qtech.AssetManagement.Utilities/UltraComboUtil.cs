﻿using Infragistics.Win.UltraWinGrid;
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

            myUltraCombo.SetDataBinding(AssetTypeManager.GetList().Where(x => x.mPost).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ChartOfAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(ChartOfAccountManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ChartOfAccountFixedAsset(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            ChartOfAccountCriteria criteria = new ChartOfAccountCriteria();
            criteria.mForFixedAssetAccount = true;

            myUltraCombo.SetDataBinding(ChartOfAccountManager.GetList(criteria).Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ChartOfAccountAccumulatedDepreciation(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            ChartOfAccountCriteria criteria = new ChartOfAccountCriteria();
            criteria.mForAccumulatedDepreciationAccount = true;

            myUltraCombo.SetDataBinding(ChartOfAccountManager.GetList(criteria).Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void ChartOfAccountDepreciationExpenseAccount(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            ChartOfAccountCriteria criteria = new ChartOfAccountCriteria();
            criteria.mForDepreciationExpenseAccount = true;

            myUltraCombo.SetDataBinding(ChartOfAccountManager.GetList(criteria).Where(x => x.mActive).ToList(), null, true);
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

            myUltraCombo.SetDataBinding(DepreciationMethodManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AveragingMethod(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AveragingMethodManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void FunctionalLocation(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(FunctionalLocationManager.GetList().Where(x => x.mActive).ToList(), null, true);
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

            myUltraCombo.SetDataBinding(SupplierManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void Unit(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(UnitManager.GetList().Where(x => x.mActive).ToList(), null, true);
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

        public static void WorkOrderType(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(WorkOrderTypeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void MaintenanceJobType(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(MaintenanceJobTypeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void MaintenanceJobTypeVariant(UltraCombo myUltraCombo, MaintenanceJobTypeVariantCriteria myMaintenanceJobTypeVariantCriteria)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(MaintenanceJobTypeVariantManager.GetList(myMaintenanceJobTypeVariantCriteria), null, true);
            myUltraCombo.Refresh();
        }

        public static void Trade(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(TradeManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }


        public static void AssetClass(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AssetClassManager.GetList().Where(x => x.mPost).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AccountType(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AccountTypeManager.GetList().Where(x => x.mPost).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AccountGroup(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AccountGroupManager.GetList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void AccountClassification(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(AccountClassificationManager.GetList().Where(x => x.mPost).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void Currency(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(CurrencyManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }

        public static void Project(UltraCombo myUltraCombo)
        {
            myUltraCombo.ValueMember = "mId";
            myUltraCombo.DisplayMember = "mName";

            myUltraCombo.SetDataBinding(ProjectManager.GetList().Where(x => x.mActive).ToList(), null, true);
            myUltraCombo.Refresh();
        }
    }
}
