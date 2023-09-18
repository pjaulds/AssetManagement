// -----------------------------------------------------------------------
// <copyright file="PersonnelCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The PersonnelCollection class is designed to work with lists of instances of Personnel.
    /// </summary>
    public class PersonnelCollection : BusinessCollectionBase<Personnel>
    {

        /// <summary>
        /// Initializes a new instance of the PersonnelCollection class.
        /// </summary>
        public PersonnelCollection() { }

        /// <summary>
        /// Initializes a new instance of the PersonnelCollection class.
        /// </summary>
        public PersonnelCollection(IList<Personnel> initialList) : base(initialList) { }
    }
}