using System.ComponentModel;
using System;
namespace Qtech.AssetManagement.BusinessEntities
{
    public class Audit : BusinessBase
    {
        [DataObjectFieldAttribute(true, true, false)]
        public override int mId { get; set; }
       
        public DateTime mDate { get; set; }
        public new int mUserId { get; set; }

        public short mTableId { get; set; }
        public int mRowId { get; set; }
        public byte mActionId { get; set; }
        public string mField { get; set; }
        public string mOldValue { get; set; }
        public string mNewValue { get; set; }
    }
}
