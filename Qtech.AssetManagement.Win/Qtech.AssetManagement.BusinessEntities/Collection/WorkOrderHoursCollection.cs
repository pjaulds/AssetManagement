// -----------------------------------------------------------------------
// <copyright file="WorkOrderHoursCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The WorkOrderHoursCollection class is designed to work with lists of instances of WorkOrderHours.
    /// </summary>
    public class WorkOrderHoursCollection : BusinessCollectionBase<WorkOrderHours>
    {

        /// <summary>
        /// Initializes a new instance of the WorkOrderHoursCollection class.
        /// </summary>
        public WorkOrderHoursCollection() { }

        /// <summary>
        /// Initializes a new instance of the WorkOrderHoursCollection class.
        /// </summary>
        public WorkOrderHoursCollection(IList<WorkOrderHours> initialList) : base(initialList) { }
    }
}