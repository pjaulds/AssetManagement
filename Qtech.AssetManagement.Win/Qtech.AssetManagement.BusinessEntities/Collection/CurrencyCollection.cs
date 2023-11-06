// -----------------------------------------------------------------------
// <copyright file="CurrencyCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The CurrencyCollection class is designed to work with lists of instances of Currency.
    /// </summary>
    public class CurrencyCollection : BusinessCollectionBase<Currency>
    {

        /// <summary>
        /// Initializes a new instance of the CurrencyCollection class.
        /// </summary>
        public CurrencyCollection() { }

        /// <summary>
        /// Initializes a new instance of the CurrencyCollection class.
        /// </summary>
        public CurrencyCollection(IList<Currency> initialList) : base(initialList) { }
    }
}