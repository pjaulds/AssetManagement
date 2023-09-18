// -----------------------------------------------------------------------
// <copyright file="PurchaseOrderDetailCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PurchaseOrderDetailCollection class is designed to work with lists of instances of PurchaseOrderDetail.
    /// </summary>
    public class PurchaseOrderDetailCollection : BusinessCollectionBase<PurchaseOrderDetail>
    {

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderDetailCollection class.
        /// </summary>
        public PurchaseOrderDetailCollection() { }

        /// <summary>
        /// Initializes a new instance of the PurchaseOrderDetailCollection class.
        /// </summary>
        public PurchaseOrderDetailCollection(IList<PurchaseOrderDetail> initialList) : base(initialList) { }
    }
}