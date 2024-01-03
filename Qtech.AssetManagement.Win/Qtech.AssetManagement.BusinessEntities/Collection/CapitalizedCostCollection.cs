// -----------------------------------------------------------------------
// <copyright file="CapitalizedCostCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The CapitalizedCostCollection class is designed to work with lists of instances of CapitalizedCost.
    /// </summary>
    public class CapitalizedCostCollection : BusinessCollectionBase<CapitalizedCost>
    {

        /// <summary>
        /// Initializes a new instance of the CapitalizedCostCollection class.
        /// </summary>
        public CapitalizedCostCollection() { }

        /// <summary>
        /// Initializes a new instance of the CapitalizedCostCollection class.
        /// </summary>
        public CapitalizedCostCollection(IList<CapitalizedCost> initialList) : base(initialList) { }
    }
}