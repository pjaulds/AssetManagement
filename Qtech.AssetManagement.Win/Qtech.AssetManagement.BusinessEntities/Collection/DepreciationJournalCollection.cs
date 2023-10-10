// -----------------------------------------------------------------------
// <copyright file="DepreciationJournalCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The DepreciationJournalCollection class is designed to work with lists of instances of DepreciationJournal.
    /// </summary>
    public class DepreciationJournalCollection : BusinessCollectionBase<DepreciationJournal>
    {

        /// <summary>
        /// Initializes a new instance of the DepreciationJournalCollection class.
        /// </summary>
        public DepreciationJournalCollection() { }

        /// <summary>
        /// Initializes a new instance of the DepreciationJournalCollection class.
        /// </summary>
        public DepreciationJournalCollection(IList<DepreciationJournal> initialList) : base(initialList) { }
    }
}