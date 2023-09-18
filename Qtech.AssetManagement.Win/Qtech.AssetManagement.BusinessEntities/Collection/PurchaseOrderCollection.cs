// -----------------------------------------------------------------------
// <copyright file="PurchaseOrderCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PurchaseOrderCollection class is designed to work with lists of instances of PurchaseOrder.
    /// </summary>
    public class PurchaseOrderCollection : BusinessCollectionBase<PurchaseOrder>
    {

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderCollection class.
        /// </summary>
        public PurchaseOrderCollection() { }

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderCollection class.
        /// </summary>
        public PurchaseOrderCollection(IList<PurchaseOrder> initialList) : base(initialList) { }
    }
}