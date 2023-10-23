// -----------------------------------------------------------------------
// <copyright file="TradeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The TradeCollection class is designed to work with lists of instances of Trade.
    /// </summary>
    public class TradeCollection : BusinessCollectionBase<Trade>
    {

        /// <summary>
        /// Initializes a new instance of the TradeCollection class.
        /// </summary>
        public TradeCollection() { }

        /// <summary>
        /// Initializes a new instance of the TradeCollection class.
        /// </summary>
        public TradeCollection(IList<Trade> initialList) : base(initialList) { }
    }
}