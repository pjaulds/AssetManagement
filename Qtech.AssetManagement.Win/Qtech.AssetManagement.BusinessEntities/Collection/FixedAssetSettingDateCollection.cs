// -----------------------------------------------------------------------
// <copyright file="FixedAssetSettingDateCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FixedAssetSettingDateCollection class is designed to work with lists of instances of FixedAssetSettingDate.
    /// </summary>
    public class FixedAssetSettingDateCollection : BusinessCollectionBase<FixedAssetSettingDate>
    {

        /// <summary>
        /// Initializes a new instance of the FixedAssetSettingDateCollection class.
        /// </summary>
        public FixedAssetSettingDateCollection() { }

        /// <summary>
        /// Initializes a new instance of the FixedAssetSettingDateCollection class.
        /// </summary>
        public FixedAssetSettingDateCollection(IList<FixedAssetSettingDate> initialList) : base(initialList) { }
    }
}