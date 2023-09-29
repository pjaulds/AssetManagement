// -----------------------------------------------------------------------
// <copyright file="PaymentModeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PaymentModeCollection class is designed to work with lists of instances of PaymentMode.
    /// </summary>
    public class PaymentModeCollection : BusinessCollectionBase<PaymentMode>
    {

        /// <summary>
        /// Initializes a new instance of the PaymentModeCollection class.
        /// </summary>
        public PaymentModeCollection() { }

        /// <summary>
        /// Initializes a new instance of the PaymentModeCollection class.
        /// </summary>
        public PaymentModeCollection(IList<PaymentMode> initialList) : base(initialList) { }
    }
}