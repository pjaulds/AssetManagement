// -----------------------------------------------------------------------
// <copyright file="WorkOrderCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The WorkOrderCollection class is designed to work with lists of instances of WorkOrder.
    /// </summary>
    public class WorkOrderCollection : BusinessCollectionBase<WorkOrder>
    {

        /// <summary>
        /// Initializes a new instance of the WorkOrderCollection class.
        /// </summary>
        public WorkOrderCollection() { }

        /// <summary>
        /// Initializes a new instance of the WorkOrderCollection class.
        /// </summary>
        public WorkOrderCollection(IList<WorkOrder> initialList) : base(initialList) { }
    }
}