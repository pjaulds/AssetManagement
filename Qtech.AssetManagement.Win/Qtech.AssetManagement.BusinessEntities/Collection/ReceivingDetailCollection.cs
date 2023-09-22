// -----------------------------------------------------------------------
// <copyright file="ReceivingDetailCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ReceivingDetailCollection class is designed to work with lists of instances of ReceivingDetail.
    /// </summary>
    public class ReceivingDetailCollection : BusinessCollectionBase<ReceivingDetail>
    {

        /// <summary>
        /// Initializes a new instance of the ReceivingDetailCollection class.
        /// </summary>
        public ReceivingDetailCollection() { }

        /// <summary>
        /// Initializes a new instance of the ReceivingDetailCollection class.
        /// </summary>
        public ReceivingDetailCollection(IList<ReceivingDetail> initialList) : base(initialList) { }
    }
}