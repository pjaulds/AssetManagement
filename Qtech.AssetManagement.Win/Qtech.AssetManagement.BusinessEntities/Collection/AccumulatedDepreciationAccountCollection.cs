// -----------------------------------------------------------------------
// <copyright file="AccumulatedDepreciationAccountCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AccumulatedDepreciationAccountCollection class is designed to work with lists of instances of AccumulatedDepreciationAccount.
    /// </summary>
    public class AccumulatedDepreciationAccountCollection : BusinessCollectionBase<AccumulatedDepreciationAccount>
    {

        /// <summary>
        /// Initializes a new instance of the AccumulatedDepreciationAccountCollection class.
        /// </summary>
        public AccumulatedDepreciationAccountCollection() { }

        /// <summary>
        /// Initializes a new instance of the AccumulatedDepreciationAccountCollection class.
        /// </summary>
        public AccumulatedDepreciationAccountCollection(IList<AccumulatedDepreciationAccount> initialList) : base(initialList) { }
    }
}