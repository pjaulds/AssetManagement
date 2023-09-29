// -----------------------------------------------------------------------
// <copyright file="PurchaseVoucherCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PurchaseVoucherCollection class is designed to work with lists of instances of PurchaseVoucher.
    /// </summary>
    public class PurchaseVoucherCollection : BusinessCollectionBase<PurchaseVoucher>
    {

        /// <summary>
        /// Initializes a new instance of the PurchaseVoucherCollection class.
        /// </summary>
        public PurchaseVoucherCollection() { }

        /// <summary>
        /// Initializes a new instance of the PurchaseVoucherCollection class.
        /// </summary>
        public PurchaseVoucherCollection(IList<PurchaseVoucher> initialList) : base(initialList) { }
    }
}