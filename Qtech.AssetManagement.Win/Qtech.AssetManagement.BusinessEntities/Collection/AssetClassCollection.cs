// -----------------------------------------------------------------------
// <copyright file="AssetClassCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetClassCollection class is designed to work with lists of instances of AssetClass.
    /// </summary>
    public class AssetClassCollection : BusinessCollectionBase<AssetClass>
    {

        /// <summary>
        /// Initializes a new instance of the AssetClassCollection class.
        /// </summary>
        public AssetClassCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetClassCollection class.
        /// </summary>
        public AssetClassCollection(IList<AssetClass> initialList) : base(initialList) { }
    }
}