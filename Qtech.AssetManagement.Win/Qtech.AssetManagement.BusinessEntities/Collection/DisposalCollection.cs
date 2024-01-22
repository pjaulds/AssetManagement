// -----------------------------------------------------------------------
// <copyright file="DisposalCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The DisposalCollection class is designed to work with lists of instances of Disposal.
    /// </summary>
    public class DisposalCollection : BusinessCollectionBase<Disposal>
    {

        /// <summary>
        /// Initializes a new instance of the DisposalCollection class.
        /// </summary>
        public DisposalCollection() { }

        /// <summary>
        /// Initializes a new instance of the DisposalCollection class.
        /// </summary>
        public DisposalCollection(IList<Disposal> initialList) : base(initialList) { }
    }
}