// -----------------------------------------------------------------------
// <copyright file="UnitCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The UnitCollection class is designed to work with lists of instances of Unit.
    /// </summary>
    public class UnitCollection : BusinessCollectionBase<Unit>
    {

        /// <summary>
        /// Initializes a new instance of the UnitCollection class.
        /// </summary>
        public UnitCollection() { }

        /// <summary>
        /// Initializes a new instance of the UnitCollection class.
        /// </summary>
        public UnitCollection(IList<Unit> initialList) : base(initialList) { }
    }
}