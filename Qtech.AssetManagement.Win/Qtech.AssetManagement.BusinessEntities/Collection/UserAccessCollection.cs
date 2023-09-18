// -----------------------------------------------------------------------
// <copyright file="UserAccessCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The UserAccessCollection class is designed to work with lists of instances of UserAccess.
    /// </summary>
    public class UserAccessCollection : BusinessCollectionBase<UserAccess>
    {

        /// <summary>
        /// Initializes a new instance of the UserAccessCollection class.
        /// </summary>
        public UserAccessCollection() { }

        /// <summary>
        /// Initializes a new instance of the UserAccessCollection class.
        /// </summary>
        public UserAccessCollection(IList<UserAccess> initialList) : base(initialList) { }
    }
}