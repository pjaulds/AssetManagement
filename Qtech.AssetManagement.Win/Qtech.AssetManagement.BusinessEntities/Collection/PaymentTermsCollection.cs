// -----------------------------------------------------------------------
// <copyright file="PaymentTermsCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PaymentTermsCollection class is designed to work with lists of instances of PaymentTerms.
    /// </summary>
    public class PaymentTermsCollection : BusinessCollectionBase<PaymentTerms>
    {

        /// <summary>
        /// Initializes a new instance of the PaymentTermsCollection class.
        /// </summary>
        public PaymentTermsCollection() { }

        /// <summary>
        /// Initializes a new instance of the PaymentTermsCollection class.
        /// </summary>
        public PaymentTermsCollection(IList<PaymentTerms> initialList) : base(initialList) { }
    }
}