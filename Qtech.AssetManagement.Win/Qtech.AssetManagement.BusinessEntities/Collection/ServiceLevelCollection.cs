// -----------------------------------------------------------------------
// <copyright file="ServiceLevelCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ServiceLevelCollection class is designed to work with lists of instances of ServiceLevel.
    /// </summary>
    public class ServiceLevelCollection : BusinessCollectionBase<ServiceLevel>
    {

        /// <summary>
        /// Initializes a new instance of the ServiceLevelCollection class.
        /// </summary>
        public ServiceLevelCollection() { }

        /// <summary>
        /// Initializes a new instance of the ServiceLevelCollection class.
        /// </summary>
        public ServiceLevelCollection(IList<ServiceLevel> initialList) : base(initialList) { }
    }
}