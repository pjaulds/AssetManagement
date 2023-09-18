// -----------------------------------------------------------------------
// <copyright file="ProductCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ProductCollection class is designed to work with lists of instances of Product.
    /// </summary>
    public class ProductCollection : BusinessCollectionBase<Product>
    {

        /// <summary>
        /// Initializes a new instance of the ProductCollection class.
        /// </summary>
        public ProductCollection() { }

        /// <summary>
        /// Initializes a new instance of the ProductCollection class.
        /// </summary>
        public ProductCollection(IList<Product> initialList) : base(initialList) { }
    }
}