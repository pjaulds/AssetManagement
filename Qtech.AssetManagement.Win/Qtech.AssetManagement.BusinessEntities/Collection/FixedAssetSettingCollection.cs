// -----------------------------------------------------------------------
// <copyright file="FixedAssetSettingCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The FixedAssetSettingCollection class is designed to work with lists of instances of FixedAssetSetting.
    /// </summary>
    public class FixedAssetSettingCollection : BusinessCollectionBase<FixedAssetSetting>
    {

        /// <summary>
        /// Initializes a new instance of the FixedAssetSettingCollection class.
        /// </summary>
        public FixedAssetSettingCollection() { }

        /// <summary>
        /// Initializes a new instance of the FixedAssetSettingCollection class.
        /// </summary>
        public FixedAssetSettingCollection(IList<FixedAssetSetting> initialList) : base(initialList) { }
    }
}