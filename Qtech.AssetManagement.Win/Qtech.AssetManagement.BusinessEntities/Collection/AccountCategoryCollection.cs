// -----------------------------------------------------------------------
// <copyright file="AccountTypeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AccountTypeCollection class is designed to work with lists of instances of AccountType.
    /// </summary>
    public class AccountTypeCollection : BusinessCollectionBase<AccountType>
    {

        /// <summary>
        /// Initializes a new instance of the AccountTypeCollection class.
        /// </summary>
        public AccountTypeCollection() { }

        /// <summary>
        /// Initializes a new instance of the AccountTypeCollection class.
        /// </summary>
        public AccountTypeCollection(IList<AccountType> initialList) : base(initialList) { }
    }
}