// -----------------------------------------------------------------------
// <copyright file="AccountGroupCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AccountGroupCollection class is designed to work with lists of instances of AccountGroup.
    /// </summary>
    public class AccountGroupCollection : BusinessCollectionBase<AccountGroup>
    {

        /// <summary>
        /// Initializes a new instance of the AccountGroupCollection class.
        /// </summary>
        public AccountGroupCollection() { }

        /// <summary>
        /// Initializes a new instance of the AccountGroupCollection class.
        /// </summary>
        public AccountGroupCollection(IList<AccountGroup> initialList) : base(initialList) { }
    }
}