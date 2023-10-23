// -----------------------------------------------------------------------
// <copyright file="MaintenanceJobTypeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The MaintenanceJobTypeCollection class is designed to work with lists of instances of MaintenanceJobType.
    /// </summary>
    public class MaintenanceJobTypeCollection : BusinessCollectionBase<MaintenanceJobType>
    {

        /// <summary>
        /// Initializes a new instance of the MaintenanceJobTypeCollection class.
        /// </summary>
        public MaintenanceJobTypeCollection() { }

        /// <summary>
        /// Initializes a new instance of the MaintenanceJobTypeCollection class.
        /// </summary>
        public MaintenanceJobTypeCollection(IList<MaintenanceJobType> initialList) : base(initialList) { }
    }
}