using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class JournalVoucher : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public DateTime mDate { get; set; }
        public Int32 mNumber { get; set; }

        [NotEqualTo(Message = "Please select supplier", mValue = "0")]
        public Int32 mSupplierId { get; set; }
        public String mSupplierName { get; set; }
        public String mType { get; set; }
        public String mDetails { get; set; }
        public Boolean mPost { get; set; }

        public string mTransactionNo { get; set; }

        public JournalVoucherDetailCollection mJournalVoucherDetailCollection { get; set; }
        public JournalVoucherDetailCollection mDeletedJournalVoucherDetailCollection { get; set; }
        #endregion
    }
}