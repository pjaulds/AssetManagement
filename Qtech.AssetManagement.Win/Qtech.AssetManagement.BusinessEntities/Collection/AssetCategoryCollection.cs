// -----------------------------------------------------------------------
// <copyright file="AssetCategoryCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetCategoryCollection class is designed to work with lists of instances of AssetCategory.
    /// </summary>
    public class AssetCategoryCollection : BusinessCollectionBase<AssetCategory>
    {

        /// <summary>
        /// Initializes a new instance of the AssetCategoryCollection class.
        /// </summary>
        public AssetCategoryCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetCategoryCollection class.
        /// </summary>
        public AssetCategoryCollection(IList<AssetCategory> initialList) : base(initialList) { }
    }
}