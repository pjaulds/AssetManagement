// -----------------------------------------------------------------------
// <copyright file="MaintenanceRequestTypeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The MaintenanceRequestTypeCollection class is designed to work with lists of instances of MaintenanceRequestType.
    /// </summary>
    public class MaintenanceRequestTypeCollection : BusinessCollectionBase<MaintenanceRequestType>
    {

        /// <summary>
        /// Initializes a new instance of the MaintenanceRequestTypeCollection class.
        /// </summary>
        public MaintenanceRequestTypeCollection() { }

        /// <summary>
        /// Initializes a new instance of the MaintenanceRequestTypeCollection class.
        /// </summary>
        public MaintenanceRequestTypeCollection(IList<MaintenanceRequestType> initialList) : base(initialList) { }
    }
}