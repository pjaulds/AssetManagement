// -----------------------------------------------------------------------
// <copyright file="FixedAssetCapitalizedCostCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FixedAssetCapitalizedCostCollection class is designed to work with lists of instances of FixedAssetCapitalizedCost.
    /// </summary>
    public class FixedAssetCapitalizedCostCollection : BusinessCollectionBase<FixedAssetCapitalizedCost>
    {

        /// <summary>
        /// Initializes a new instance of the FixedAssetCapitalizedCostCollection class.
        /// </summary>
        public FixedAssetCapitalizedCostCollection() { }

        /// <summary>
        /// Initializes a new instance of the FixedAssetCapitalizedCostCollection class.
        /// </summary>
        public FixedAssetCapitalizedCostCollection(IList<FixedAssetCapitalizedCost> initialList) : base(initialList) { }
    }
}