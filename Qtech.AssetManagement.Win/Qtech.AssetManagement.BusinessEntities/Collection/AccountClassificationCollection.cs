// -----------------------------------------------------------------------
// <copyright file="AccountClassificationCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AccountClassificationCollection class is designed to work with lists of instances of AccountClassification.
    /// </summary>
    public class AccountClassificationCollection : BusinessCollectionBase<AccountClassification>
    {

        /// <summary>
        /// Initializes a new instance of the AccountClassificationCollection class.
        /// </summary>
        public AccountClassificationCollection() { }

        /// <summary>
        /// Initializes a new instance of the AccountClassificationCollection class.
        /// </summary>
        public AccountClassificationCollection(IList<AccountClassification> initialList) : base(initialList) { }
    }
}