// -----------------------------------------------------------------------
// <copyright file="UsersCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The UsersCollection class is designed to work with lists of instances of Users.
    /// </summary>
    public class UsersCollection : BusinessCollectionBase<Users>
    {

        /// <summary>
        /// Initializes a new instance of the UsersCollection class.
        /// </summary>
        public UsersCollection() { }

        /// <summary>
        /// Initializes a new instance of the UsersCollection class.
        /// </summary>
        public UsersCollection(IList<Users> initialList) : base(initialList) { }
    }
}