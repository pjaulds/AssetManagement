// -----------------------------------------------------------------------
// <copyright file="AveragingMethodCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The AveragingMethodCollection class is designed to work with lists of instances of AveragingMethod.
    /// </summary>
    public class AveragingMethodCollection : BusinessCollectionBase<AveragingMethod>
    {

        /// <summary>
        /// Initializes a new instance of the AveragingMethodCollection class.
        /// </summary>
        public AveragingMethodCollection() { }

        /// <summary>
        /// Initializes a new instance of the AveragingMethodCollection class.
        /// </summary>
        public AveragingMethodCollection(IList<AveragingMethod> initialList) : base(initialList) { }
    }
}