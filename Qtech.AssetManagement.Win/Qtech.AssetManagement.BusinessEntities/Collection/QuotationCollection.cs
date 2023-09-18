// -----------------------------------------------------------------------
// <copyright file="QuotationCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The QuotationCollection class is designed to work with lists of instances of Quotation.
    /// </summary>
    public class QuotationCollection : BusinessCollectionBase<Quotation>
    {

        /// <summary>
        /// Initializes a new instance of the QuotationCollection class.
        /// </summary>
        public QuotationCollection() { }

        /// <summary>
        /// Initializes a new instance of the QuotationCollection class.
        /// </summary>
        public QuotationCollection(IList<Quotation> initialList) : base(initialList) { }
    }
}