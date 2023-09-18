using System.Collections.ObjectModel;
using System.Collections.Generic;
namespace Qtech.AssetManagement.BusinessEntities
{
    public class AuditCollection : Collection<BusinessEntities.Audit>
    {
        /// <summary>
        /// Initializes a new instance of the BankCollection class.
        /// </summary>
        public AuditCollection() { }

        /// <summary>
        /// Initializes a new instance of the BankCollection class.
        /// </summary>
        public AuditCollection(IList<BusinessEntities.Audit> initialList) : base(initialList) { }
    }
}
