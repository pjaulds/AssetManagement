// -----------------------------------------------------------------------
// <copyright file="JournalVoucherDetailCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The JournalVoucherDetailCollection class is designed to work with lists of instances of JournalVoucherDetail.
    /// </summary>
    public class JournalVoucherDetailCollection : BusinessCollectionBase<JournalVoucherDetail>
    {

        /// <summary>
        /// Initializes a new instance of the JournalVoucherDetailCollection class.
        /// </summary>
        public JournalVoucherDetailCollection() { }

        /// <summary>
        /// Initializes a new instance of the JournalVoucherDetailCollection class.
        /// </summary>
        public JournalVoucherDetailCollection(IList<JournalVoucherDetail> initialList) : base(initialList) { }
    }
}