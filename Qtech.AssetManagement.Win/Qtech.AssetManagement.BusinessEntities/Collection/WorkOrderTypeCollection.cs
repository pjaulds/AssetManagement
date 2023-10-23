// -----------------------------------------------------------------------
// <copyright file="WorkOrderTypeCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The WorkOrderTypeCollection class is designed to work with lists of instances of WorkOrderType.
    /// </summary>
    public class WorkOrderTypeCollection : BusinessCollectionBase<WorkOrderType>
    {

        /// <summary>
        /// Initializes a new instance of the WorkOrderTypeCollection class.
        /// </summary>
        public WorkOrderTypeCollection() { }

        /// <summary>
        /// Initializes a new instance of the WorkOrderTypeCollection class.
        /// </summary>
        public WorkOrderTypeCollection(IList<WorkOrderType> initialList) : base(initialList) { }
    }
}