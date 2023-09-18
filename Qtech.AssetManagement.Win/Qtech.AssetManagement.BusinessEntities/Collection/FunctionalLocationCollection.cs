// -----------------------------------------------------------------------
// <copyright file="FunctionalLocationCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FunctionalLocationCollection class is designed to work with lists of instances of FunctionalLocation.
    /// </summary>
    public class FunctionalLocationCollection : BusinessCollectionBase<FunctionalLocation>
    {

        /// <summary>
        /// Initializes a new instance of the FunctionalLocationCollection class.
        /// </summary>
        public FunctionalLocationCollection() { }

        /// <summary>
        /// Initializes a new instance of the FunctionalLocationCollection class.
        /// </summary>
        public FunctionalLocationCollection(IList<FunctionalLocation> initialList) : base(initialList) { }
    }
}