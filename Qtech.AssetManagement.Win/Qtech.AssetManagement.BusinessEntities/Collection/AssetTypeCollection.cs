// -----------------------------------------------------------------------
// <copyright file="AssetTypeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetTypeCollection class is designed to work with lists of instances of AssetType.
    /// </summary>
    public class AssetTypeCollection : BusinessCollectionBase<AssetType>
    {

        /// <summary>
        /// Initializes a new instance of the AssetTypeCollection class.
        /// </summary>
        public AssetTypeCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetTypeCollection class.
        /// </summary>
        public AssetTypeCollection(IList<AssetType> initialList) : base(initialList) { }
    }
}