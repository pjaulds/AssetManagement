// -----------------------------------------------------------------------
// <copyright file="FaultSymptomsCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FaultSymptomsCollection class is designed to work with lists of instances of FaultSymptoms.
    /// </summary>
    public class FaultSymptomsCollection : BusinessCollectionBase<FaultSymptoms>
    {

        /// <summary>
        /// Initializes a new instance of the FaultSymptomsCollection class.
        /// </summary>
        public FaultSymptomsCollection() { }

        /// <summary>
        /// Initializes a new instance of the FaultSymptomsCollection class.
        /// </summary>
        public FaultSymptomsCollection(IList<FaultSymptoms> initialList) : base(initialList) { }
    }
}