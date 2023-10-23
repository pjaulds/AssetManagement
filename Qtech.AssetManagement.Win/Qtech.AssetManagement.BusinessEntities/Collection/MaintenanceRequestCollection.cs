// -----------------------------------------------------------------------
// <copyright file="MaintenanceRequestCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The MaintenanceRequestCollection class is designed to work with lists of instances of MaintenanceRequest.
    /// </summary>
    public class MaintenanceRequestCollection : BusinessCollectionBase<MaintenanceRequest>
    {

        /// <summary>
        /// Initializes a new instance of the MaintenanceRequestCollection class.
        /// </summary>
        public MaintenanceRequestCollection() { }

        /// <summary>
        /// Initializes a new instance of the MaintenanceRequestCollection class.
        /// </summary>
        public MaintenanceRequestCollection(IList<MaintenanceRequest> initialList) : base(initialList) { }
    }
}