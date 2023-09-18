using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Dal;
using System;
namespace Qtech.AssetManagement.Bll
{
    public class AuditManager
    {
        public static string GetNewNumber(string tableName)
        {
            return AuditDB.GetNewNumber(tableName);
        }

        public static DateTime GetDateToday()
        {
            return AuditDB.GetDateToday();
        }

        public static void AuditInsert(bool isSubItem, string userName, short tableId, int rowId, string description)
        {
            BusinessEntities.Audit audit =
                new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = tableId;
            audit.mRowId = rowId;
            audit.mActionId = 1;

            Save(audit);
        }

        public static void AuditUpdate(bool isSubItem, string userName, short tableId, int rowId, string field, string oldValue, string newValue, string description)
        {
            BusinessEntities.Audit audit =
                 new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = tableId;
            audit.mRowId = rowId;
            audit.mField = field;
            audit.mOldValue = oldValue;
            audit.mNewValue = newValue;

            Save(audit);
        }

        public static void AuditDelete(bool isSubItem, string userName, short tableId, int rowId, string description)
        {
            BusinessEntities.Audit audit =
                new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = tableId;
            audit.mRowId = rowId;
            audit.mActionId = 3;

            Save(audit);
        }

        public static void AuditPrint(string userName, short tableId, int rowId, string description)
        {
            BusinessEntities.Audit audit =
                new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = tableId;
            audit.mRowId = rowId;
            audit.mActionId = 4;

            Save(audit);
        }

        public static void AuditOthers(string userName, string description)
        {
            BusinessEntities.Audit audit =
                new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = 0;
            audit.mRowId = 0;
            audit.mActionId = 5;

            Save(audit);
        }

        public static void AuditOthers(string userName, int rowId, string description)
        {
            BusinessEntities.Audit audit =
                new BusinessEntities.Audit();

            audit.mUserFullName = userName;
            audit.mTableId = 0;
            audit.mRowId = rowId;
            audit.mActionId = 5;

            Save(audit);
        }
        
        public static void Save(BusinessEntities.Audit audit)
        {
            AuditDB.Save(audit);
        }

        public static AuditCollection GetList(AuditCriteria auditCriteria)
        {
            return AuditDB.GetList(auditCriteria);
        }

        public static void BackUpDatabase(string script, string path)
        {
            AuditDB.BackUpDatabase(script, path);
        }
    }
}
