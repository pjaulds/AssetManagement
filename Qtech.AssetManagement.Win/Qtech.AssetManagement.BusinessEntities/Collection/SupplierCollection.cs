// -----------------------------------------------------------------------
// <copyright file="SupplierCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The SupplierCollection class is designed to work with lists of instances of Supplier.
    /// </summary>
    public class SupplierCollection : BusinessCollectionBase<Supplier>
    {

        /// <summary>
        /// Initializes a new instance of the SupplierCollection class.
        /// </summary>
        public SupplierCollection() { }

        /// <summary>
        /// Initializes a new instance of the SupplierCollection class.
        /// </summary>
        public SupplierCollection(IList<Supplier> initialList) : base(initialList) { }
    }
}