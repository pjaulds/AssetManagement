// -----------------------------------------------------------------------
// <copyright file="FixedAssetCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FixedAssetCollection class is designed to work with lists of instances of FixedAsset.
    /// </summary>
    public class FixedAssetCollection : BusinessCollectionBase<FixedAsset>
    {

        /// <summary>
        /// Initializes a new instance of the FixedAssetCollection class.
        /// </summary>
        public FixedAssetCollection() { }

        /// <summary>
        /// Initializes a new instance of the FixedAssetCollection class.
        /// </summary>
        public FixedAssetCollection(IList<FixedAsset> initialList) : base(initialList) { }
    }
}