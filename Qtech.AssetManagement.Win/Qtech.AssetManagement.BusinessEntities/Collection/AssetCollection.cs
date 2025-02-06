// -----------------------------------------------------------------------
// <copyright file="AssetCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetCollection class is designed to work with lists of instances of Asset.
    /// </summary>
    public class AssetCollection : BusinessCollectionBase<Asset>
    {

        /// <summary>
        /// Initializes a new instance of the AssetCollection class.
        /// </summary>
        public AssetCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetCollection class.
        /// </summary>
        public AssetCollection(IList<Asset> initialList) : base(initialList) { }
    }
}