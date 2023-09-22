// -----------------------------------------------------------------------
// <copyright file="ReceivingCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ReceivingCollection class is designed to work with lists of instances of Receiving.
    /// </summary>
    public class ReceivingCollection : BusinessCollectionBase<Receiving>
    {

        /// <summary>
        /// Initializes a new instance of the ReceivingCollection class.
        /// </summary>
        public ReceivingCollection() { }

        /// <summary>
        /// Initializes a new instance of the ReceivingCollection class.
        /// </summary>
        public ReceivingCollection(IList<Receiving> initialList) : base(initialList) { }
    }
}