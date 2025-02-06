// -----------------------------------------------------------------------
// <copyright file="AssetCapitalizeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetCapitalizeCollection class is designed to work with lists of instances of AssetCapitalize.
    /// </summary>
    public class AssetCapitalizeCollection : BusinessCollectionBase<AssetCapitalize>
    {

        /// <summary>
        /// Initializes a new instance of the AssetCapitalizeCollection class.
        /// </summary>
        public AssetCapitalizeCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetCapitalizeCollection class.
        /// </summary>
        public AssetCapitalizeCollection(IList<AssetCapitalize> initialList) : base(initialList) { }
    }
}