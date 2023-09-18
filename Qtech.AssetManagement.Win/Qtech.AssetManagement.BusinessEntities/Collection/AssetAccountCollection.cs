// -----------------------------------------------------------------------
// <copyright file="AssetAccountCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AssetAccountCollection class is designed to work with lists of instances of AssetAccount.
    /// </summary>
    public class AssetAccountCollection : BusinessCollectionBase<AssetAccount>
    {

        /// <summary>
        /// Initializes a new instance of the AssetAccountCollection class.
        /// </summary>
        public AssetAccountCollection() { }

        /// <summary>
        /// Initializes a new instance of the AssetAccountCollection class.
        /// </summary>
        public AssetAccountCollection(IList<AssetAccount> initialList) : base(initialList) { }
    }
}