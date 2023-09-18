// -----------------------------------------------------------------------
// <copyright file="PurchaseRequestCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PurchaseRequestCollection class is designed to work with lists of instances of PurchaseRequest.
    /// </summary>
    public class PurchaseRequestCollection : BusinessCollectionBase<PurchaseRequest>
    {

        /// <summary>
        /// Initializes a new instance of the PurchaseRequestCollection class.
        /// </summary>
        public PurchaseRequestCollection() { }

        /// <summary>
        /// Initializes a new instance of the PurchaseRequestCollection class.
        /// </summary>
        public PurchaseRequestCollection(IList<PurchaseRequest> initialList) : base(initialList) { }
    }
}