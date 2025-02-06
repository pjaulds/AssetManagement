// -----------------------------------------------------------------------
// <copyright file="ProjectCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ProjectCollection class is designed to work with lists of instances of Project.
    /// </summary>
    public class ProjectCollection : BusinessCollectionBase<Project>
    {

        /// <summary>
        /// Initializes a new instance of the ProjectCollection class.
        /// </summary>
        public ProjectCollection() { }

        /// <summary>
        /// Initializes a new instance of the ProjectCollection class.
        /// </summary>
        public ProjectCollection(IList<Project> initialList) : base(initialList) { }
    }
}