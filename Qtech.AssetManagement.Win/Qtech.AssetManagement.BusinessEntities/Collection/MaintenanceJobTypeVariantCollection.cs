// -----------------------------------------------------------------------
// <copyright file="MaintenanceJobTypeVariantCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The MaintenanceJobTypeVariantCollection class is designed to work with lists of instances of MaintenanceJobTypeVariant.
    /// </summary>
    public class MaintenanceJobTypeVariantCollection : BusinessCollectionBase<MaintenanceJobTypeVariant>
    {

        /// <summary>
        /// Initializes a new instance of the MaintenanceJobTypeVariantCollection class.
        /// </summary>
        public MaintenanceJobTypeVariantCollection() { }

        /// <summary>
        /// Initializes a new instance of the MaintenanceJobTypeVariantCollection class.
        /// </summary>
        public MaintenanceJobTypeVariantCollection(IList<MaintenanceJobTypeVariant> initialList) : base(initialList) { }
    }
}