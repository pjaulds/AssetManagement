// -----------------------------------------------------------------------
// <copyright file="QuotationDetailCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The QuotationDetailCollection class is designed to work with lists of instances of QuotationDetail.
    /// </summary>
    public class QuotationDetailCollection : BusinessCollectionBase<QuotationDetail>
    {

        /// <summary>
        /// Initializes a new instance of the QuotationDetailCollection class.
        /// </summary>
        public QuotationDetailCollection() { }

        /// <summary>
        /// Initializes a new instance of the QuotationDetailCollection class.
        /// </summary>
        public QuotationDetailCollection(IList<QuotationDetail> initialList) : base(initialList) { }
    }
}