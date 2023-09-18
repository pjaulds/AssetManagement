// -----------------------------------------------------------------------
// <copyright file="DepreciationMethodCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The DepreciationMethodCollection class is designed to work with lists of instances of DepreciationMethod.
    /// </summary>
    public class DepreciationMethodCollection : BusinessCollectionBase<DepreciationMethod>
    {

        /// <summary>
        /// Initializes a new instance of the DepreciationMethodCollection class.
        /// </summary>
        public DepreciationMethodCollection() { }

        /// <summary>
        /// Initializes a new instance of the DepreciationMethodCollection class.
        /// </summary>
        public DepreciationMethodCollection(IList<DepreciationMethod> initialList) : base(initialList) { }
    }
}