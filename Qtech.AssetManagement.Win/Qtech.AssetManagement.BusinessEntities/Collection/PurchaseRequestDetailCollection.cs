// -----------------------------------------------------------------------
// <copyright file="PurchaseRequestDetailCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PurchaseRequestDetailCollection class is designed to work with lists of instances of PurchaseRequestDetail.
    /// </summary>
    public class PurchaseRequestDetailCollection : BusinessCollectionBase<PurchaseRequestDetail>
    {

        /// <summary>
        /// Initializes a new instance of the PurchaseRequestDetailCollection class.
        /// </summary>
        public PurchaseRequestDetailCollection() { }

        /// <summary>
        /// Initializes a new instance of the PurchaseRequestDetailCollection class.
        /// </summary>
        public PurchaseRequestDetailCollection(IList<PurchaseRequestDetail> initialList) : base(initialList) { }
    }
}