// -----------------------------------------------------------------------
// <copyright file="ChartOfAccountCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ChartOfAccountCollection class is designed to work with lists of instances of ChartOfAccount.
    /// </summary>
    public class ChartOfAccountCollection : BusinessCollectionBase<ChartOfAccount>
    {

        /// <summary>
        /// Initializes a new instance of the ChartOfAccountCollection class.
        /// </summary>
        public ChartOfAccountCollection() { }

        /// <summary>
        /// Initializes a new instance of the ChartOfAccountCollection class.
        /// </summary>
        public ChartOfAccountCollection(IList<ChartOfAccount> initialList) : base(initialList) { }
    }
}