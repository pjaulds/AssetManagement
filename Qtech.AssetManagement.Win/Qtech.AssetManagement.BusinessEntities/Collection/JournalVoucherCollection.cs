// -----------------------------------------------------------------------
// <copyright file="JournalVoucherCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The JournalVoucherCollection class is designed to work with lists of instances of JournalVoucher.
    /// </summary>
    public class JournalVoucherCollection : BusinessCollectionBase<JournalVoucher>
    {

        /// <summary>
        /// Initializes a new instance of the JournalVoucherCollection class.
        /// </summary>
        public JournalVoucherCollection() { }

        /// <summary>
        /// Initializes a new instance of the JournalVoucherCollection class.
        /// </summary>
        public JournalVoucherCollection(IList<JournalVoucher> initialList) : base(initialList) { }
    }
}