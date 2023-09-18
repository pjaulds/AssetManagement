// -----------------------------------------------------------------------
// <copyright file="DepreciationExpenseAccountCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The DepreciationExpenseAccountCollection class is designed to work with lists of instances of DepreciationExpenseAccount.
    /// </summary>
    public class DepreciationExpenseAccountCollection : BusinessCollectionBase<DepreciationExpenseAccount>
    {

        /// <summary>
        /// Initializes a new instance of the DepreciationExpenseAccountCollection class.
        /// </summary>
        public DepreciationExpenseAccountCollection() { }

        /// <summary>
        /// Initializes a new instance of the DepreciationExpenseAccountCollection class.
        /// </summary>
        public DepreciationExpenseAccountCollection(IList<DepreciationExpenseAccount> initialList) : base(initialList) { }
    }
}