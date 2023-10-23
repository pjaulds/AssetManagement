// -----------------------------------------------------------------------
// <copyright file="FaultAreaCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FaultAreaCollection class is designed to work with lists of instances of FaultArea.
    /// </summary>
    public class FaultAreaCollection : BusinessCollectionBase<FaultArea>
    {

        /// <summary>
        /// Initializes a new instance of the FaultAreaCollection class.
        /// </summary>
        public FaultAreaCollection() { }

        /// <summary>
        /// Initializes a new instance of the FaultAreaCollection class.
        /// </summary>
        public FaultAreaCollection(IList<FaultArea> initialList) : base(initialList) { }
    }
}