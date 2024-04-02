using System;
using System.ComponentModel;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    public class JournalVoucherDetail : BusinessBase
    {
        #region Public Properties

        public override Int32 mId { get; set; }
        public Int32 mJournalVoucherId { get; set; }
        public Int32 mChartOfAccountId { get; set; }
        public String mChartOfAccountCode { get; set; }
        public String mChartOfAccountName { get; set; }
        public Boolean mDebitCredit { get; set; }
        public Decimal mAmount { get; set; }

        public decimal mDebit { get; set; }
        public decimal mCredit { get; set; }
        #endregion
    }
}