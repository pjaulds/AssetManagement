// -----------------------------------------------------------------------
// <copyright file="CompanyProfileCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The CompanyProfileCollection class is designed to work with lists of instances of CompanyProfile.
    /// </summary>
    public class CompanyProfileCollection : BusinessCollectionBase<CompanyProfile>
    {

        /// <summary>
        /// Initializes a new instance of the CompanyProfileCollection class.
        /// </summary>
        public CompanyProfileCollection() { }

        /// <summary>
        /// Initializes a new instance of the CompanyProfileCollection class.
        /// </summary>
        public CompanyProfileCollection(IList<CompanyProfile> initialList) : base(initialList) { }
    }
}